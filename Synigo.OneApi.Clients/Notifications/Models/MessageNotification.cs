﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Synigo.OneApi.Clients.Notifications.Models
{
    public class MessageNotification
    {
        static readonly string NotificationResource = "ExternalNotifications";

        [JsonProperty("tenantId")]
        private string TenantId { get; set; }

        [JsonProperty("resource")]
        private string Resource { get; set; }

        [JsonProperty("resourceType")]
        private string ResourceType { get; set; }

        [JsonProperty("data")]
        private NotificationSource Data { get; set; }

        public MessageNotification(string tenantId, NotificationSource notificationSource)
        {
            TenantId = tenantId;
            Resource = NotificationResource;
            ResourceType = NotificationResource;
            Data = notificationSource;
        }

        public MessageNotification(string tenantId, Dictionary<string, string> multiTitle, Dictionary<string, string> multiDescription, string typeIdentifier, string imageUrl = null, string url = null, NotificationAction action = NotificationAction.Created)
        {
            TenantId = tenantId;
            Resource = NotificationResource;
            ResourceType = NotificationResource;
            Data = new NotificationSource(multiTitle, multiDescription, typeIdentifier, imageUrl, url, action);
        }

        public MessageNotification(string tenantId,string title, string description, string typeIdentifier, string imageUrl = null, string url = null, NotificationAction action = NotificationAction.Created)
        {
            TenantId = tenantId;
            Resource = NotificationResource;
            ResourceType = NotificationResource;
            Data = new NotificationSource(title, description, typeIdentifier, imageUrl, url, action);
        }
    }
}
