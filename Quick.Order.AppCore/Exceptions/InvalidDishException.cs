using System;
using System.Collections.Generic;
using System.Text;

namespace Quick.Order.AppCore.Exceptions
{
    public class InvalidDishException: System.Exception
    {
        public InvalidDishException(string message):base(message)
        {

        }


    }

    public class RestaurantNotFoundException : System.Exception
    {
        public RestaurantNotFoundException() : base("retaurant not found")
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
}
