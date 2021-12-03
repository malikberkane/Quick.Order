using Quick.Order.AppCore.Resources;

namespace Quick.Order.AppCore.Exceptions
{
    public class InvalidDishException: System.Exception
    {
        public InvalidDishException():base("Invalid dish definition: all fields must be filled")
        {

        }


    }

    public class ExistingDishSectionException : System.Exception
    {
        public ExistingDishSectionException() : base("the dish section already exists")
        {

        }


    }

    public class SectionNotFoundException : System.Exception
    {
        public SectionNotFoundException() : base("The section where to add the dish was not found")
        {

        }


    }

    public class ExistingDishException : System.Exception
    {
        public ExistingDishException() : base("the dish already exists")
        {

        }


    }

    public class RestaurantNotFoundException : System.Exception
    {
        public RestaurantNotFoundException() : base("retaurant not found")
        {

        }


    }


    public class NotResaurantForUserDefinedException : System.Exception
    {
        public NotResaurantForUserDefinedException() : base("No restaurant defined for user")
        {

        }


    }


    public class NoRestaurantForSessionException : System.Exception
    {
        public NoRestaurantForSessionException() : base("no restaurant registered for current session")
        {

        }


    }

    public class SettingAdminForNewRestaurantException : System.Exception
    {
        public SettingAdminForNewRestaurantException() : base("Current admin for session not found: impossible to create new restaurant")
        {

        }


    }

    public class UserNotLoggedException : System.Exception
    {
        public UserNotLoggedException() : base("Current user is not logged")
        {

        }


    }

    public class InvalidRestaurantCode : System.Exception
    {
        public InvalidRestaurantCode() : base("Invalid restaurant code")
        {

        }


    }


    public class NoNetworkException : System.Exception
    {
        public NoNetworkException() : base(AppResources.RestoreInternetConnectionAlert)
        {

        }


    }
}
