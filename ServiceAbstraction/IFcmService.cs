using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IFcmService
    {
        Task SubscribeWaiterToTopicAsync(string deviceToken);
        Task SubscribeBaristaToTopicAsync(string deviceToken);
        Task<string> SendToTopicAsync(string topic, string title, string body);

    }
}
