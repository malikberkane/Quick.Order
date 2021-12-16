using Android.Content;
using Android.Text.Method;
using Quick.Order.Native.Droid;
using Quick.Order.Native.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("malikberkane")]
[assembly: ExportEffect(typeof(AndroidNoEntryUnderliningEffect), nameof(NoEntryUnderliningEffect))]
[assembly: ExportEffect(typeof(AndroidSelectAllOnFocusEffect), nameof(SelectAllOnFocusEffect))]
[assembly: ExportRenderer(typeof(Entry), typeof(EntryNumericKeyboardRenderer))]

namespace Quick.Order.Native.Droid
{
    public class AndroidNoEntryUnderliningEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            if (Control != null)
            {

                Control.SetBackground(null);

                var inputLayout = Control as Android.Widget.EditText;

                if (inputLayout == null)
                    return;

                inputLayout.SetSelectAllOnFocus(true);

            }
        }

        protected override void OnDetached()
        {

        }
    }



    public class AndroidSelectAllOnFocusEffect : Xamarin.Forms.Platform.Android.PlatformEffect
    {
        protected override void OnAttached()
        {
            var inputLayout = Control as Android.Widget.EditText;

            if (inputLayout == null)
                return;

            inputLayout.SetSelectAllOnFocus(true);
        }

        protected override void OnDetached()
        {

        }
    }


    public class EntryNumericKeyboardRenderer : EntryRenderer
    {
        public EntryNumericKeyboardRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                if(e.NewElement.Keyboard== Keyboard.Numeric)
                {
                    this.Control.KeyListener = DigitsKeyListener.GetInstance("1234567890,.");

                }
            }
        }
    }


}