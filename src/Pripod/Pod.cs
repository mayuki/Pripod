using Pripod.Data;
using Pripod.KubernetesService;
using System;

namespace Pripod
{
    /// <summary>
    /// Provides Pod information.
    /// </summary>
    public class Pod
    {
        private static readonly object _syncObject = new object();

        private static IPodInfo _current;
        private static IKubernetesServiceProvider _serviceProvider = GetDefaultProvider();

        /// <summary>
        /// Gets information about Pod running this process.
        /// </summary>
        public static IPodInfo Current
        {
            get
            {
                if (_current == null)
                {
                    Initialize();
                }
                return _current;
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
        public static void Initialize(OwnerReferencesResolve resolve = OwnerReferencesResolve.All, bool throwOnFail = false)
        {
            lock (_syncObject)
            {
                if (_current != null) return;

                if (!_serviceProvider.IsRunningOnKubernetes)
                {
                    _current = new PseudoPodInfo();
                    return;
                }

                try
                {
                    _current = _serviceProvider.CreatePodInfoAsync(resolve).GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    if (throwOnFail)
                    {
                        throw;
                    }
                    else
                    {
                        _current = new PseudoPodInfo();
                    }
                }
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
