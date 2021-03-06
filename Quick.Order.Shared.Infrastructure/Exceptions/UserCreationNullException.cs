using System;
using System.Collections.Generic;
using System.Text;

namespace Quick.Order.Shared.Infrastructure.Exceptions
{
    public class UserCreationNullException: System.Exception
    {
        public UserCreationNullException():base("Firebase returns null after user creation")
        {

        }
    }

    public class SignInResultNullException : System.Exception
    {
        public SignInResultNullException() : base("Firebase returns null after sign in method called")
        {

        }
    }

    public class UnableToParseLocalOrderException : Exception
    {
    
        public UnableToParseLocalOrderException() : base("Unable to parse local order: problem with order id or order date")
        {
        }

       
    }

}
