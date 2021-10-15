using Quick.Order.AppCore.Contracts;
using System;
using Xamarin.Essentials;

namespace Quick.Order.Shared.Infrastructure
{
    public class VibrationService : IVibrationService
    {
        public void Vibrate(int seconds=1)
        {

            var duration = TimeSpan.FromSeconds(seconds);
            Vibration.Vibrate(duration);
        }


      
    }
  
}
