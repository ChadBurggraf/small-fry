namespace Passbook
{
    using System;
    using System.Configuration;
    using System.Web;
    using SmallFry;

    /*
     * An example pass and registration is automatically created in the SQLite database.
     * Visit ~/passbook/v1/passes/com.company.pass.example/ABC123 to see the example pass data.
     * The example registration's device library ID is 123456789.
     */

    public class Global : HttpApplication
    {
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            var patterns = new { passTypeIdentifier = @"^([\w]\.?)+$" };

            serviceCollection
                .WithHostEncoding(new GzipDeflateEncoding())
                .WithHostFormat(new JsonFormat())
                .WithService("Passbook v1", "api/v1")
                    .WithEndpoint("passes/{passTypeIdentifier}/{serialNumber}", null, patterns)
                        .Get(PassbookService.GetLatestVersionOfPass)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}", null, patterns)
                        .Get(PassbookService.GetSerialNumbersForDevice)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}", null, patterns)
                        .Post<Registration>(PassbookService.RegisterDeviceForPushNotifications)
                        .Delete(PassbookService.UnregisterDevice);
        }

        public void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext httpContext = HttpContext.Current;

            if (httpContext != null)
            {
                try
                {
                    httpContext.Response.Headers.Remove("Server");
                }
                catch (PlatformNotSupportedException)
                {
                }
            }
        }

        public void Application_Start(object sender, EventArgs e)
        {
            Global.RegisterServices(ServiceHost.Instance.Services);
        }
    }
}