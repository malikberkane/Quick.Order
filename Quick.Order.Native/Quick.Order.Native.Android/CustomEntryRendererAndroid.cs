using Android.Content;
using Quick.Order.Native.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRendererAndroid))]

namespace Quick.Order.Native.Droid
{

    public class CustomEntryRendererAndroid : EntryRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {

                this.Control.SetBackground(null);

                var inputLayout = Control as Android.Widget.EditText;

                if (inputLayout == null)
                    return;

                inputLayout.SetSelectAllOnFocus(true);

            }
        }

        public CustomEntryRendererAndroid(Context context) : base(context)
        {

        }
    }
}