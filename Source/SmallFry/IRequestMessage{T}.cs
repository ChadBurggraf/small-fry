//-----------------------------------------------------------------------------
// <copyright file="IRequestMessage{T}.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;
    
    public interface IRequestMessage<T> : IRequestMessage
    {
        T RequestObject { get; }
    }
}