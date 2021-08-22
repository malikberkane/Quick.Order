using System;

namespace Quick.Order.AppCore.Contracts
{
    public delegate void ConnectivityChangedEventHandler(object sender, NetworkAccessStateChanged args);

    public interface IConnectivityService
    {
        bool HasNetwork();

        event ConnectivityChangedEventHandler ConnectivityStateChanged;

    }


    public class NetworkAccessStateChanged: EventArgs
    {
       public bool NetworkRestored { get; set; }
    }


}
