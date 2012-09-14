namespace SmallFry
{
    using System;
    using System.IO;

    public sealed class JsonFormat : IFormat
    {
        public object Deserialize(Type type, Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Serialize(object value, Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}