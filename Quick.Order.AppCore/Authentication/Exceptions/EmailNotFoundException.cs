namespace Quick.Order.AppCore.Authentication.Exceptions
{
    public class EmailNotFoundException: System.Exception
    {
        public EmailNotFoundException():base("Email non trouvé")
        {

        }
    }

    public class WrongPasswordException : System.Exception
    {
        public WrongPasswordException() : base("Mot de passe invalide")
        {

        }
    }

    public class UserCreationException : System.Exception
    {
        public UserCreationException(string message) : base(message)
        {

        }
    }
}
