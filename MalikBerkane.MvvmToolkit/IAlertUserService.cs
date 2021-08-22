
using System;


namespace MalikBerkane.MvvmToolkit
{
    public interface IAlertUserService
    {
        void WrongInput(string warning);
        void ShowActionSnack(string message, string actionLabel, Action action, Action dismissedAction = null, AlertType state = AlertType.Info);
        void ShowSnack(string message, AlertType state = AlertType.Info);
    }
}
