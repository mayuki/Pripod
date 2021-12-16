using Pripod.Data;
using Pripod.KubernetesService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pripod
{
    /// <summary>
    /// Provides Pod information.
    /// </summary>
    public class Pod
    {
        private static readonly object _syncObject = new object();

        private static IPodInfo? _current;
        private static IKubernetesServiceProvider _serviceProvider = GetDefaultProvider();

        /// <summary>
        /// Gets information about Pod running this process.
        /// </summary>
        public static IPodInfo Current
        {
            get
            {
                if (_current is null)
                {
                    Initialize();
                }
#pragma warning disable CS8603 // Possible null reference return.
                return _current;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }

        /// <summary>
        /// Registers Kubernetes service provider.
        /// </summary>
        /// <param name="provider"></param>
        public static void RegisterServiceProvider(IKubernetesServiceProvider provider)
            => _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));

        /// <summary>
        /// Gets Pod information explicitly and initialize it.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="throwOnFail"></param>
        /// <param name="forceRefresh"></param>
        public static void Initialize(OwnerReferencesResolve resolve = OwnerReferencesResolve.All, bool throwOnFail = false, bool forceRefresh = false)
        {
            InitializeAsync(resolve, throwOnFail, forceRefresh).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets Pod information explicitly and initialize it.
        /// </summary>
        /// <param name="resolve"></param>
        /// <param name="throwOnFail"></param>
        /// <param name="forceRefresh"></param>
        public static async Task InitializeAsync(OwnerReferencesResolve resolve = OwnerReferencesResolve.All, bool throwOnFail = false, bool forceRefresh = false)
        {
            if (_current is not null && !forceRefresh)
            {
                return;
            }

            IPodInfo podInfo;
            if (!_serviceProvider.IsRunningOnKubernetes)
            {
                podInfo = new PseudoPodInfo();
            }
            else
            {
                try
                {
                    podInfo = await _serviceProvider.CreatePodInfoAsync(resolve).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    if (throwOnFail)
                    {
                        throw;
                    }
                    else
                    {
                        podInfo = new PseudoPodInfo();
                    }
                }
            }

            lock (_syncObject)
            {
                if (_current is not null && !forceRefresh)
                {
                    return;
                }

                _current = podInfo;
            }
        }

        private static IKubernetesServiceProvider GetDefaultProvider()
        {
            return (Environment.OSVersion.Platform == PlatformID.Unix)
                ? (IKubernetesServiceProvider)new UnixKubernetesServiceProvider()
                : (IKubernetesServiceProvider)new WindowsKubernetesServiceProvider();
        }
    }
}
