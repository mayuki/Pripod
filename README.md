# Pripod
Pripod enables you to easily access Pod information from the .NET Core app within a Pod.

# Usage
```csharp
using Pripod;

Console.WriteLine($"IsRunningOnKubernetes: {Pod.Current.IsRunningOnKubernetes}");
Console.WriteLine($"Pod: {Pod.Current.Namespace}/{Pod.Current.Name} @ {Pod.Current.NodeName}");
Console.WriteLine("Labels:");
foreach (var keyValue in Pod.Current.Labels)
{
    Console.WriteLine($"  - {keyValue.Key}: {keyValue.Value}");
}
Console.WriteLine($"HostIP: {Pod.Current.HostIP}");
Console.WriteLine($"PodIP: {Pod.Current.PodIP}");
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

# Requirements
- .NET Core 2.2 or later
- Kubernetes 1.10 or later

# FYI
If you only need pod information, you can also use the Kubernetes Downward API. You do not need this library for that. 
- [Expose Pod Information to Containers Through Files](https://kubernetes.io/docs/tasks/inject-data-application/downward-api-volume-expose-pod-information/)
- [Expose Pod Information to Containers Through Environment Variables](https://kubernetes.io/docs/tasks/inject-data-application/environment-variable-expose-pod-information/)

# Limitations
- Running pod with virtual-kubelet (e.g. Azure Container Instance) is not supported.
- Running pod on Windows instance is not supported.

# License
[MIT License](LICENSE)

Pripod contains part of [Utf8Json](https://github.com/neuecc/Utf8Json) that licensed under MIT License.
