using System;
using Xamarin.Forms;

namespace Quick.Order.Native.Services
{
    public class PageModelMessagingService
    {


        public void Send(string messageId)
        {
            MessagingCenter.Send(this, messageId);

        }

        public void Send<T>(string messageId, T param = null) where T : class
        {
            MessagingCenter.Send(this, messageId, param);

        }

        public void Subscribe(string message, Action action, object subscriber)
        {
            Action<PageModelMessagingService> formsAction = delegate (PageModelMessagingService service) { action(); };
            MessagingCenter.Subscribe(subscriber, message, formsAction);
        }

        public void Subscribe<T>(string message, Action<T> action, object subscriber) where T : class
        {
            MessagingCenter.Subscribe<PageModelMessagingService, T>(subscriber, message, (s, t) => { action(t); });
        }

        public void Unsubscribe<T>(string message, object subscriber) where T : class
        {
            MessagingCenter.Unsubscribe<PageModelMessagingService, T>(subscriber, message);
        }




    }
}