using GisScos.BLL.Models.Settings;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace GisScos.BLL.Context
{
    /// <summary>
    /// Класс контекста Dapper
    /// </summary>
    public class DapperContext
    {
        private readonly AppSettings _settings;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="settings">настройки приложения</param>
        public DapperContext(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        /// <summary>
        /// Создает SqlConnection
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection() => new SqlConnection(_settings?.ConnectionStrings?.MainConnection);
    }
}
