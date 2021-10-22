using Xamarin.Forms;

namespace MalikBerkane.MvvmToolkit
{
    public class ExtendedNavigationPage : NavigationPage
    {
        public ExtendedNavigationPage(Page root) : base(root)
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Popped += this.OnPopped;
        }


        private void OnPopped(object sender, NavigationEventArgs e)
        {
            if (e.Page.BindingContext is IPageModel pageModel)
            {
                try
                {
                    pageModel.CleanUp();
                    e.Page.Appearing -= pageModel.OnAppearing;
                    e.Page.Appearing -= pageModel.OnDisappearing;
                    e.Page.BindingContext = null;
                }
                catch (System.Exception)
                {

                    //Exception sur une page qui n'existe plus.
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Popped -= this.OnPopped;
        }

    }


}
