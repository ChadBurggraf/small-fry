namespace SmallFry
{
    using System;
    
    public interface IRequestMessage<T> : IRequestMessage
    {
        T Request { get; }
    }
}