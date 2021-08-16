using Quick.Order.Native.Droid;
using Quick.Order.Native.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("malikberkane")]
[assembly: ExportEffect(typeof(AndroidNoEntryUnderliningEffect), nameof(NoEntryUnderliningEffect))]

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
}