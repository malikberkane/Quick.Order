using System;
using Firebase.Auth;
using System.Threading.Tasks;
using Plugin.GoogleClient;
using Quick.Order.AppCore.Models;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.Shared.Infrastructure.Exceptions;
using Quick.Order.AppCore.Authentication.Exceptions;

namespace Quick.Order.Shared.Infrastructure.Authentication
{
    public class FirebaseAuthenticationService : IAuthenticationService
    {
        public const string WebAPIkey = "AIzaSyBhWjAJpwbA21UwB6I4bFvnP9tvMcFbaTs";


        public AutenticatedRestaurantAdmin LoggedUser { get; private set; }
        public async Task<AutenticatedRestaurantAdmin> CreateUser(string username, string email, string password)
        {

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password, username);

            if (auth == null)
            {
                throw new UserCreationNullException();
            }

            var loggedUser = new AutenticatedRestaurantAdmin
            {
                RestaurantAdmin = new RestaurantAdmin() { Email = auth.User.Email, Name = auth.User.DisplayName },
                AuthenticationToken = auth.FirebaseToken,
                AuthenticationExpired = auth.IsExpired()
            };

            LoggedUser = loggedUser;

            return loggedUser;


        }


        public async Task<bool> IsSignedIn()
        {
            return false;
            //return await _cacheService.GetItem<RestaurantAdmin>("CurrentUser", false, CacheType.Local) != null;
        }

        public Task ResetPassword(string email)
        {
            return Task.CompletedTask;
        }

        public async Task<AutenticatedRestaurantAdmin> SignIn(string email, string password)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));

                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
                if (auth == null)
                {
                    throw new SignInResultNullException();
                }

                var loggedUser = new AutenticatedRestaurantAdmin
                {
                    RestaurantAdmin = new RestaurantAdmin() { Email = auth.User.Email, Name = auth.User.DisplayName },
                    AuthenticationToken = auth.FirebaseToken,
                    AuthenticationExpired = auth.IsExpired()
                };

                LoggedUser = loggedUser;

                return loggedUser;
            }
            catch (FirebaseAuthException authException)
            {
                switch (authException.Reason)
                {
                    case AuthErrorReason.WrongPassword:
                        throw new WrongPasswordException();
                    case AuthErrorReason.UnknownEmailAddress:
                        throw new EmailNotFoundException();
                    case AuthErrorReason.EmailExists:
                        throw new UserCreationException("Email already exists");
                    default:
                        throw;
                }

                throw;
            }

        }


        public async Task<string> SignInWithOAuth()
        {

            await CrossGoogleClient.Current.LoginAsync();
            var googleToken = CrossGoogleClient.Current.AccessToken;

            if (string.IsNullOrEmpty(googleToken))
            {
                throw new Exception("Echec de l'authentification OAuth");
            }
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Google, googleToken);
            //await _cacheService.InsertItem("CurrentUser", auth.User, CacheType.Local);
            return auth.FirebaseToken;

        }

        public Task SignOut()
        {

            LoggedUser = null;
            return Task.CompletedTask;
            //CrossGoogleClient.Current.Logout();
            //return Task.CompletedTask;
            ////return _cacheService.RemoveItem("CurrentUser", CacheType.Local);
        }
    }


}