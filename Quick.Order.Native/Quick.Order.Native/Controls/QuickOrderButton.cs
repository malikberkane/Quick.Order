using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.Controls
{
    public class QuickOrderButton : ContentView
    {


        public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(QuickOrderButton));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
      
    }
}
