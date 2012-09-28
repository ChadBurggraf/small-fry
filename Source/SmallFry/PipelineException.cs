//-----------------------------------------------------------------------------
// <copyright file="PipelineException.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Represents an exception thrown during the execution of a service
    /// endpoint's pipeline.
    /// </summary>
    [Serializable]
    public sealed class PipelineException : Exception
    {
        private const string DefaultMessage = "An exception occurred while processing a service endpoint's request/response pipeline. Please review ErrorType, ServiceName, Route, RequestUri, MethodType, and InnerException for more information.";
        private PipelineErrorType errorType;
        private string serviceName, route;
        private Uri requestUri;
        private MethodType methodType;

        /// <summary>
        /// Initializes a new instance of the PipelineException class.
        /// </summary>
        public PipelineException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the PipelineException class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public PipelineException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PipelineException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public PipelineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PipelineException class with information about the source of the exception in the request/response pipeline.
        /// </summary>
        /// <param name="errorType">The type of pipeline error the exception represents.</param>
        /// <param name="serviceName">The name of the service the that threw the exception.</param>
        /// <param name="route">The route being handled when the exception was thrown.</param>
        /// <param name="requestUri">The request URI being handled when the exception was thrown.</param>
        /// <param name="methodType">The method type being handled when the exception was thrown.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = "Reviewed.")]
        public PipelineException(PipelineErrorType errorType, string serviceName, string route, Uri requestUri, MethodType methodType, Exception innerException)
            : base(PipelineException.DefaultMessage, innerException)
        {
            this.errorType = errorType;
            this.serviceName = serviceName;
            this.route = route;
            this.requestUri = requestUri;
            this.methodType = methodType;
        }

        /// <summary>
        /// Initializes a new instance of the PipelineException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private PipelineException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.errorType = (PipelineErrorType)Enum.ToObject(typeof(PipelineErrorType), info.GetInt32("errorType"));
                this.serviceName = info.GetString("serviceName");
                this.route = info.GetString("route");
                this.requestUri = (Uri)info.GetValue("requestUri", typeof(Uri));
                this.methodType = (MethodType)Enum.ToObject(typeof(MethodType), info.GetInt32("methodType"));
            }
        }

        /// <summary>
        /// Gets the type of the error this exception represents.
        /// </summary>
        public PipelineErrorType ErrorType
        {
            get { return this.errorType; }
        }

        /// <summary>
        /// Gets the method type being handled when this exception was thrown.
        /// </summary>
        public MethodType MethodType
        {
            get { return this.methodType; }
        }

        /// <summary>
        /// Gets the request URI being handled when this exception was thrown.
        /// </summary>
        public Uri RequestUri
        {
            get { return this.requestUri; }
        }

        /// <summary>
        /// Gets the route being handled when this exception was thrown.
        /// </summary>
        public string Route
        {
            get { return this.route; }
        }

        /// <summary>
        /// Gets the name of the service that threw the exception.
        /// </summary>
        public string ServiceName
        {
            get { return this.serviceName; }
        }

        /// <summary>
        /// When overridden in a derived class, sets the SerializationInfo with information about the exception.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("errorType", (int)this.errorType);
                info.AddValue("serviceName", this.serviceName);
                info.AddValue("route", this.route);
                info.AddValue("requestUri", this.requestUri);
                info.AddValue("methodType", (int)this.methodType);
            }
        }
    }
}