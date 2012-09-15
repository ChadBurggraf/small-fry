namespace SmallFry
{
    using System;
    using System.IO;
    using ServiceStack.Text;

    public sealed class JsonFormat : IFormat
    {
        static JsonFormat()
        {
            JsConfig.EmitCamelCaseNames = true;
            JsConfig.IncludeNullValues = false;
        }

        public object Deserialize(Type type, Stream stream)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            return JsonSerializer.DeserializeFromStream(type, stream);
        }

        public bool Equals(IFormat other)
        {
            if ((object)other != null)
            {
                return this.GetType().Equals(other.GetType());
            }

            return false;
        }

        public void Serialize(object value, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream", "stream cannot be null.");
            }

            if (value != null)
            {
                JsonSerializer.SerializeToStream(value, value.GetType(), stream);
            }
        }
    }
}