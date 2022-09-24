using Common.Interfaces.Services;
using CurlThin;
using CurlThin.Enums;

namespace Common.Services.CurlHTTPClientService
{
    /// <summary>
    /// Сервис для отправки запросов
    /// </summary>
    public class CurlHTTPClientService : IHTTPClientService
    {
        private readonly CURLcode curlInitCode;

        public CurlHTTPClientService()
        {
            curlInitCode = CurlNative.Init();
        }

        ~CurlHTTPClientService()
        {
            if (curlInitCode == CURLcode.OK)
            {
                CurlNative.Cleanup();
            }
        }

        public Task<string> Get(string url, Dictionary<string, string>? headers)
        {
            using var curl = CurlNative.Easy.Init();

            return curl
                .SetMethod(HTTPMethod.GET)
                .SetUrl(url)
                .SetHeaders(headers)
                .DisableCertVerify()
                .Send();
        }

        public Task<string> Post(string url, string? json, Dictionary<string, string>? headers)
        {
            using var curl = CurlNative.Easy.Init();

            return curl
                .SetMethod(HTTPMethod.POST)
                .SetUrl(url)
                .SetHeaders(headers, true)
                .SetBody(json)
                .DisableCertVerify()
                .Send();
        }

        public Task<string> Put(string url, string? json, Dictionary<string, string>? headers)
        {
            using var curl = CurlNative.Easy.Init();

            return curl
                .SetMethod(HTTPMethod.PUT)
                .SetUrl(url)
                .SetHeaders(headers, true)
                .SetBody(json)
                .DisableCertVerify()
                .Send();
        }

        public Task<string> Delete(string url, Dictionary<string, string>? headers)
        {
            using var curl = CurlNative.Easy.Init();

            return curl
                .SetMethod(HTTPMethod.DELETE)
                .SetUrl(url)
                .SetHeaders(headers)
                .DisableCertVerify()
                .Send();
        }
    }
}