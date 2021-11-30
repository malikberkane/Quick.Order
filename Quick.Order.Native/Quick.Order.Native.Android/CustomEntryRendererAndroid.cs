using Quick.Order.Native.Droid;
using Quick.Order.Native.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("malikberkane")]
[assembly: ExportEffect(typeof(AndroidNoEntryUnderliningEffect), nameof(NoEntryUnderliningEffect))]
[assembly: ExportEffect(typeof(AndroidSelectAllOnFocusEffect), nameof(SelectAllOnFocusEffect))]

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

}