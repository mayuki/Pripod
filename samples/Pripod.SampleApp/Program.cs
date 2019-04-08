using Pripod;
using Pripod.Data;
using System;

namespace Pripod.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Pod.Initialize(throwOnFail: true);

                Console.WriteLine($"IsRunningOnKubernetes: {Pod.Current.IsRunningOnKubernetes}");

                WriteAnnotationsAndLabels(Pod.Current);
                Console.WriteLine($"  HostIP: {Pod.Current.HostIP}");
                Console.WriteLine($"  PodIP: {Pod.Current.PodIP}");

                WriteAnnotationsAndLabels(Pod.Current.StatefulSet);
                WriteAnnotationsAndLabels(Pod.Current.Deployment);
                WriteAnnotationsAndLabels(Pod.Current.DaemonSet);
                WriteAnnotationsAndLabels(Pod.Current.ReplicaSet);
                WriteAnnotationsAndLabels(Pod.Current.Job);
                WriteAnnotationsAndLabels(Pod.Current.CronJob);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            System.Threading.Thread.Sleep(45 * 1000);
        }

        private static void WriteAnnotationsAndLabels(IMetaV1ObjectInfo o)
        {
            if (o is null) return;

            Console.WriteLine();
            Console.WriteLine($"{((ITypeMetaInfo)o).Kind}: {o}");
            Console.WriteLine("  Annotations:");
            foreach (var keyValue in o.Annotations)
            {
                Console.WriteLine($"    - {keyValue.Key}: {keyValue.Value}");
            }
            Console.WriteLine("  Labels:");
            foreach (var keyValue in o.Labels)
            {
                Console.WriteLine($"    - {keyValue.Key}: {keyValue.Value}");
            }
        }
    }
}
