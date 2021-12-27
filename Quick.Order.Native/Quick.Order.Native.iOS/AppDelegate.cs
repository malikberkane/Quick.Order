using System;
using System.Collections.Generic;
using System.Linq;
using Quick.Order.AppCore.Contracts;

using Foundation;
using UIKit;
using FreshMvvm;
using Plugin.GoogleClient;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Quick.Order.Native.iOS;
using CoreGraphics;
using System.IO;

[assembly: ExportRenderer(typeof(Editor), typeof(MyEditorRenderer))]
namespace Quick.Order.Native.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();

            

            GoogleVisionBarCodeScanner.iOS.Initializer.Init();
            // Temporary work around for bug on Firebase Library
            // https://github.com/xamarin/GoogleApisForiOSComponents/issues/368
            Firebase.Core.App.Configure();
            // This line is not needed in version 5.0.5
            FreshIOC.Container.Register<IPrintService, IOSPrintService>();
            FreshIOC.Container.Register<IDeepLinkService, IOSDeepLinkService>();
            FreshIOC.Container.Register<ILoggerService, IOSLoggerService>();
            global::Xamarin.Forms.Forms.Init();
            GoogleClientManager.Initialize();
            UINavigationBar.Appearance.BarTintColor = UIColor.White;

            LoadApplication(new App());

            Firebase.Crashlytics.Crashlytics.SharedInstance.SetCrashlyticsCollectionEnabled(true);

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return GoogleClientManager.OnOpenUrl(app, url, options);
        }
    }


    public class IOSDeepLinkService : IDeepLinkService
    {
        public string CreateDeepLinkUrl(string restaurantId)
        {
            return $"https://malikberkane.page.link/?link=http://quickorder/?id={restaurantId}/&apn=com.malikberkane.quickorder";
        }


        public string ExtractRestaurantIdFromUri(Uri uri)
        {
            return uri.Query.Replace("?link=http://quickorder/?id=", "").Replace("/&apn=com.malikberkane.quickorder", "");
        }
    }

    public class IOSLoggerService : ILoggerService
    {
        public void Log(System.Exception ex)
        {
            try
            {

                var crashInfo = new Dictionary<object, object>
                {
                    [NSError.LocalizedDescriptionKey] = ex.Message,
                    ["StackTrace"] = ex.StackTrace
                };

                var error = new NSError(new NSString(ex.GetType().FullName),
                                        -1,
                                        NSDictionary.FromObjectsAndKeys(crashInfo.Values.ToArray(), crashInfo.Keys.ToArray(), crashInfo.Count));


                Firebase.Crashlytics.Crashlytics.SharedInstance.RecordError(error);
            }
            catch (Exception)
            {
                Debug.WriteLine(ex);
            }

        }

        public void SetUserId(string userId)
        {
            Firebase.Crashlytics.Crashlytics.SharedInstance.SetUserId(userId);
        }


    }



    public class IOSPrintService : IPrintService
    {
        [Obsolete]
        void IPrintService.Print(byte[] content)
        {
            var data = NSData.FromStream(new MemoryStream(content));
            var uiimage = UIImage.LoadFromData(data);

            var printer = UIPrintInteractionController.SharedPrintController;

            if (printer == null)
            {
                Console.WriteLine("Unable to print at this time.");
            }
            else
            {

                var printInfo = UIPrintInfo.PrintInfo;
                printInfo.OutputType = UIPrintInfoOutputType.General;
                printInfo.JobName = "QR Code printing";
                printer.PrintInfo = printInfo;
                printer.PrintingItem = uiimage;
                printer.ShowsPageRange = true;

                var handler = new UIPrintInteractionCompletionHandler((printInteractionController, completed, error) =>
                {
                    if (completed)
                    {
                        Console.WriteLine("Print Completed.");
                    }
                    else if (!completed && error != null)
                    {
                        Console.WriteLine("Error Printing.");
                    }

                });

                CGRect frame = new CGRect();
                frame.Size = uiimage.Size;

                printer.Present(true, handler);
            }

        }
    }


    public class MyEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Editor> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Layer.CornerRadius = 4;
                Control.Layer.BorderColor = Color.LightGray.ToCGColor();
                Control.Layer.BorderWidth = 1;
            }
        }
    }
}