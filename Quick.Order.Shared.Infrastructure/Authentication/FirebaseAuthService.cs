using System;
using Firebase.Auth;
using System.Threading.Tasks;
using Plugin.GoogleClient;
using Quick.Order.AppCore.Models;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.Shared.Infrastructure.Exceptions;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.BusinessOperations;

namespace Quick.Order.Shared.Infrastructure.Authentication
{
    public class FirebaseAuthenticationService : IAuthenticationService
    {
        public const string WebAPIkey = "AIzaSyBhWjAJpwbA21UwB6I4bFvnP9tvMcFbaTs";
        private readonly ILoggerService loggerService;
        private readonly BackOfficeSessionService backOfficeSessionService;
        private readonly ILocalHistoryService localHistoryService;

        public FirebaseAuthenticationService(ILoggerService loggerService, BackOfficeSessionService backOfficeRestaurantService, ILocalHistoryService localHistoryService)
        {
            this.loggerService = loggerService;
            backOfficeSessionService = backOfficeRestaurantService;
            this.localHistoryService = localHistoryService;
        }
        public AutenticatedRestaurantAdmin LoggedUser { get; private set; }

        public Task SendPasswordResetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new Exception("Login text null or empty");
            }
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            return authProvider.SendPasswordResetEmailAsync(email);
        }
        public async Task<AutenticatedRestaurantAdmin> CreateUser(string username, string email, string password)
        {

            try
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
            catch (FirebaseAuthException authException)
            {
                
                loggerService.Log(authException);

                throw new Exception(AuthErrorReasonToMessage(authException.Reason));

            }


        }


        public string AuthErrorReasonToMessage(AuthErrorReason authErrorReason)
        {
            switch (authErrorReason)
            {
                case AuthErrorReason.Undefined:
                    return "Wrong password";
                case AuthErrorReason.OperationNotAllowed:
                    return "operation not allowed";
                case AuthErrorReason.UserDisabled:
                    return "User disabled";
                case AuthErrorReason.UserNotFound:
                    return "User not found";
                case AuthErrorReason.InvalidProviderID:
                    return "Invalid provider id";
                case AuthErrorReason.InvalidAccessToken:
                    return "Invalid access token";
                case AuthErrorReason.LoginCredentialsTooOld:
                    return "Login credentials too old";
                case AuthErrorReason.MissingRequestURI:
                    return "Missing request uri";
                case AuthErrorReason.SystemError:
                    return "System error";
                case AuthErrorReason.InvalidEmailAddress:
                    return "Invalid email adress";
                case AuthErrorReason.MissingPassword:
                    return "Missing password";
                case AuthErrorReason.WeakPassword:
                    return "Invalid provider id";
                case AuthErrorReason.EmailExists:
                    return "Email exists";
                case AuthErrorReason.MissingEmail:
                    return "Missing email";
                case AuthErrorReason.UnknownEmailAddress:
                    return "Unkown email";
                case AuthErrorReason.WrongPassword:
                    return "Wrong password";
                case AuthErrorReason.TooManyAttemptsTryLater:
                    return "Too many attemps, try later";
                case AuthErrorReason.MissingRequestType:
                    return "Missing request type";
                case AuthErrorReason.ResetPasswordExceedLimit:
                    return "Reset password exceeded limit";
                case AuthErrorReason.InvalidIDToken:
                    return "Invalid id toke,";
                case AuthErrorReason.MissingIdentifier:
                    return "Invalid provider id";
                //case AuthErrorReason.InvalidIdentifier:
                //    break;
                //case AuthErrorReason.AlreadyLinked:
                //    break;
                //case AuthErrorReason.StaleIDToken:
                //    break;
                case AuthErrorReason.DuplicateCredentialUse:
                    return "Duplicate credential use";
                default:
                    return "An unexpected error occured";
            }

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

                loggerService.SetUserId(auth.User.Email);


                var loggedUser = new AutenticatedRestaurantAdmin
                {
                    RestaurantAdmin = new RestaurantAdmin() { Email = auth.User.Email, Name = auth.User.DisplayName },
                    AuthenticationToken = auth.FirebaseToken,
                    AuthenticationExpired = auth.IsExpired()
                };

                LoggedUser = loggedUser;
                localHistoryService.SetUserEmail(email);
                return loggedUser;
            }
            catch (FirebaseAuthException authException)
            {

                loggerService.Log(authException);

                throw new Exception(AuthErrorReasonToMessage(authException.Reason));

            }

        }


        public async Task<AutenticatedRestaurantAdmin> SignInWithOAuth()
        {

            await CrossGoogleClient.Current.LoginAsync();
            var googleToken = CrossGoogleClient.Current.AccessToken;

            if (string.IsNullOrEmpty(googleToken))
            {
                throw new Exception("Echec de l'authentification OAuth");
            }
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIkey));
            var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Google, googleToken);
            loggerService.SetUserId(auth.User.Email);

            var loggedUser = new AutenticatedRestaurantAdmin
            {
                RestaurantAdmin = new RestaurantAdmin() { Email = auth.User.Email, Name = auth.User.DisplayName },
                AuthenticationToken = auth.FirebaseToken,
                AuthenticationExpired = auth.IsExpired()
            };

            LoggedUser = loggedUser;

            return loggedUser;

        }

        public Task SignOut()
        {

            LoggedUser = null;
            backOfficeSessionService.SetCurrentRestaurantSessionToNull();
            return Task.CompletedTask;

        }
    }


}