using Pripod.Internal.Kubernetes;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pripod.Data
{
    [DebuggerDisplay("CronJob: {Namespace}/{Name}")]
    public class CronJobInfo : IMetaV1ObjectInfo, ITypeMetaInfo
    {
        private readonly CronJob _object;

        string ITypeMetaInfo.Kind => _object.Kind;
        string ITypeMetaInfo.ApiVersion => _object.ApiVersion;

        public string Namespace => _object.Metadata.Namespace;
        public string Name => _object.Metadata.Name;
        public IReadOnlyDictionary<string, string> Annotations => _object.Metadata.Annotations;
        public IReadOnlyDictionary<string, string> Labels => _object.Metadata.Labels;

        internal CronJobInfo(CronJob obj)
        {
            _object = obj;
        }

        public override string ToString()
        {
            return $"{Namespace}/{Name}";
        }
    }
}
