﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Share.Models.Task;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TasksGenerator.Infrastructure;
using TasksGenerator.Infrastructure.Providers;

namespace TasksGenerator.HttpProvider.Providers
{
    /// <summary>
    /// Sending requests to external API by Http
    /// </summary>
    public sealed class HttpTransportProvider : ITransportProvider<HttpStatusCode>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AppSettings _appSettings;

        public HttpTransportProvider(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings)
        {
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<HttpStatusCode> SendRequestExternalApiAsync(IMessageExternalApi messageBody, string endPointUrl)
        {
            Uri path = new Uri($"{endPointUrl}{_appSettings.ExternalApiAction}");

            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Add(_appSettings.CustomHeader.Name, _appSettings.CustomHeader.Value);

                var httpContent = new StringContent(JsonConvert.SerializeObject(messageBody), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(path, httpContent);
                return response.StatusCode;
            }
        }
    }
}