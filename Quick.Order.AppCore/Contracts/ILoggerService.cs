using System;

namespace Quick.Order.AppCore.Contracts
{
    public interface ILoggerService
    {
        void Log(Exception ex);
    }
}
