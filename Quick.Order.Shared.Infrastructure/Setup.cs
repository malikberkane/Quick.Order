using FreshMvvm;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.Shared.Infrastructure.Authentication;
using Quick.Order.Shared.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quick.Order.Shared.Infrastructure
{
    public class Setup
    {
        public static void Init()
        {
            FreshIOC.Container.Register<IRestaurantRepository, RestaurantRepository>();
            FreshIOC.Container.Register<IAuthenticationService, FirebaseAuthenticationService>();

        }

    }
}
