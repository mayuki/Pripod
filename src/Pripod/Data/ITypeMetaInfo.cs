using System;
using System.Collections.Generic;
using System.Text;

namespace Pripod.Data
{
    public interface ITypeMetaInfo
    {
        string Kind { get; }
        string ApiVersion { get; }
    }
}
