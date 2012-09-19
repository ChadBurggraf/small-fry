//-----------------------------------------------------------------------------
// <copyright file="ParameterType.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    internal sealed class ParameterType
    {
        public ParameterType(string name, Type type)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name", "name must contain a value.");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type", "type cannot be null.");
            }

            this.Name = name;
            this.Type = type;
        }

        public string Name { get; private set; }

        public Type Type { get; private set; }
    }
}
