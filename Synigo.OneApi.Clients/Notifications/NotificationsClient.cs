﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Synigo.OneApi.Clients.Notifications.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Synigo.OneApi.Clients.Notifications
{
    public class NotificationsClient : INotificationsClient
    {
        private readonly IConfiguration Configuration;
        private readonly string _tenantId;
        private readonly string _synigoApiUrl;

        public NotificationsClient(IConfiguration configuration)
        {
            Configuration = configuration;
            _tenantId = Configuration.GetSection("AzureAd").GetValue<string>("TenantId");
            _synigoApiUrl = Configuration.GetSection("AzureAd").GetValue<string>("SynigoApiUrl");
        }

        /// <summary>
        /// Sends portal notification to all users in organization
        /// </summary>
        /// <param name="notification" cref="NotificationSource">received NotificationSource model</param>
        /// <returns cref="HttpResponseMessage">Returns HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> SendNotification(NotificationSource notification)
        {
            var client = new SynigoApiClient(Configuration);
            var messageNotification = new MessageNotification(_tenantId, notification);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_synigoApiUrl}/messagenotifications/{_tenantId}/messageNotification");
            request.Content = new StringContent(JsonConvert.SerializeObject(messageNotification));
            return await client.SendAsync(request);
        }

        /// <summary>
        /// Sends push notification to mobile application
        /// </summary>
        /// <param name="pushNotification" cref="PushNotification">Push notification model</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> SendPushNotification(PushNotification pushNotification)
        {
            pushNotification.TenantId = _tenantId;
            var client = new SynigoApiClient(Configuration);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_synigoApiUrl}/pushnotifications/{_tenantId}/message");
            request.Content = new StringContent(JsonConvert.SerializeObject(pushNotification));
            return await client.SendAsync(request);
        }
    }
}
