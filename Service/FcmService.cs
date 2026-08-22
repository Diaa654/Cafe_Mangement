using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FcmService(ILogger<FcmService> _logger) : IFcmService
    {
        public async Task<string> SendToTopicAsync(string topic, string title, string body)
        {
            var message = new Message()
            {
                Topic = topic,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            try
            {
                await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return "Success";
            }
            catch (FirebaseMessagingException ex) when (ex.MessagingErrorCode == MessagingErrorCode.Unregistered)
            {
                return "Unregistered";
            }
            catch (Exception)
            {
                return "Failed";
            }
        }
        
        
        public async Task SubscribeWaiterToTopicAsync(string deviceToken)
        {
            await SubscribeUserToTopicAsync(deviceToken, "Waiters", "الويتر");
        }

        public async Task SubscribeBaristaToTopicAsync(string deviceToken)
        {
            await SubscribeUserToTopicAsync(deviceToken, "Barista", "الباريستا");
        }
        private async Task SubscribeUserToTopicAsync(string deviceToken, string topicName, string roleNameInArabic)
        {
            if (string.IsNullOrWhiteSpace(deviceToken)) return;

            var registrationTokens = new List<string> { deviceToken };
            try
            {
                var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(registrationTokens, topicName);

                if (response.FailureCount > 0)
                {
                    var error = response.Errors.FirstOrDefault();
                    _logger.LogWarning($"فشل اشتراك {roleNameInArabic}: {error?.Reason} | Index: {error?.Index}");
                }
                else
                {
                    _logger.LogInformation($"تم اشتراك جهاز {roleNameInArabic} بنجاح في مجموعة {topicName}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"إيرور مفاجئ في اشتراك {roleNameInArabic}: {ex.Message}");
            }
        }

    }
}
