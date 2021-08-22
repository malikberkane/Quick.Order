using System;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
namespace MalikBerkane.MvvmToolkit
{
    public class AlertUserService : IAlertUserService
    {
        public async void ShowActionSnack(string message, string actionLabel, Action action, Action dismissedAction = null, AlertType state = AlertType.Info)
        {

            var actions = new SnackBarActionOptions
            {
                Action = async () => action.Invoke(),
                Text = actionLabel,
                ForegroundColor = Color.White
            };

            var options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,

                    Message = message
                },
                BackgroundColor = SnackColorFromAlertType(state),
                Duration = SnackDurationFromAlertType(state),
                Actions = new[] { actions }
            };

            if (!await Application.Current.MainPage.DisplaySnackBarAsync(options) && dismissedAction != null)
            {
                dismissedAction.Invoke();
            }

        }

        public void ShowSnack(string message, AlertType state = AlertType.Info)
        {
            var options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = message
                },
                BackgroundColor = SnackColorFromAlertType(state),
                Duration = SnackDurationFromAlertType(state),
            };

            Application.Current.MainPage.DisplaySnackBarAsync(options);
        }



        public void WrongInput(string warning)
        {
            Application.Current.MainPage.DisplayAlert(string.Empty, warning, "Ok");
        }


        private Color SnackColorFromAlertType(AlertType alertType)
        {
            switch (alertType)
            {
                case AlertType.Success:
                    return Color.DarkGreen;
                case AlertType.Error:
                    return Color.DarkRed;
                default:
                    return Color.FromHex("#505050");
            }
        }

        private TimeSpan SnackDurationFromAlertType(AlertType alertType)
        {
            switch (alertType)
            {
                case AlertType.Success:
                    return TimeSpan.FromSeconds(2);
                case AlertType.Error:
                    return TimeSpan.FromSeconds(4);
                default:
                    return TimeSpan.FromSeconds(4);
            }
        }
    }

}
