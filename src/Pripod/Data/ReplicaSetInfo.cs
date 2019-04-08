using Pripod.Internal.Kubernetes;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pripod.Data
{
    [DebuggerDisplay("ReplicaSet: {Namespace}/{Name}")]
    public class ReplicaSetInfo : IMetaV1ObjectInfo, ITypeMetaInfo
    {
        private readonly ReplicaSet _object;

        string ITypeMetaInfo.Kind => _object.Kind;
        string ITypeMetaInfo.ApiVersion => _object.ApiVersion;

        public string Namespace => _object.Metadata.Namespace;
        public string Name => _object.Metadata.Name;
        public IReadOnlyDictionary<string, string> Annotations => _object.Metadata.Annotations;
        public IReadOnlyDictionary<string, string> Labels => _object.Metadata.Labels;

        internal ReplicaSetInfo(ReplicaSet obj)
        {
            _object = obj;
        }

        public override string ToString()
        {
            return $"{Namespace}/{Name}";
        }
    }
}
