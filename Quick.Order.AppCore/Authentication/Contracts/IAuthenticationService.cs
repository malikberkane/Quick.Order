using Quick.Order.AppCore.Models;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Authentication.Contracts
{
    public interface IAuthenticationService
    {
        Task<AutenticatedRestaurantAdmin> CreateUser(string username, string email, string password);
        Task<AutenticatedRestaurantAdmin> SignIn(string email, string password);
        Task<AutenticatedRestaurantAdmin> SignInWithOAuth();
        Task SignOut();

        AutenticatedRestaurantAdmin LoggedUser { get; }
    }
}
