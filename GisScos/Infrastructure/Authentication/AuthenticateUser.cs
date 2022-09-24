using GisScos.BLL.Models.Settings;

namespace GisScos.Infrastructure.Authentication
{
    /// <summary>
    /// Класс для работы с аутентификацией
    /// </summary>
    public static class Authentication
    {
        /// <summary>
        /// Возвращает аутентифицированного пользователя
        /// </summary>
        /// <param name="users">Массив зарегистрированных пользователей</param>
        /// <param name="userName">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>Пользователь или null</returns>
        public static UserAuthenticationModel? GetAuthenticateUser(
            this IEnumerable<UserAuthenticationModel> users,
            string userName, string password)
        {
            UserAuthenticationModel? user = null;
            if (users != null && users.Any())
            {
                user = users.SingleOrDefault(x => x.User == userName && x.Password == password);
            }

            return user;
        }
    }
}