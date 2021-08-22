using Quick.Order.AppCore.Contracts;
using System;
using Xamarin.Essentials;
namespace Quick.Order.Shared.Infrastructure
{
    public class ConnectivityService : IConnectivityService
    {
        public event ConnectivityChangedEventHandler ConnectivityStateChanged;

        public ConnectivityService()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

        }

        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if(e.NetworkAccess== NetworkAccess.Internet)
            {
                ConnectivityStateChanged.Invoke(this, new NetworkAccessStateChanged { NetworkRestored=true });

            }
            else
            {
                ConnectivityStateChanged.Invoke(this, new NetworkAccessStateChanged { NetworkRestored = false });

            }
        }

        public bool HasNetwork()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
