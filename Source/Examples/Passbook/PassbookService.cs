namespace Passbook
{
    using System;
    using System.Data;
    using SmallFry;

    public static class PassbookService
    {
        public static void GetLatestVersionOfPass(IRequestMessage request, IResponseMessage response)
        {
            Pass pass;

            using (Repository repository = new Repository())
            {
                pass = repository.GetPass(
                    request.RouteValue<string>("passTypeIdentifier"),
                    request.RouteValue<string>("serialNumber"));
            }

            if (pass != null)
            {
                if (pass.LastUpdated > request.Headers.Get<DateTime>("If-Modified-Since"))
                {
                    response.ResponseObject = pass.Data;
                }
                else
                {
                    response.SetStatus(StatusCode.NotModified);
                }
            }
            else
            {
                response.SetStatus(StatusCode.NotFound);
            }
        }

        public static void GetSerialNumbersForDevice(IRequestMessage request, IResponseMessage response)
        {
            DeviceSerialNumbers serialNumbers;

            using (Repository repository = new Repository())
            {
                serialNumbers = repository.GetDeviceSerialNumbers(
                    request.RouteValue<string>("deviceLibraryIdentifier"),
                    request.RouteValue<string>("passTypeIdentifier"),
                    request.QueryValue<DateTime?>("passesUpdatedSince"));
            }

            if (serialNumbers != null)
            {
                response.ResponseObject = serialNumbers;
            }
            else
            {
                response.SetStatus(StatusCode.NoContent);
            }
        }

        public static void RegisterDeviceForPushNotifications(IRequestMessage<Registration> request, IResponseMessage response)
        {
            if (!string.IsNullOrEmpty(request.RequestObject.PushToken))
            {
                using (Repository repository = new Repository())
                {
                    using (IDbTransaction transaction = repository.Connection.BeginTransaction())
                    {
                        try
                        {
                            Pass pass = repository.GetPass(
                                request.RouteValue<string>("passTypeIdentifier"),
                                request.RouteValue<string>("serialNumber"),
                                transaction);

                            if (pass != null)
                            {
                                Registration registration = new Registration()
                                {
                                    Created = DateTime.UtcNow,
                                    DeviceLibraryIdentifier = request.RouteValue<string>("deviceLibraryIdentifier"),
                                    LastUpdated = DateTime.UtcNow,
                                    PassId = pass.Id,
                                    PushToken = request.RequestObject.PushToken
                                };

                                if (repository.CreateRegistration(registration, transaction))
                                {
                                    response.SetStatus(StatusCode.Created);
                                }
                            }
                            else
                            {
                                response.SetStatus(StatusCode.NotFound);
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            else
            {
                response.SetStatus(StatusCode.BadRequest);
            }
        }

        public static void UnregisterDevice(IRequestMessage request, IResponseMessage response)
        {
            using (Repository repository = new Repository())
            {
                repository.DeleteRegistration(
                    request.RouteValue<string>("deviceLibraryIdentifier"),
                    request.RouteValue<string>("passTypeIdentifier"),
                    request.RouteValue<string>("serialNumber"));
            }
        }
    }
}