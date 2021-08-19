using Quick.Order.AppCore.Contracts;
using System;
using Xamarin.Essentials;

namespace Quick.Order.Shared.Infrastructure
{
    public class VibrationService : IVibrationService
    {
        public void Vibrate()
        {

            var duration = TimeSpan.FromSeconds(2);
            Vibration.Vibrate(duration);
        }
    }

  
}
