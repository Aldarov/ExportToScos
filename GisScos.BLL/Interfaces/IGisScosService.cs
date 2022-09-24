/// <summary>
/// Интерфейс для получения данных из ГИС СЦОС
/// </summary>
public interface IGisScosService
{
    /// <summary>
    /// Проверка подключения к ГИС СЦОС
    /// </summary>
    /// <returns></returns>
    Task<string> CheckConnection();
}