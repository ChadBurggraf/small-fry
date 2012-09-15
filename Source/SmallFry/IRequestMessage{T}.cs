namespace SmallFry
{
    using System;
    
    public interface IRequestMessage<T> : IRequestMessage
    {
        T RequestObject { get; }
    }
}