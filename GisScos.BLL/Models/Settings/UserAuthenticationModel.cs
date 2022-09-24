namespace GisScos.BLL.Models.Settings
{
    /// <summary>
    /// Модель аутентификации пользователя
    /// </summary>
    public class UserAuthenticationModel
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string? Role { get; set; }
    }
}
