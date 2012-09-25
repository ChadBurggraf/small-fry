namespace Passbook
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Passbook.Models;

    public sealed class Repository : IDisposable
    {
        private SQLiteConnection connection;
        private bool disposed;

        public Repository(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path", "path must contain a value.");
            }

            if (!Path.IsPathRooted(path))
            {
                throw new ArgumentException("path must be fully qualified.", "path");
            }

            Repository.EnsureDatabase(path);
            this.connection = new SQLiteConnection(Repository.GetConnectionString(path));
            this.connection.Open();
        }

        ~Repository()
        {
            this.Dispose(false);
        }

        public IDbConnection Connection
        {
            get { return this.connection; }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool CreateRegistration(Registration registration, IDbTransaction transaction = null)
        {
            const string ExistingSql =
@"SELECT COUNT([Id])
FROM [Registration]
WHERE
    [PassId] = @PassId
    AND [DeviceLibraryIdentifier] = @DeviceLibraryIdentifier;";

            const string InsertSql =
@"INSERT INTO [Registration]([Created],[DeviceLibraryIdentifier],[LastUpdated],[PassId],[PushToken])
VALUES(@Created,@DeviceLibraryIdentifier,@LastUpdated,@PassId,@PushToken);
SELECT last_insert_rowid();";

            if (0 < this.connection.Query<long>(ExistingSql, registration, transaction).First())
            {
                registration.Id = this.connection.Query<long>(InsertSql, registration, transaction).First();
                return true;
            }

            return false;
        }

        public Pass GetPass(string passTypeIdentifier, string serialNumber, IDbTransaction transaction = null)
        {
            const string Sql =
@"SELECT *
FROM [Pass]
WHERE
    [PassTypeIdentifier] = @PassTypeIdentifier
    AND [SerialNumber] = @SerialNumber;";

            return this.connection.Query<Pass>(
                Sql,
                new { PassTypeIdentifier = passTypeIdentifier, SerialNumber = serialNumber },
                transaction).FirstOrDefault();
        }

        public DeviceSerialNumbers GetDeviceSerialNumbers(string deviceLibraryIdentifier, string passTypeIdentifier, DateTime? passesUpdatedSince, IDbTransaction transaction = null)
        {
            StringBuilder sb = new StringBuilder(
@"SELECT
    p.[LastUpdated],
    p.[SerialNumber]
FROM [Pass] p
    INNER JOIN [Registration] r ON p.[Id] = r.[PassId]
WHERE
    p.[PassTypeIdentifier] = @PassTypeIdentifier
    AND r.[DeviceLibraryIdentifier] = @DeviceLibraryIdentifier]");

            if (passesUpdatedSince != null)
            {
                sb.Append("    AND p.[LastUpdated] > @PassesUpdatedSince");
            }

            sb.Append("ORDER BY p.[LastUpdated] DESC;");

            DeviceSerialNumbers result = new DeviceSerialNumbers();
            int i = 0;

            foreach (var sn in this.connection.Query(
                sb.ToString(), 
                new { DeviceLibraryIdentifier = deviceLibraryIdentifier, PassTypeIdentifier = passTypeIdentifier, PassesUpdatedSince = passesUpdatedSince },
                transaction))
            {
                if (i == 0)
                {
                    result.LastUpdated = (DateTime)sn.LastUpdated;
                }

                result.SerialNumbers.Add((string)sn.SerialNumber);
                i++;
            }

            return i > 0 ? result : null;
        }

        public bool DeleteRegistration(string deviceLibraryidentifier, string passTypeIdentifier, string serialNumber, IDbTransaction transaction = null)
        {
            const string Sql =
@"DELETE FROM [Registration]
WHERE
    [Id] IN
    (
        SELECT r.[Id]
        FROM [Pass] p
            INNER JOIN [Registration] r ON p.[Id] = r.[PassId]
        WHERE
            p.[PassTypeIdentifier] = @PassTypeIdentifier
            AND p.[SerialNumber] = @SerialNumber
            AND r.[DeviceLibraryIdentifier] = @DeviceLibraryIdentifier
    );";

            return 0 < this.connection.Execute(
                Sql,
                new { DeviceLibraryIdentifier = deviceLibraryidentifier, PassTypeIdentifier = passTypeIdentifier, SerialNumber = serialNumber },
                transaction);
        }

        private static void EnsureDatabase(string path)
        {
            const string Sql =
@"CREATE TABLE IF NOT EXISTS [Pass]
(
    [Created] DATETIME NOT NULL,
    [Data] TEXT,
    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [LastUpdated] DATETIME NOT NULL,
    [PassTypeIdentifier] VARCHAR(64) NOT NULL,
    [SerialNumber] VARCHAR(64) NOT NULL,
    CONSTRAINT [UC_PassTypeIdentifier] UNIQUE([PassTypeIdentifier]),
    CONSTRAINT [UC_PassTypeIdentifier_SerialNumber] UNIQUE([PassTypeIdentifier], [SerialNumber])
);

CREATE TABLE IF NOT EXISTS [Registration]
(
    [Created] DATETIME NOT NULL,
    [DeviceLibraryIdentifier] VARCHAR(64) NOT NULL,
    [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    [LastUpdated] DATETIME NOT NULL,
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
            builder.JournalMode = SQLiteJournalModeEnum.Off;
            builder.SyncMode = SynchronizationModes.Off;
            builder.Version = 3;
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