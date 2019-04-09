using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Pripod.Data;
using Pripod.Internal.Kubernetes;
using Pripod.Internal.Utf8Json;

namespace Pripod.KubernetesService
{
    public abstract class KubernetesServiceProviderBase : IKubernetesServiceProvider
    {
        public abstract string AccessToken { get; }
        public abstract string HostName { get; }
        public abstract string Namespace { get; }
        public abstract string KubernetesServiceEndPoint { get; }
        public abstract bool IsRunningOnKubernetes { get; }

        public bool SkipCertificationValidation { get; set; } = true;

        public async Task<IPodInfo> CreatePodInfoAsync(OwnerReferencesResolve resolve)
        {
            using (var httpClient = CreateHttpClient())
            {
                // Endpoints:
                // /api/v1/namespaces/<Namespace>/pods/<PodName>
                // /apis/apps/v1/namespaces/<Namespace>/replicasets/<ReplicSetName>
                // /apis/apps/v1/namespaces/<Namespace>/deployments/<DeploymentName>

                var podNamespace = Namespace;
                var podHostName = HostName;

                var reader = await GetJsonAsync(httpClient, $"/api/v1/namespaces/{podNamespace}/pods/{podHostName}");
                var pod = new Internal.Kubernetes.Pod(ref reader);
                var podInfo = new PodInfo(pod);

                if (resolve == OwnerReferencesResolve.All)
                {
                    await ResolveOwnerReferencesAsync(httpClient, podInfo, pod.Metadata.OwnerReferences);
                }

                return podInfo;
            }
        }

        private HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            if (SkipCertificationValidation)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = /* HttpClientHandler.DangerousAcceptAnyServerCertificateValidator; */ delegate { return true; };
            }

            return httpClient;
        }

        private async Task<JsonReader> GetJsonAsync(HttpClient httpClient, string apiPath)
        {
            return new JsonReader(await httpClient.GetByteArrayAsync(KubernetesServiceEndPoint + apiPath).ConfigureAwait(false));
        }

        private async Task ResolveOwnerReferencesAsync(HttpClient httpClient, PodInfo podInfo, IReadOnlyList<OwnerReference> ownerReferences)
        {
            foreach (var ownerReference in ownerReferences)
            {
                await ResolveOwnerReferenceAsync(httpClient, podInfo, ownerReference);
            }
        }

        private async Task ResolveOwnerReferenceAsync(HttpClient httpClient, PodInfo podInfo, OwnerReference ownerReference)
        {
            JsonReader reader;
            IMetaV1Object obj = default;

            switch (ownerReference.Kind)
            {
                case "StatefulSet" when ownerReference.ApiVersion == "apps/v1":
                    reader = await GetJsonAsync(httpClient, $"/apis/apps/v1/namespaces/{podInfo.Namespace}/statefulsets/{ownerReference.Name}");
                    var statefulSet = new StatefulSet(ref reader);
                    obj = statefulSet;
                    podInfo.StatefulSet = new StatefulSetInfo(statefulSet);
                    break;
                case "DaemonSet" when ownerReference.ApiVersion == "apps/v1":
                    reader = await GetJsonAsync(httpClient, $"/apis/apps/v1/namespaces/{podInfo.Namespace}/daemonsets/{ownerReference.Name}");
                    var daemonSet = new DaemonSet(ref reader);
                    obj = daemonSet;
                    podInfo.DaemonSet = new DaemonSetInfo(daemonSet);
                    break;
                case "ReplicaSet" when ownerReference.ApiVersion == "apps/v1":
                    reader = await GetJsonAsync(httpClient, $"/apis/apps/v1/namespaces/{podInfo.Namespace}/replicasets/{ownerReference.Name}");
                    var replicaSet = new ReplicaSet(ref reader);
                    obj = replicaSet;
                    podInfo.ReplicaSet = new ReplicaSetInfo(replicaSet);
                    break;
                case "Deployment" when ownerReference.ApiVersion == "apps/v1":
                    reader = await GetJsonAsync(httpClient, $"/apis/apps/v1/namespaces/{podInfo.Namespace}/deployments/{ownerReference.Name}");
                    var deployment = new Deployment(ref reader);
                    obj = deployment;
                    podInfo.Deployment = new DeploymentInfo(deployment);
                    break;
                case "Job" when ownerReference.ApiVersion == "batch/v1":
                    reader = await GetJsonAsync(httpClient, $"/apis/batch/v1/namespaces/{podInfo.Namespace}/jobs/{ownerReference.Name}");
                    var job = new Job(ref reader);
                    obj = job;
                    podInfo.Job = new JobInfo(job);
                    break;
                case "CronJob" when ownerReference.ApiVersion == "batch/v1beta1":
                    reader = await GetJsonAsync(httpClient, $"/apis/{ownerReference.ApiVersion}/namespaces/{podInfo.Namespace}/cronjobs/{ownerReference.Name}");
                    var cronJob = new CronJob(ref reader);
                    obj = cronJob;
                    podInfo.CronJob = new CronJobInfo(cronJob);
                    break;
            }

            if (obj != null)
            {
                await ResolveOwnerReferencesAsync(httpClient, podInfo, obj.Metadata.OwnerReferences);
            }
        }
    }
}

