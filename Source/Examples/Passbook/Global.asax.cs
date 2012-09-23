namespace Passbook
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Dapper;
    using ServiceStack.Text;
    using SmallFry;

    public class Global : HttpApplication
    {
        public void Application_Start(object sender, EventArgs e)
        {
            ServiceHost.Instance.Services
                .WithHostEncoding(new GzipDeflateEncoding())
                .WithHostFormat(new JsonFormat())
                .WithService("Passbook v1", "api/v1")
                    .WithEndpoint("passes/{passTypeIdentifier}/{serialNumber}", new { passTypeIdentifier = typeof(Guid), serialNumber = typeof(Guid) })
                        .Get(Global.GetPass)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}", new { deviceLibraryIdentifier = typeof(Guid), passTypeIdentifier = typeof(Guid) })
                        .Get(Global.GetSerialNumbers)
                    .WithEndpoint("devices/{deviceLibraryIdentifier}/registrations/{passTypeIdentifier}/{serialNumber}", new { deviceLibraryIdentifier = typeof(Guid), passTypeIdentifier = typeof(Guid), serialNumber = typeof(Guid) })
                        .Post<Registration>(Global.RegisterDevice)
                        .Delete(Global.UnregisterDevice);
        }

        public static void GetPass(IRequestMessage request, IResponseMessage response)
        {
            using (Repository repo = new Repository())
            {
                Pass pass = repo.GetPass(
                    request.RouteValue<Guid>("passTypeIdentifier"),
                    request.RouteValue<Guid>("serialNumber"));

                if (pass != null)
                {
                    response.ResponseObject = pass.Data;
                }
                else
                {
                    
                }
            }
        }

        public static void GetSerialNumbers(IRequestMessage request, IResponseMessage response)
        {
            Guid deviceLibraryIdentifier = request.RouteValue<Guid>("deviceLibraryIdentifier");
            Guid passTypeIdentifier = request.RouteValue<Guid>("passTypeIdentifier");
            DateTime? passesUpdatedSince = request.RequestUri.GetQueryValue<DateTime?>("passesUpdatedSince");
        }

        public static void RegisterDevice(IRequestMessage<Registration> request, IResponseMessage response)
        {
            Guid deviceLibraryIdentifier = request.RouteValue<Guid>("deviceLibraryIdentifier");
            Guid passTypeIdentifier = request.RouteValue<Guid>("passTypeIdentifier");
            Guid serialNumber = request.RouteValue<Guid>("serialNumber");
        }

        public static void UnregisterDevice(IRequestMessage request, IResponseMessage response)
        {
            Guid deviceLibraryIdentifier = request.RouteValue<Guid>("deviceLibraryIdentifier");
            Guid passTypeIdentifier = request.RouteValue<Guid>("passTypeIdentifier");
            Guid serialNumber = request.RouteValue<Guid>("serialNumber");
        }

        public sealed class Pass
        {
            private PassData data;

            public Pass()
            {
                this.Registrations = new List<Registration>();
            }

            public DateTime CreateDate { get; set; }

            public PassData Data
            {
                get
                {
                    return this.data;
                }

                set
                {
                    this.data = value;
                    this.DataString = value != null ? data.ToJson() : null;
                }
            }

            public string DataString { get; private set; }

            public DateTime LastUpdateDate { get; set; }

            public string PassTypeIdentifier { get; set; }

            public IList<Registration> Registrations { get; private set; }

            public string SerialNumber { get; set; }
        }

        public sealed class PassData
        {
            public decimal CreditRemaining { get; set; }
        }

        public sealed class Registration
        {
            public string DeviceLibraryIdentifier { get; set; }

            public string PushToken { get; set; }
        }

        public sealed class SerialNumbersResult
        {
            private List<Guid> serialNumbers = new List<Guid>();

            public DateTime LastUpdated { get; set; }

            public IList<Guid> SerialNumbers
            {
                get { return this.serialNumbers; }
            }
        }

        public sealed class Repository : IDisposable
        {
            private SQLiteConnection connection;
            private string path;
            private bool disposed;

            public Repository()
            {
                this.path = HttpContext.Current.Server.MapPath("~/App_Data/Passbook.sqlite");
                Repository.EnsureDatabase(this.path);
                this.connection = new SQLiteConnection(Repository.GetConnectionString(this.path));
                this.connection.Open();
            }

            ~Repository()
            {
                this.Dispose(false);
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            public Pass GetPass(Guid passTypeIdentifier, Guid serialNumber)
            {
                return this.connection.Query<Pass>(
                    "SELECT * FROM [Pass] WHERE [PassTypeIdentifier] = @PassTypeIdentifier AND [SerialNumber] = @SerialNumber;",
                    new { PassTypeIdentifier = passTypeIdentifier, SerialNumber = serialNumber }).FirstOrDefault();
            }

            private static void EnsureDatabase(string path)
            {
                const string Sql =
@"CREATE TABLE IF NOT EXISTS [Pass]
(
    [CreateDate] DATETIME NOT NULL,
    [Data] TEXT,
    [LastUpdateDate] DATETIME NOT NULL,
    [PassTypeIdentifier] CHAR(32) NOT NULL PRIMARY KEY,
    [SerialNumber] CHAR(32) NOT NULL,
    CONSTRAINT [UC_PassTypeIdentifier_SerialNumber] UNIQUE([PassTypeIdentifier], [SerialNumber])
);

CREATE TABLE IF NOT EXISTS [Registration]
(
    [DeviceLibraryIdentifier] CHAR(32) NOT NULL,
    [PassId] CHAR(32) NOT NULL,
    [PushToken] CHAR(32) NOT NULL,
    CONSTRAINT [PK_Registration] PRIMARY KEY([PassId], [DeviceLibraryIdentifier])
);";

                if (!File.Exists(path))
                {
                    using (SQLiteConnection connection = new SQLiteConnection(Repository.GetConnectionString(path)))
                    {
                        connection.Open();
                        connection.Execute(Sql);
                    }
                }
            }

            private static string GetConnectionString(string path)
            {
                SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
                builder.DataSource = path;
                builder.DateTimeKind = DateTimeKind.Utc;
                builder.JournalMode = SQLiteJournalModeEnum.Off;
                builder.SyncMode = SynchronizationModes.Off;
                return builder.ToString();
            }

            private void Dispose(bool disposing)
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        if (this.connection != null)
                        {
                            this.connection.Dispose();
                            this.connection = null;
                        }
                    }

                    this.disposed = true;
                }
            }
        }
    }
}