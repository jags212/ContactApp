using ContactApp.Data.Models;
using ContactApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.Data
{
    public static class ContactConstant
    {
        public const string GetContact = "api/Contacts";
        public const string GetContactById = "api/Contacts/{0}";
    }

    public static class ContactGroupConstant
    {
        public const string GetContact = "api/ContactGroups";
        public const string GetContactById = "api/ContactGroups/{0}";
    }

    public class TokenClient
    {
        public const string TokenUrl = "account/createtoken";

        private readonly ILogger<TokenClient> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        string host = string.Empty;
        public TokenClient(ILogger<TokenClient> logger, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<Dictionary<string , string>> GetAuthHeaderAsync()
        {
            var accessToken = "";
            try
            {
                TokenModel tokenParams = await PostAuthTokenAsync<TokenModel>();
                if (tokenParams != null && !string.IsNullOrEmpty(tokenParams.token))
                {
                    accessToken = tokenParams.token;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
            return string.IsNullOrEmpty(accessToken) ? null : new Dictionary<string, string> { ["Authorization"] = $"Bearer {accessToken}" };
        }

        public async Task<TResponse> PostAuthTokenAsync<TResponse>()
        {
            TResponse response = default(TResponse);
            host = "http://"+_httpContextAccessor.HttpContext.Request.Host.Value+"/";
            string baseUri = host+TokenUrl;
            var postContent = new LoginViewModel { Username = "contactdemo@test.com", Password = "P@ssw0rd!" };
            using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post , baseUri))
            {
                httpRequestMessage.Content = JsonContent.Create(postContent);
                //httpRequestMessage.Content = new StringContent("{\"Username\":\"contactdemo@test.com\",\"Password\":P@ssw0rd!}", Encoding.UTF8, "application/json");
                {
                    HttpResponseMessage result = await _httpClient.SendAsync(httpRequestMessage);
                    try
                    {
                        result.EnsureSuccessStatusCode();
                        var obj = await result.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<TResponse>(obj);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        throw ex;
                    }
                }
            }

            return response;
        }
    }
}
