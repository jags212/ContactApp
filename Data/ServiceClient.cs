using ContactApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ContactApp.Data
{
    public class ServiceClient
    {
        private readonly ILogger<ServiceClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        string host = string.Empty;
        public ServiceClient(ILogger<ServiceClient> logger, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            host = "http://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/";
        }
        public async Task<TResponse> GetDataAsync<TResponse>(string relativePath, Dictionary<string, string> headers)
        {
            TResponse response = default(TResponse);
            using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, host + relativePath))
            {
                LoadAppHeader(httpRequestMessage, headers);
                HttpResponseMessage result = await _httpClient.SendAsync(httpRequestMessage);
                try
                {
                    result.EnsureSuccessStatusCode();
                    response = JsonConvert.DeserializeObject<TResponse>(await result.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw ex;
                }
            }
            return response;
        }

        public async Task PostDataAsync<TReq>(string relativePath, Dictionary<string, string> headers, TReq request)
        {
            using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, host + relativePath))
            {
                LoadAppHeader(httpRequestMessage, headers);
                httpRequestMessage.Content = JsonContent.Create(request);
                {
                    HttpResponseMessage result = await _httpClient.SendAsync(httpRequestMessage);
                    try
                    {
                        result.EnsureSuccessStatusCode();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
        }

        public async Task PutDataAsync<TReq>(string relativePath, Dictionary<string, string> headers, TReq request)
        {
            using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, host + relativePath))
            {
                LoadAppHeader(httpRequestMessage, headers);
                httpRequestMessage.Content = JsonContent.Create(request);
                {
                    HttpResponseMessage result = await _httpClient.SendAsync(httpRequestMessage);
                    try
                    {
                        result.EnsureSuccessStatusCode();

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);

                    }
                }
            }
        }

        public async Task DeleteDataAsync<TReq>(string relativePath, Dictionary<string, string> headers)
        {
            using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, host + relativePath))
            {
                LoadAppHeader(httpRequestMessage, headers);
                HttpResponseMessage result = await _httpClient.SendAsync(httpRequestMessage);
                try
                {
                    result.EnsureSuccessStatusCode();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            }
        }

        private void LoadAppHeader(HttpRequestMessage httpRequestMessage, Dictionary<string, string> headers)
        {
            try
            {
                if (headers != null && headers.Any())
                {
                    foreach (var item in headers)
                    {
                        httpRequestMessage.Headers.TryAddWithoutValidation(item.Key, item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
