using System;
using System.IO;

namespace Pripod.KubernetesService
{
    public class UnixKubernetesServiceProvider : KubernetesServiceProviderBase
    {
        private bool? _isRunningOnKubernetes;
        private string? _namespace;
        private string? _hostName;
        private string? _accessToken;
        private string? _kubernetesServiceEndPoint;

        public override bool IsRunningOnKubernetes
            => _isRunningOnKubernetes ?? (bool)(_isRunningOnKubernetes = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")));

        public override string AccessToken
            => _accessToken ?? (_accessToken = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/token"));

        public override string HostName
            => _hostName ?? (_hostName = Environment.GetEnvironmentVariable("HOSTNAME"));

        public override string KubernetesServiceEndPoint
            => _kubernetesServiceEndPoint ?? (_kubernetesServiceEndPoint = $"https://{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_HOST")}:{Environment.GetEnvironmentVariable("KUBERNETES_SERVICE_PORT")}");

        public override string Namespace
            => _namespace ?? (_namespace = File.ReadAllText("/var/run/secrets/kubernetes.io/serviceaccount/namespace"));
    }
}
