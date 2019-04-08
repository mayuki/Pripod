using System;
using System.Collections.Generic;
using System.Text;

namespace Pripod.Data
{
    public interface IMetaV1ObjectInfo
    {
        /// <summary>
        /// Gets namespace of the resource.
        /// </summary>
        string Namespace { get; }

        /// <summary>
        /// Gets name of the resource.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets annotations of the resource.
        /// </summary>
        IReadOnlyDictionary<string, string> Annotations { get; }

        /// <summary>
        /// Gets labels of the resource.
        /// </summary>
        IReadOnlyDictionary<string, string> Labels { get; }
    }
}
