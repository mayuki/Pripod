using System;
using System.IO;

namespace Pripod.KubernetesService
{
    public class WindowsKubernetesServiceProvider : KubernetesServiceProviderBase
    {
        private bool? _isRunningOnKubernetes;

        public override bool IsRunningOnKubernetes
            => _isRunningOnKubernetes ?? (bool)(_isRunningOnKubernetes = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")));

        public override string AccessToken
            => throw new NotImplementedException();

        public override string HostName
            => throw new NotImplementedException();

        public override string KubernetesServiceEndPoint
            => throw new NotImplementedException();

        public override string Namespace
            => throw new NotImplementedException();
    }
}
