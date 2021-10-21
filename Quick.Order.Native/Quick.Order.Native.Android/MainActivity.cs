using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.GoogleClient;
using Quick.Order.AppCore.Contracts;
using Android.PrintServices;
using FreshMvvm;
using Plugin.CurrentActivity;
using Xamarin.Forms.Platform.Android.AppLinks;
using Firebase;
using Firebase.Crashlytics;

namespace Quick.Order.Native.Droid
{
    [Activity(Label = "Quick.Order.Native", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]



    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                  DataScheme = "http",
                  DataHost = "malikberkane.page.link",
                  DataPathPrefix = "/",
                  AutoVerify = true,
                  Categories = new[] { Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]

    [IntentFilter(new[] { Android.Content.Intent.ActionView },
                  DataScheme = "https",
                  DataHost = "malikberkane.page.link",
                  DataPathPrefix = "/",
                  AutoVerify = true,
                  Categories = new[] { Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            GoogleClientManager.Initialize(this);

            GoogleVisionBarCodeScanner.Droid.RendererInitializer.Init();

            Rg.Plugins.Popup.Popup.Init(this);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            FirebaseApp.InitializeApp(this);
            AndroidAppLinks.Init(this);
            FreshIOC.Container.Register<IPrintService, PrintService>();
            FreshIOC.Container.Register<IDeepLinkService, DeepLinkService>();
            FreshIOC.Container.Register<ILoggerService, LoggerService>();

            FirebaseCrashlytics.Instance.SetCrashlyticsCollectionEnabled(true);
            LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            GoogleClientManager.OnAuthCompleted(requestCode, resultCode, data);
        }
    }
}