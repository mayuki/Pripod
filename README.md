# Pripod
[![NuGet package](https://img.shields.io/nuget/v/Pripod.svg)](https://nuget.org/packages/Pripod)

Pripod enables you to easily access Pod information from the .NET Core app inside a Pod.

Access information related to pods such as Deployment and ReplicaSet using the Kubernetes API.

# Features
- No configuration
  - If the Kubernetes cluster enables RBAC enabled, you need to assign service account to a Pod.
- Minimal API surface (`Pod.Current.`)
  - **Pod**: Name, Namespace, HostIP, PodIP, Labels, Annotations, NodeName
    - **Deployment,ReplicaSet,DaemonSet,StatefulSet,Job,CronJob**: Name, Namespace, Labels, Annotations
- No dependencies. No need to install `KubernetesClient`, `Json.NET`, etc...

# Usage

## Code sample and outputs
```csharp
using Pripod;

// Optional: Explicitly initialize to improve performance on first-time access.
// Pod.Initialize();

// Determines whether this process is running on the Kubernetes cluster.
Console.WriteLine($"IsRunningOnKubernetes: {Pod.Current.IsRunningOnKubernetes}");

// Show Pod information
Console.WriteLine($"Pod: {Pod.Current.Namespace}/{Pod.Current.Name} @ {Pod.Current.NodeName}");
Console.WriteLine("Labels:");
foreach (var keyValue in Pod.Current.Labels)
{
    Console.WriteLine($"  - {keyValue.Key}: {keyValue.Value}");
}
Console.WriteLine($"HostIP: {Pod.Current.HostIP}");
Console.WriteLine($"PodIP: {Pod.Current.PodIP}");

// Show Deployment information
Console.WriteLine($"Deployment: {Pod.Current.Deployment?.Namespace}/{Pod.Current.Deployment?.Name}");
```

```
IsRunningOnKubernetes: True
Pod: default/consoleapp1-595b95b5f7-xsdjc @ docker-for-desktop
Labels:
 - pod-template-hash: 1516516193
 - run: consoleapp1
HostIP: 192.168.0.1
PodIP: 10.1.0.14
Deployment: default/consoleapp1
```

## Install
```
PM> Install-Package Pripod
```
```
$ dotnet add package Pripod
```

# Requirements
- .NET Standard 2.0 or later
- Kubernetes 1.10 or later

# FYI
If you only need pod information, you can also use the Kubernetes Downward API. **You do not need this library for that.**
- [Expose Pod Information to Containers Through Files](https://kubernetes.io/docs/tasks/inject-data-application/downward-api-volume-expose-pod-information/)
- [Expose Pod Information to Containers Through Environment Variables](https://kubernetes.io/docs/tasks/inject-data-application/environment-variable-expose-pod-information/)

# Limitations
- Running pod with virtual-kubelet (e.g. Azure Container Instance) is not supported.
- Running pod on Windows instance is not supported.

# License
[MIT License](LICENSE)

Pripod contains part of [Utf8Json](https://github.com/neuecc/Utf8Json) that licensed under MIT License.
