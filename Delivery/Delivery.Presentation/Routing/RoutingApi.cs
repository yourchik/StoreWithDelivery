namespace Delivery.PresentationApi.Routing;

public class RoutingApi(RoutesOrdersApi routesOrdersApi)
{
    public void RegisterAllRoutes(WebApplication app)
    {
        routesOrdersApi.Register(app);
    }
}