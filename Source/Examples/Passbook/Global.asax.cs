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
                Pass pass = repo.GetPasses(
                    request.RouteValue<string>("passTypeIdentifier"), 
                    request.RouteValue<string>("serialNumber")).FirstOrDefault();

                if (pass != null)
                {
                    if (request.Headers.Get<DateTime>("Last-Modified") > pass.LastUpdateDate)
                    {
                        response.SetStatus(StatusCode.NotModified);
                    }
                    else
                    {
                        response.ResponseObject = pass.Data;
                    }
                }
                else
                {
                    response.SetStatus(StatusCode.NotFound);
                }
            }
        }

        public static void GetSerialNumbers(IRequestMessage request, IResponseMessage response)
        {
            using (Repository repo = new Repository())
            {
                Pass pass = repo.GetPasses(request.RouteValue<string>("passTypeIdentifier")).FirstOrDefault();

                if (pass != null)
                {
                    IEnumerable<Registration> registrations = repo.GetRegistrations(
                        pass.Id,
                        request.RouteValue<string>("deviceLibraryIdentifier"),
                        request.RequestUri.GetQueryValue<DateTime?>("passesUpdatedSince"));

                    if (registrations.Any())
                    {
                        response.ResponseObject = new
                        {
                            SerialNumbers = registrations.Select(r => r.Ser
                        };
                    }
                    else
                    {
                        response.SetStatus(StatusCode.NoContent);
                    }
                }
                else
                {
                    response.SetStatus(StatusCode.NotFound);
                }
            }

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
                this.Data = null;
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
                    this.DataString = value != null ? data.ToJson() : "{}";
                }
            }

            public string DataString { get; private set; }

            public long Id { get; set; }

            public DateTime LastUpdateDate { get; set; }

            public string PassTypeIdentifier { get; set; }

            public string SerialNumber { get; set; }
        }

        public sealed class PassData
        {
            public decimal CreditRemaining { get; set; }
        }

        public sealed class Registration
        {
            public DateTime CreateDate { get; set; }

            public string DeviceLibraryIdentifier { get; set; }

            public long Id { get; set; }

            public long PassId { get; set; }

            public string PushToken { get; set; }

            public DateTime LastUpdateDate { get; set; }
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

            public IEnumerable<Pass> GetPasses(string passTypeIdentifier, string serialNumber = null)
            {
                const string Sql =
@"SELECT * 
FROM [Pass] 
WHERE 
    [PassTypeIdentifier] = @PassTypeIdentifier";

                string sql = Sql;

                if (!string.IsNullOrEmpty(serialNumber))
                {
                    sql += @"
    AND [SerialNumber] = @SerialNumber";
                }

                sql += @"
ORDER BY [LastUpdateDate] DESC;";

                return this.connection.Query<Pass>(
                    sql,
                    new { PassTypeIdentifier = passTypeIdentifier, SerialNumber = serialNumber });
            }

            public IEnumerable<Registration> GetRegistrations(long passId, string deviceLibraryIdentifier, DateTime? updatedSince)
            {
                const string Sql =
@"SELECT *
FROM [Registration]
WHERE
    [PassId] = @PassId
    AND [DeviceLibraryIdentifier] = @DeviceLibraryIdentifier";

                string sql = Sql;

                if (updatedSince != null)
                {
                    sql += @"
    AND [LastUpdateDate] = @UpdatedSince";
                }

                sql += @"
ORDER BY [LastUpdateDate] DESC;";

                return this.connection.Query<Registration>(sql, new { PassId = passId, DeviceLibraryIdentifier = deviceLibraryIdentifier, UpdatedSince = updatedSince });
            }

            private static void EnsureDatabase(string path)
            {
                const string Sql =
@"CREATE TABLE IF NOT EXISTS [Pass]
(
    [CreateDate] DATETIME NOT NULL,
    [Data] TEXT,
    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [LastUpdateDate] DATETIME NOT NULL,
    [PassTypeIdentifier] VARCHAR(64) NOT NULL,
    [SerialNumber] VARCHAR(64) NOT NULL,
    CONSTRAINT [UC_PassTypeIdentifier_SerialNumber] UNIQUE([PassTypeIdentifier], [SerialNumber])
);

CREATE TABLE IF NOT EXISTS [Registration]
(
    [CreateDate] DATETIME NOT NULL,
    [DeviceLibraryIdentifier] VARCHAR(64) NOT NULL,
    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [LastUpdateDate] DATETIME NOT NULL,
    [PassId] INTEGER NOT NULL,
    [PushToken] VARCHAR(64) NOT NULL,
    CONSTRAINT [UC_PassId_DeviceLibraryIdentifier] UNIQUE([PassId], [DeviceLibraryIdentifier])
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