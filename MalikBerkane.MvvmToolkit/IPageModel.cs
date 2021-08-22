
using System;
using System.Threading.Tasks;


namespace MalikBerkane.MvvmToolkit
{
    public interface IPageModel
    {
        void Init(object initData);
        Task CleanUp();
        void OnAppearing(object sender, EventArgs e);

        Task EnsurePageModelIsInLoadingState(Func<Task> action, bool delay = false);
        Task EnsurePageModelIsInLoadingState<T>(Func<T, Task> action, T param, bool delay = false) where T : class;
    }
}
