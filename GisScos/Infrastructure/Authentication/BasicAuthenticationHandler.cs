using GisScos.BLL.Models.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace GisScos.Infrastructure.Authentication
{
    /// <summary>
    /// Basic аутентификация
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AppSettings _setting;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="setting">настройки приложения</param>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<AppSettings> setting
        ) : base(options, logger, encoder, clock)
        {
            _setting = setting.Value;
        }

        /// <summary>
        /// Переопределение HandleAuthenticateAsync
        /// </summary>
        /// <returns></returns>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");

            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Отсутствует заголовок авторизации"));

            if (_setting?.Users == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("В системе отсутствует информация о пользователях"));
            }

            UserAuthenticationModel? user = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (authHeader.Parameter != null)
                {
                    var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    var username = credentials[0];
                    var password = credentials[1];
                    user = _setting.Users.GetAuthenticateUser(username, password);
                }
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Неверный заголовок авторизации"));
            }

            if (user == null)
                return Task.FromResult(AuthenticateResult.Fail("Неверный логин или пароль"));

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user?.User ?? ""),
                new Claim(ClaimTypes.Name, user?.User ?? ""),
                new Claim(ClaimTypes.Role, user?.Role ?? "")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
