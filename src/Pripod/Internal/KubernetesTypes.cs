using Pripod.Internal;
using Pripod.Internal.Utf8Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pripod.Internal.Kubernetes
{
    internal interface ITypeMeta
    {
        string Kind { get; }
        string ApiVersion { get; }
    }

    internal interface IMetaV1Object
    {
        MetaV1Metadata Metadata { get; }
    }

    internal class MetaV1Metadata
    {
        public string Name { get; } = default!;
        public string GenerateName { get; } = default!;
        public string Namespace { get; } = default!;
        public string SelfLink { get; } = default!;
        public string Uid { get; } = default!;
        public string ResourceVersion { get; } = default!;
        public string CreationTimestamp { get; } = default!;
        public IReadOnlyDictionary<string, string> Labels { get; } = JsonReaderHelper.EmptyStringDictionary;
        public IReadOnlyDictionary<string, string> Annotations { get; } = JsonReaderHelper.EmptyStringDictionary;
        public IReadOnlyList<OwnerReference> OwnerReferences { get; } = Array.Empty<OwnerReference>();

        public MetaV1Metadata(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "name":
                                    Name = reader.ReadString();
                                    break;
                                case "generateName":
                                    GenerateName = reader.ReadString();
                                    break;
                                case "namespace":
                                    Namespace = reader.ReadString();
                                    break;
                                case "selfLink":
                                    SelfLink = reader.ReadString();
                                    break;
                                case "uid":
                                    Uid = reader.ReadString();
                                    break;
                                case "resourceVersion":
                                    ResourceVersion = reader.ReadString();
                                    break;
                                case "creationTimestamp":
                                    CreationTimestamp = reader.ReadString();
                                    break;
                                case "labels":
                                    Labels = JsonReaderHelper.ReadAsStringDictionary(ref reader);
                                    break;
                                case "annotations":
                                    Annotations = JsonReaderHelper.ReadAsStringDictionary(ref reader);
                                    break;
                                case "ownerReferences":
                                    OwnerReferences = JsonReaderHelper.ReadAsArray(ref reader, OwnerReference.Create);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class OwnerReference
    {
        public static OwnerReference Create(ref JsonReader reader) => new OwnerReference(ref reader);

        public string ApiVersion { get; set; } = default!;
        public string Kind { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Uid { get; set; } = default!;

        public OwnerReference(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "name":
                                    Name = reader.ReadString();
                                    break;
                                case "uid":
                                    Uid = reader.ReadString();
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class Pod : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; } = default!;
        public string ApiVersion { get; } = default!;
        public MetaV1Metadata Metadata { get; } = default!;
        public PodSpec Spec { get; } = default!;
        public PodStatus Status { get; } = default!;

        public Pod(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                case "spec":
                                    Spec = new PodSpec(ref reader);
                                    break;
                                case "status":
                                    Status = new PodStatus(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }

        }
    }

    internal class PodSpec
    {
        public string NodeName { get; } = default!;
        public string? ServiceAccountName { get; }
        public string? ServiceAccount { get; }

        public PodSpec(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "nodeName":
                                    NodeName = reader.ReadString();
                                    break;
                                case "serviceAccountName":
                                    ServiceAccountName = reader.ReadString();
                                    break;
                                case "serviceAccount":
                                    ServiceAccount = reader.ReadString();
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class PodStatus
    {
        public string? HostIP { get; }
        public string? PodIP { get; }

        public PodStatus(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "hostIP":
                                    HostIP = reader.ReadString();
                                    break;
                                case "podIP":
                                    PodIP = reader.ReadString();
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class ReplicaSet : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; } = default!;
        public string ApiVersion { get; } = default!;
        public MetaV1Metadata Metadata { get; } = default!;

        public ReplicaSet(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }

        }
    }

    internal class Deployment : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; } = default!;
        public string ApiVersion { get; } = default!;
        public MetaV1Metadata Metadata { get; } = default!;

        public Deployment(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }

        }
    }

    internal class DaemonSet : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; } = default!;
        public string ApiVersion { get; } = default!;
        public MetaV1Metadata Metadata { get; } = default!;

        public DaemonSet(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class StatefulSet : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; } = default!;
        public string ApiVersion { get; } = default!;
        public MetaV1Metadata Metadata { get; } = default!;

        public StatefulSet(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class Job : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; } = default!;
        public string ApiVersion { get; } = default!;
        public MetaV1Metadata Metadata { get; } = default!;

        public Job(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }

    internal class CronJob : IMetaV1Object, ITypeMeta
    {
        public string Kind { get; } = default!;
        public string ApiVersion { get; } = default!;
        public MetaV1Metadata Metadata { get; } = default!;

        public CronJob(ref JsonReader reader)
        {
            reader.ReadIsBeginObjectWithVerify();

            while (true)
            {
                var token = reader.GetCurrentJsonToken();
                switch (token)
                {
                    case JsonToken.String:
                        {
                            var propName = reader.ReadPropertyName();
                            switch (propName)
                            {
                                case "kind":
                                    Kind = reader.ReadString();
                                    break;
                                case "apiVersion":
                                    ApiVersion = reader.ReadString();
                                    break;
                                case "metadata":
                                    Metadata = new MetaV1Metadata(ref reader);
                                    break;
                                default:
                                    reader.ReadNextBlock();
                                    break;
                            }
                        }
                        break;
                    case JsonToken.ValueSeparator:
                        reader.ReadNext();
                        break;
                    case JsonToken.EndObject:
                        reader.ReadNext();
                        return;
                }
            }
        }
    }
}
