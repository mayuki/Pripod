using Pripod.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using PodObject = Pripod.Internal.Kubernetes.Pod;

namespace Pripod.Data
{
    public interface IPodInfo : IMetaV1ObjectInfo
    {
        /// <summary>
        /// Determines whether the process is running on Kubernetes cluster.
        /// </summary>
        bool IsRunningOnKubernetes { get; }

        /// <summary>
        /// Gets information about Deployment related to the Pod.
        /// </summary>
        DeploymentInfo Deployment { get; }
        /// <summary>
        /// Gets information about ReplicaSet related to the Pod.
        /// </summary>
        ReplicaSetInfo ReplicaSet { get; }
        /// <summary>
        /// Gets information about DaemonSet related to the Pod.
        /// </summary>
        DaemonSetInfo DaemonSet { get; }
        /// <summary>
        /// Gets information about StatefulSet related to the Pod.
        /// </summary>
        StatefulSetInfo StatefulSet { get; }
        /// <summary>
        /// Gets information about Job related to the Pod.
        /// </summary>
        JobInfo Job { get; }
        /// <summary>
        /// Gets information about CronJob related to the Pod.
        /// </summary>
        CronJobInfo CronJob { get; }

        /// <summary>
        /// Gets the name of the node running the Pod.
        /// </summary>
        string NodeName { get; }
        /// <summary>
        /// Gets IP address of the node running the Pod.
        /// </summary>
        string HostIP { get; }
        /// <summary>
        /// Gets IP address of the Pod.
        /// </summary>
        string PodIP { get; }
    }

    [DebuggerDisplay("Pod: {Namespace}/{Name} @ {NodeName}")]
    internal class PodInfo : IPodInfo, ITypeMetaInfo
    {
        private readonly PodObject _object;

        public bool IsRunningOnKubernetes => true;

        public DeploymentInfo Deployment { get; internal set; }
        public ReplicaSetInfo ReplicaSet { get; internal set; }
        public DaemonSetInfo DaemonSet { get; internal set; }
        public StatefulSetInfo StatefulSet { get; internal set; }
        public JobInfo Job { get; internal set; }
        public CronJobInfo CronJob { get; internal set; }

        string ITypeMetaInfo.Kind => _object.Kind;
        string ITypeMetaInfo.ApiVersion => _object.ApiVersion;

        public string Namespace => _object.Metadata.Namespace;
        public string Name => _object.Metadata.Name;
        public string NodeName => _object.Spec.NodeName;
        public string HostIP => _object.Status.HostIP;
        public string PodIP => _object.Status.PodIP;
        public IReadOnlyDictionary<string, string> Annotations => _object.Metadata.Annotations;
        public IReadOnlyDictionary<string, string> Labels => _object.Metadata.Labels;

        internal PodInfo(PodObject obj)
        {
            _object = obj;
        }

        public override string ToString()
        {
            return $"{Namespace}/{Name} @ {NodeName}";
        }
    }

    [DebuggerDisplay("PseudoPod: {Namespace}/{Name} @ {NodeName}")]
    internal class PseudoPodInfo : IPodInfo, ITypeMetaInfo
    {
        public bool IsRunningOnKubernetes => false;

        public DeploymentInfo Deployment => null;

        public ReplicaSetInfo ReplicaSet => null;

        public DaemonSetInfo DaemonSet => null;

        public StatefulSetInfo StatefulSet => null;
        public JobInfo Job => null;
        public CronJobInfo CronJob => null;

        string ITypeMetaInfo.Kind => "Pod";
        string ITypeMetaInfo.ApiVersion => "v1";

        public string Namespace => "unknown";

        public string Name => Environment.MachineName;

        public string NodeName => Environment.MachineName;

        public string HostIP => null;

        public string PodIP => null;

        public IReadOnlyDictionary<string, string> Annotations => JsonReaderHelper.EmptyStringDictionary;

        public IReadOnlyDictionary<string, string> Labels => JsonReaderHelper.EmptyStringDictionary;

        public override string ToString()
        {
            return $"{Namespace}/{Name} @ {NodeName}";
        }
    }
}
