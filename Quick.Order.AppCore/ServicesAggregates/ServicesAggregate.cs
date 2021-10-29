namespace Quick.Order.AppCore.ServicesAggregate
{
    public class ServicesAggregate
    {
        public ServicesAggregate(BusinessServicesAggregate businessServicesAggregate, PluginServicesAggregate pluginServicesAggregate, RepositoriesAggregate repositories)
        {
            Business = businessServicesAggregate;
            Plugin = pluginServicesAggregate;
            Repositories = repositories;
        }

        public BusinessServicesAggregate Business { get; }
        public PluginServicesAggregate Plugin { get; }

        public RepositoriesAggregate Repositories { get; }
    }
}
