using Pripod.Internal.Kubernetes;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pripod.Data
{
    [DebuggerDisplay("DaemonSet: {Namespace}/{Name}")]
    public class DaemonSetInfo : IMetaV1ObjectInfo, ITypeMetaInfo
    {
        private readonly DaemonSet _object;

        string ITypeMetaInfo.Kind => _object.Kind;
        string ITypeMetaInfo.ApiVersion => _object.ApiVersion;

        public string Namespace => _object.Metadata.Namespace;
        public string Name => _object.Metadata.Name;
        public IReadOnlyDictionary<string, string> Annotations => _object.Metadata.Annotations;
        public IReadOnlyDictionary<string, string> Labels => _object.Metadata.Labels;

        internal DaemonSetInfo(DaemonSet obj)
        {
            _object = obj;
        }

        public override string ToString()
        {
            return $"{Namespace}/{Name}";
        }
    }
}
