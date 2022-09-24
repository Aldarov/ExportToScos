using Common.Interfaces.Services;
using Common.Models.Exceptions;
using Common.Models.Query;
using Dapper;
using GisScos.BLL.Context;
using GisScos.BLL.Interfaces;
using GisScos.BLL.Models.Export;
using GisScos.BLL.Models.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace GisScos.BLL.Services.Export
{
    /// <summary>
    /// Сервис экспорта данных в ГИС СЦОС
    /// </summary>
    public class ExportService : IExportService
    {
        private readonly AppSettings _settings;
        private readonly DapperContext _context;
        private readonly IHTTPClientService _httpClientService;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <param name="httpClientService"></param>
        public ExportService(DapperContext context, IOptions<AppSettings> settings, IHTTPClientService httpClientService)
        {
            _settings = settings.Value;
            _context = context;
            _httpClientService = httpClientService;
        }

        /// <summary>
        /// Заполнение очереди и отправка данных в ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        public async Task QueueFillingAndExportData()
        {
            if (!_settings.EnableExport)
                return;

            //сохранить данные в локальные таблицах, после предыдущей успешной отправки данных в ГИС СЦОС
            var unsavePackages = await GetUnsavePackages();
            if (unsavePackages.Any())
            {
                using var connection = _context.CreateConnection();
                foreach (var package in unsavePackages)
                {
                    await SaveExportPackageToLocalTable(package, connection);
                }
            }

            unsavePackages = await GetUnsavePackages();
            if (unsavePackages.Any())
            {
                throw new Exception("Данные из предыдущей выгрузки в ГИС СЦОС несохранены в локальные таблицы");
            }

            //проверка неотправленных пакетов в очереди, если такие есть, то делаем повторную отправку.
            await ExportToSCOS();

            //проверка неотправленных пакетов в очереди, если все еще есть генерируем ошибку
            var unsentPackages = await GetUnsentPackages();
            if (unsentPackages.Any())
            {
                throw new Exception("Данные из предыдущей выгрузки в ГИС СЦОС не отправляются");
            }

            //заполнение очереди отправки новыми данными
            await QueueFilling();

            //отправка пакетов из очереди
            await ExportToSCOS();
        }

        /// <summary>
        /// Заполнение очереди отправки данных в ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        private async Task QueueFilling()
        {
            var entities = await GetExportEntities();

            using var connection = _context.CreateConnection();
            foreach (var entity in entities)
            {
                try
                {
                    await connection.ExecuteAsync(
                        $@"exec {entity.MergeProcedure}"
                    );
                }
                catch (Exception e)
                {
                    throw new Exception(
                        $"Ошибка при заполнении очереди на отправку данных в ГИС СЦОС (mergeProcedure - {entity.MergeProcedure}). " +
                        $"Ошибка: {e.InnerException?.Message ?? e.Message}"
                    ); 
                }
            }
        }

        /// <summary>
        /// Отправка пакетов из очереди в ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        public async Task ExportToSCOS()
        {
            //Получаем неотправленные данные
            var unsentPackages = await GetUnsentPackages();
            if (unsentPackages?.Count() > 0)
            {
                using var connection = _context.CreateConnection();
                foreach (var package in unsentPackages)
                {
                    string? response = null;

                    if ((package.ActionName == ExportActionTypes.Insert) || (package.ActionName == ExportActionTypes.Update))
                    {
                        var url = $"{_settings!.Host}{_settings!.BaseUrl}{package.EntityExportUrl}";
                        try
                        {
                            response = await _httpClientService.Post(url, package.ExportData,
                                new Dictionary<string, string>() {{ "X-CN-UUID", _settings?.AccessKeyToScos! }}
                            );
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Ошибка при отправке, добавленных/измененных, данных в ГИС СЦОС " +
                                $"(url - {url}, data - {package.ExportData}). " +
                                $"Ошибка: {e.InnerException?.Message ?? e.Message}"
                            );
                        }
                    }
                    else if (package.ActionName == ExportActionTypes.Delete)
                    {
                        var baseUrl = $"{_settings?.Host}{_settings?.BaseUrl}";
                        string url = "";

                        if (package?.EntityId == 4) //StudyPlansDisciplines
                        {
                            var studyPlanScosId = JObject.Parse(package.ExportData!)["study_plan_scos_id"]?.ToString();
                            var disciplineScosId = JObject.Parse(package.ExportData!)["discipline_scos_id"]?.ToString();
                            var semester = JObject.Parse(package.ExportData!)["semester"]?.ToString();

                            if (studyPlanScosId == null || disciplineScosId == null || semester == null)
                            {
                                throw new Exception($"Ошибка при отправке удаленных данных в ГИС СЦОС. " +
                                    $"Отсутсвует один из идентификаторов необходимых для удаления данных. " +
                                    $"(package_id - {package.Id}, study_plan_scos_id - {studyPlanScosId}, discipline_scos_id - {disciplineScosId}, semester - {semester})");
                            }

                            url = baseUrl + $"study_plans/{studyPlanScosId}/disciplines/{disciplineScosId}?semester={semester}";
                        }
                        else if (package?.EntityId == 6) //StudyPlansStudents
                        {
                            var studyPlanScosId = JObject.Parse(package.ExportData!)["study_plan_scos_id"]?.ToString();
                            var studentScosId = JObject.Parse(package.ExportData!)["student_scos_id"]?.ToString();

                            if (studyPlanScosId == null || studentScosId == null)
                            {
                                throw new Exception($"Ошибка при отправке удаленных данных в ГИС СЦОС. " +
                                    $"Отсутсвует один из идентификаторов необходимых для удаления данных. " +
                                    $"(package_id - {package.Id}, study_plan_scos_id - {studyPlanScosId}, student_scos_id - {studentScosId})");
                            }

                            url = baseUrl + $"students/{studentScosId}/study_plans/{studyPlanScosId}";
                        }
                        else
                        {
                            url = baseUrl + $"{package?.EntityExportUrl}/{package?.ExportData}";
                        }

                        try
                        {
                            response = await _httpClientService.Delete(url, new Dictionary<string, string>() {{ "X-CN-UUID", _settings?.AccessKeyToScos! }});
                        }
                        catch(Exception e)
                        {
                            response = e.Message;
                        }
                    }

                    if (response != null)
                    {
                        //сохраняем ответы
                        await connection.ExecuteAsync(
                            @"update ExportQueue
                            set response_data = @response, export_status_id = 2
                            where id = @id",
                            new { id = package?.Id, response }
                        );
                        package!.ResponseData = response;

                        await SaveExportPackageToLocalTable(package, connection);
                    }
                }
            }
        }

        /// <summary>
        /// Получить сущности для экспорта в ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<ExportEntityModel>> GetExportEntities()
        {
            IEnumerable<ExportEntityModel> res;

            using (var connection = _context.CreateConnection())
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                res = await (connection.QueryAsync<ExportEntityModel>(
                    @"select id, url_name, merge_procedure, sort_num
                    from Entities
                    where enable = 1
                    order by sort_num"
                ).ConfigureAwait(false));
            }

            return res;
        }

        /// <summary>
        /// Получить неотправленные пакеты из очереди отправки данных в ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<ExportPackageModel>> GetUnsentPackages()
        {
            IEnumerable<ExportPackageModel> res;

            using (var connection = _context.CreateConnection())
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                res = await (connection.QueryAsync<ExportPackageModel>(
                    @"select q.id, q.entity_id, e.url_name as entity_export_url, e.table_name as entity_table_name, e.json_object_name,
	                    q.export_status_id, q.action_name, q.export_data, q.response_data, q.create_date
                    from ExportQueue q
	                    inner join Entities e on q.entity_id = e.id
                    where q.export_status_id = 1
                    order by action_name desc, (case when action_name = 'DELETE' then -sort_num else sort_num end) , q.create_date"
                ).ConfigureAwait(false));
            }

            return res;
        }

        /// <summary>
        /// Получить пакеты, несохраненные в локальных таблицах, после успешной отправки данных в ГИС СЦОС
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<ExportPackageModel>> GetUnsavePackages()
        {
            IEnumerable<ExportPackageModel> res;

            using (var connection = _context.CreateConnection())
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;
                res = await (connection.QueryAsync<ExportPackageModel>(
                    @"select q.id, q.entity_id, e.url_name as entity_export_url, e.table_name as entity_table_name, e.json_object_name,
	                    q.export_status_id, q.action_name, q.export_data, q.response_data, q.create_date
                    from ExportQueue q
	                    inner join Entities e on q.entity_id = e.id
                    where q.export_status_id = 2 and q.save_export_data_to_local_table = 0
                    order by action_name desc, (case when action_name = 'DELETE' then -sort_num else sort_num end) , q.create_date"
                ).ConfigureAwait(false));
            }

            return res;
        }

        /// <summary>
        /// Сохранить отправленные данные в локальные таблицы
        /// </summary>
        /// <param name="package">Отправленный пакет</param>
        /// <param name="connection">Соединение с БД</param>
        /// <returns></returns>
        public async Task SaveExportPackageToLocalTable(ExportPackageModel package, IDbConnection connection)
        {
            if (package.ActionName == ExportActionTypes.Delete)
            {
                await DeleteFromLocalTable(package, connection);
            }
            else
            {
                await SaveToLocalTable(package, connection);
            }
        }

        /// <summary>
        /// Удаляет данные из локальной таблицы, после удаления в ГИС СЦОС
        /// </summary>
        /// <param name="package">Пакет c данными для удаления</param>
        /// <param name="connection">Соединение с БД</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task DeleteFromLocalTable(ExportPackageModel package, IDbConnection connection)
        {
            if (package?.ResponseData == "Success")
            {
                if (package.EntityId == 4) //StudyPlansDisciplines
                {
                    var study_plan = JObject.Parse(package.ExportData!)["study_plan"]?.ToString();
                    var discipline = JObject.Parse(package.ExportData!)["discipline"]?.ToString();
                    var semester = JObject.Parse(package.ExportData!)["semester"]?.ToString();

                    if (study_plan == null || discipline == null || semester == null)
                    {
                        throw new Exception($"Ошибка при удаление данных в локальной таблице, после удаления в ГИС СЦОС. " +
                            $"Отсутсвует один из идентификаторов необходимых для удаления данных. " +
                            $"(package_id - {package.Id}, study_plan - {study_plan}, discipline - {discipline}, semester - {semester})");
                    }
                    try
                    {
                        await connection.ExecuteAsync(
                            $"delete from {package.EntityTableName} " +
                            $"where study_plan = @study_plan and discipline = @discipline and semester = @semester",
                            new { study_plan, discipline, semester }
                        );
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Ошибка при удаление данных в локальной таблице, после удаления в ГИС СЦОС. " +
                            $"(package_id - {package.Id}, study_plan - {study_plan}, discipline - {discipline}, semester - {semester}). " +
                            $"Ошибка: {e.InnerException?.Message ?? e.Message}");
                    }
                }
                else if (package.EntityId == 6) //StudyPlansStudents
                {
                    var study_plan = JObject.Parse(package.ExportData!)["study_plan"]?.ToString();
                    var student = JObject.Parse(package.ExportData!)["student"]?.ToString();

                    if (study_plan == null || student == null)
                    {
                        throw new Exception($"Ошибка при удаление данных в локальной таблице, после удаления в ГИС СЦОС. " +
                            $"Отсутсвует один из идентификаторов необходимых для удаления данных. " +
                            $"(package_id - {package.Id}, study_plan - {study_plan}, student - {student})");
                    }
                    try
                    {
                        await connection.ExecuteAsync(
                            $"delete from {package.EntityTableName} " +
                            $"where study_plan = @study_plan and student = @student",
                            new { study_plan, student }
                        );
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Ошибка при удаление данных в локальной таблице, после удаления в ГИС СЦОС. " +
                            $"(package_id - {package.Id}, study_plan - {study_plan}, student - {student}). " +
                            $"Ошибка: {e.InnerException?.Message ?? e.Message}");
                    }
                }
                else
                {
                    try
                    {
                        await connection.ExecuteAsync(
                            $"delete from {package.EntityTableName} where scos_id = @scosId",
                            new { scosId = package.ExportData }
                        );
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Ошибка при удаление данных в локальной таблице, после удаления в ГИС СЦОС. " +
                            $"(package_id - {package.Id}, scos_id - {package.ExportData}. " +
                            $"Ошибка: {e.InnerException?.Message ?? e.Message}");
                    }
                }

                await connection.ExecuteAsync(
                    @"update ExportQueue
                        set save_export_data_to_local_table = 1
                        where id = @id",
                    new { id = package?.Id }
                );
            }
            else //При ошибке
            {
                await connection.ExecuteAsync(
                    $"insert into ExportErrors(entity_id, scos_id, export_queue_id, message) " +
                    $"values(@entityId, @scosId, @exportQueueId, @message)",
                    new
                    {
                        entityId = package?.EntityId,
                        scosId = package?.ExportData,
                        exportQueueId = package?.Id,
                        message = package?.ResponseData,
                    }
                );
            }
        }

        /// <summary>
        /// Сохранения данных в локальные таблицы, после сохранения в ГИС СЦОС
        /// </summary>
        /// <param name="package">Пакет с данными для сохранения</param>
        /// <param name="connection">Соединение в БД</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task SaveToLocalTable(ExportPackageModel package, IDbConnection connection)
        {
            if (string.IsNullOrEmpty(package?.ResponseData))
            {
                throw new Exception($"Ошибка при сохранении данных в локальную таблицу, после сохранения в ГИС СЦОС. " +
                    $"Отсутствует ответ из ГИС СЦОС. " +
                    $"(package - {package?.Id})"
                );
            }

            var resJson = JObject.Parse(package.ResponseData)["results"];
            if (resJson == null)
            {
                throw new Exception($"Ошибка при сохранении данных в локальную таблицу, после сохранения в ГИС СЦОС. " +
                    $"Некорректный формат ответа из ГИС СЦОС. " +
                    $"(package - {package?.Id}, responseData - {package?.ResponseData})"
                );
            }

            IEnumerable<ScosBaseResponseModel>? response = null;

            if (package.EntityId == 4) //StudyPlansDisciplines
            {
                response = JsonConvert.DeserializeObject<IEnumerable<StudyPlansDisciplinesResponseModel>>(resJson.ToString());
            }
            else if (package.EntityId == 6) //StudyPlansStudents
            {
                response = JsonConvert.DeserializeObject<IEnumerable<StudyPlansStudentsResponseModel>>(resJson.ToString());
            }
            else
            {
                response = JsonConvert.DeserializeObject<IEnumerable<ScosBaseResponseModel>>(resJson.ToString());
            }

            if (response == null)
            {
                throw new Exception($"Ошибка при сохранении данных в локальную таблицу, после сохранения в ГИС СЦОС. " +
                    $"Ошибка после десериализации ответа из ГИС СЦОС. " +
                    $"(package - {package?.Id}, resJson - {resJson})"
                );
            }

            var json = JObject.Parse(package.ExportData!)[package.JsonObjectName!] as JArray;
            foreach (var res in response)
            {
                string? exportJson = null;

                if (package?.EntityId == 4) //StudyPlansDisciplines
                {
                    exportJson = json?.SingleOrDefault(x =>
                        ((JObject)x).Value<string>("study_plan") == ((StudyPlansDisciplinesResponseModel)res).StudyPlan
                        && ((JObject)x).Value<string>("discipline") == ((StudyPlansDisciplinesResponseModel)res).Discipline
                        && ((JObject)x).Value<string>("semester") == ((StudyPlansDisciplinesResponseModel)res).Semester
                    )?.ToString();
                }
                else if (package?.EntityId == 6) //StudyPlansStudents
                {
                    exportJson = json?.SingleOrDefault(x =>
                        ((JObject)x).Value<string>("study_plan") == ((StudyPlansStudentsResponseModel)res).StudyPlan
                        && ((JObject)x).Value<string>("student") == ((StudyPlansStudentsResponseModel)res).Student
                    )?.ToString();
                }
                else
                {
                    exportJson = json?.SingleOrDefault(x => ((JObject)x).Value<string>("external_id") == res.ExternalId)?.ToString();
                }

                if (exportJson == null)
                {
                    throw new Exception($"Ошибка при сохранении данных в локальную таблицу, после сохранения в ГИС СЦОС. " +
                        $"Не найдены экспортируемые данные в пакете. " +
                        $"(package - {package?.Id}, externalId - {res.ExternalId})"
                    );
                }

                if (res.UploadStatusType == ScosUploadStatusTypes.Ok)
                {
                    try
                    {
                        await connection.ExecuteAsync(
                            $"exec SaveExportDataToLocalTable @tableName, @action, @json, @id",
                            new { tableName = package?.EntityTableName, action = package?.ActionName, json = exportJson, id = res.Id }
                        );
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Ошибка при сохранении выгруженных данных в локальную таблицу " +
                            $"(tableName - {package?.EntityTableName}, action - {package?.ActionName.ToString()}, json - {exportJson}, id - {res.Id}): " +
                            $"{e.InnerException?.Message ?? e.Message}");
                    }
                }
                else
                {
                    await connection.ExecuteAsync(
                        $"insert into ExportErrors(entity_id, external_id, export_queue_id, message, json) " +
                        $"values(@entityId, @externalId, @exportQueueId, @message, @json)",
                        new
                        {
                            entityId = package?.EntityId,
                            externalId = res.ExternalId,
                            exportQueueId = package?.Id,
                            message = res.AdditionalInfo,
                            json = exportJson
                        }
                    );
                }
            }

            await connection.ExecuteAsync(
                @"update ExportQueue
                set save_export_data_to_local_table = 1
                where id = @id",
                new { id = package?.Id }
            );
        }


        /// <summary>
        /// Возвращает список ошибок после выгрузки данных в ГИС СЦОС
        /// </summary>
        /// <param name="query">Аргументы запроса</param>
        /// <returns></returns>
        public async Task<PaginationResult<ExportError>> GetExportErrors(QueryModel query)
        {
            if (query == null)
            {
                throw new ExceptionWithCode("Неверные параметры запроса", "getExportErrors.queryShouldNotBeNull");
            }

            var res = new PaginationResult<ExportError>()
            {
                Info = new PaginateInfo()
                {
                    Page = query.Page,
                    PageSize = query.PageSize
                }
            };

            using (var connection = _context.CreateConnection())
            {
                DefaultTypeMap.MatchNamesWithUnderscores = true;

                res.Info!.RowCount = await (connection.QueryFirstOrDefaultAsync<int>(
                    @"select count(id) as row_count " +
                    "from GetExportErrors(0, 0, @search)",
                    new { search = query.Search }
                ).ConfigureAwait(false));

                res.Rows = await (connection.QueryAsync<ExportError>(
                    @"select id, entity_id, entity, action_name, external_id, scos_id, error_id, export_queue_id, message, json, create_date, resolved " +
                    "from GetExportErrors(@page , @pageSize, @search)",
                    new { page = query.Page, pageSize = query.PageSize, search = query.Search }
                ).ConfigureAwait(false));
            }

            return res;
        }

        /// <summary>
        /// Изменить поля ошибки экспорта
        /// </summary>
        /// <param name="error">Ошибка экспорта</param>
        /// <returns></returns>
        public async Task<ExportError> ChangeExportError(ExportError error)
        {
            if (error == null)
            {
                throw new ExceptionWithCode("Нет данных об ошибке экспорта", "getExportErrors.exportErrorShouldNotBeNull");
            }
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(
                    $"update ExportErrors " +
                    "set message = @message, json = @json, resolved = @resolved " +
                    "where id = @id",
                    new
                    {
                        id = error.Id,
                        message = error.Message,
                        json = error.Json,
                        resolved = error.Resolved
                    }
                );
            }

            return error;
        }

        /// <summary>
        /// Установить статус решения ошибки
        /// </summary>
        /// <param name="errorId">Код ошибки</param>
        /// <param name="resolved">Если true, то ошибка исправлена</param>
        /// <returns></returns>
        public async Task SetErrorResolutionStatus(string errorId, bool resolved)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(
                $"update err " +
                "set err.resolved = @resolved " +
                "from ExportErrors err " +
                "   inner join ExportQueue q on err.export_queue_id = q.id " +
                "where concat(err.external_id, err.scos_id, '_' + q.action_name) = @errorId",
                new { errorId, resolved }
            );
        }
    }
}
