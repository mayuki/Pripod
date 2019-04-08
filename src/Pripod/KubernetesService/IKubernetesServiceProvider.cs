using Pripod.Data;
using System.Threading.Tasks;

namespace Pripod.KubernetesService
{
    public interface IKubernetesServiceProvider
    {
        string AccessToken { get; }
        string HostName { get; }
        string Namespace { get; }
        string KubernetesServiceEndPoint { get; }
        bool IsRunningOnKubernetes { get; }

        Task<IPodInfo> CreatePodInfoAsync(OwnerReferencesResolve resolve);
    }
}
