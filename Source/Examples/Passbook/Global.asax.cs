namespace Passbook
{
    using System;
    using System.Web;
    using Passbook.Models;
    using SmallFry;

    public class Global : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            this.RegisterServices(ServiceHost.Instance.Services);
        }

        public void RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection
                .WithHostEncoding(new GzipDeflateEncoding())
                .WithHostFormat(new JsonFormat())
                .WithService("Passbook v1", "api/v1")
                    .WithEndpoint("passes/{passTypeIdentifier}/{serialNumber}")
                        .Get(PassbookV1.GetLatestVersionOfPass)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}")
                        .Get(PassbookV1.GetSerialNumbersForDevice)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}")
                        .Post<Registration>(PassbookV1.RegisterDeviceForPushNotifications)
                        .Delete(PassbookV1.UnregisterDevice);
        }
    }
}