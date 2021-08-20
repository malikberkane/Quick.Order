using Java.Lang;
using Quick.Order.AppCore.Contracts;
using System;

namespace Quick.Order.Native.Droid
{
    public class LoggerService : ILoggerService
    {
        public void Log(System.Exception ex)
        {
            Firebase.Crashlytics.FirebaseCrashlytics.Instance.RecordException(Throwable.FromException(ex));
        }
    }

}