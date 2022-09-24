
using CurlThin;
using CurlThin.Enums;
using CurlThin.SafeHandles;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Services.CurlHTTPClientService
{
    /// <summary>
    /// Класс методов расширения для HTTPClientService
    /// </summary>
    public static class CurlHTTPClientExtentions
    {
        /// <summary>
        /// Устанавливаем Url запроса
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        /// <param name="url">Url запроса</param>
        public static SafeEasyHandle SetUrl(this SafeEasyHandle handle, string url)
        {
            CurlNative.Easy.SetOpt(handle, CURLoption.URL, url);
            return handle;
        }

        /// <summary>
        /// Устанавливает метод отправки запроса
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        /// <param name="method">Метод отправки запроса</param>
        public static SafeEasyHandle SetMethod(this SafeEasyHandle handle, HTTPMethod method)
        {
            CurlNative.Easy.SetOpt(handle, CURLoption.CUSTOMREQUEST, method.ToString());
            return handle;
        }

        /// <summary>
        /// Отключить проверку сертификата
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        public static SafeEasyHandle DisableCertVerify(this SafeEasyHandle handle)
        {
            CurlNative.Easy.SetOpt(handle, CURLoption.SSL_VERIFYPEER, 0);
            return handle;
        }

        /// <summary>
        /// Устанавливает сертификат центра сертификации (CA)
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        /// <param name="pathToCert">Путь до сертификата CA в формате pem-файла</param>
        /// <returns></returns>
        public static SafeEasyHandle SetCACert(this SafeEasyHandle handle, string pathToCert)
        {
            CurlNative.Easy.SetOpt(handle, CURLoption.CAINFO, pathToCert);
            return handle;
        }

        /// <summary>
        /// Устанавливает заголовки в запросе
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        /// <param name="headers">Заголовки</param>
        /// <param name="addJsonContentType">Если true, то добавляем "Content-Type: application/json"</param>
        public static SafeEasyHandle SetHeaders(this SafeEasyHandle handle, Dictionary<string, string>? headers, bool addJsonContentType = false)
        {
            if (headers != null && headers.Any())
            {
                var headerHandle = SafeSlistHandle.Null;
                if (addJsonContentType)
                    headerHandle = CurlNative.Slist.Append(headerHandle, $"Content-Type: application/json");
                foreach (var header in headers)
                {
                    headerHandle = CurlNative.Slist.Append(headerHandle, $"{header.Key}: {header.Value}");
                }
                CurlNative.Easy.SetOpt(handle, CURLoption.HTTPHEADER, headerHandle.DangerousGetHandle());
            }
            return handle;
        }

        /// <summary>
        /// Устанавливает тело запроса
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        /// <param name="body">Тело запроса, в формате строки UTF8</param>
        public static SafeEasyHandle SetBody(this SafeEasyHandle handle, string? body)
        {
            if (body != null)
            {
                var asciiBody = Regex.Replace(body, @"[^\x00-\x7F]", m => String.Format("\\u{0:X4}", (int)m.Value[0]));
                CurlNative.Easy.SetOpt(handle, CURLoption.POSTFIELDSIZE, Encoding.ASCII.GetByteCount(asciiBody));
                CurlNative.Easy.SetOpt(handle, CURLoption.COPYPOSTFIELDS, asciiBody);
            }

            return handle;
        }

        /// <summary>
        /// Получает ответ от сервера
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        /// <param name="callback">callback-функция, получает ответ в виде строки</param>
        private static SafeEasyHandle GetResponse(this SafeEasyHandle handle, Action<string> callback)
        {
            using var stream = new MemoryStream();

            CurlNative.Easy.SetOpt(handle, CURLoption.WRITEFUNCTION, (data, size, nmemb, user) =>
            {
                var length = (int)size * (int)nmemb;
                var buffer = new byte[length];
                Marshal.Copy(data, buffer, 0, length);
                stream.Write(buffer, 0, length);
                return (UIntPtr)length;
            });

            var resultCode = CurlNative.Easy.Perform(handle);

            var response = Regex.Unescape(Encoding.UTF8.GetString(stream.ToArray()));
            stream.Close();

            if (resultCode != CURLcode.OK)
            {
                var pErrorMsg = CurlNative.Easy.StrError(resultCode);
                var errorMsg = Marshal.PtrToStringAnsi(pErrorMsg);
                throw new Exception(errorMsg);
            }

            callback?.Invoke(response);

            return handle;
        }

        /// <summary>
        /// Отправляет запрос
        /// </summary>
        /// <param name="handle">Handle запроса</param>
        /// <exception cref="Exception"></exception>
        public static Task<string> Send(this SafeEasyHandle handle)
        {
            var complete = new TaskCompletionSource<string>();

            handle.GetResponse(x => complete.TrySetResult(x));

            CurlNative.Easy.GetInfo(handle, CURLINFO.RESPONSE_CODE, out int httpCode);
            if (httpCode >= 400)
            {
                throw new Exception(complete.Task.Result);
            }

            return complete.Task;
        }
    }
}
