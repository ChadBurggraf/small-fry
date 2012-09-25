namespace Passbook
{
    using System;
    using System.Configuration;
    using System.Web;
    using SmallFry;

    public class Global : HttpApplication
    {
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .WithHostEncoding(new GzipDeflateEncoding())
                .WithHostFormat(new JsonFormat())
                .WithService("Passbook v1", "passbook/v1")
                    .WithEndpoint("passes/{passTypeIdentifier}/{serialNumber}")
                        .Get(PassbookService.GetLatestVersionOfPass)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}")
                        .Get(PassbookService.GetSerialNumbersForDevice)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}")
                        .Post<Registration>(PassbookService.RegisterDeviceForPushNotifications)
                        .Delete(PassbookService.UnregisterDevice);
        }

        public void Application_Start(object sender, EventArgs e)
        {
            Global.RegisterServices(ServiceHost.Instance.Services);
        }
    }
}