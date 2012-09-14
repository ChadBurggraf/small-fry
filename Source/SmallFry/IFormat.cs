namespace SmallFry
{
    using System;
    using System.IO;

    public interface IFormat
    {
        object Deserialize(Type type, Stream stream);

        void Serialize(object value, Stream stream);
    }
}