using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Numerics;
using Casper.Network.SDK.Converters;
using Org.BouncyCastle.Utilities.Encoders;

namespace Casper.Network.SDK.Types
{
    public abstract class ExecutableDeployItem
    {
        [JsonPropertyName("args")]
        [JsonConverter(typeof(GenericListConverter<NamedArg, NamedArg.NamedArgConverter>))]
        public List<NamedArg> RuntimeArgs { get; init; }

        public abstract byte Tag();

        public abstract string JsonPropertyName();
    }

    public class ModuleBytesDeployItem : ExecutableDeployItem
    {
        [JsonPropertyName("module_bytes")]
        [JsonConverter(typeof(HexBytesConverter))]
        public byte[] ModuleBytes { get; init; }

        public ModuleBytesDeployItem()
        {
        }

        public ModuleBytesDeployItem(byte[] moduleBytes)
        {
            RuntimeArgs = new List<NamedArg>();
            ModuleBytes = moduleBytes;
        }

        public ModuleBytesDeployItem(BigInteger amount)
        {
            RuntimeArgs = new List<NamedArg>();
            RuntimeArgs.Add(new NamedArg("amount", CLValue.U512(amount)));
            ModuleBytes = Array.Empty<byte>();
        }

        public override byte Tag() => 0;

        public override string JsonPropertyName() => "ModuleBytes";
    }

    public class StoredContractByHashDeployItem : ExecutableDeployItem
    {
        [JsonPropertyName("hash")]
        [JsonConverter(typeof(HexBytesConverter))]
        public byte[] Hash { get; init; }

        [JsonPropertyName("entry_point")] public string EntryPoint { get; init; } = "";

        public StoredContractByHashDeployItem()
        {
        }
        
        public StoredContractByHashDeployItem(string hash, string entryPoint, List<NamedArg> args = null)
        {
            Hash = Hex.Decode(hash);
            EntryPoint = entryPoint;
            RuntimeArgs = args ?? new List<NamedArg>();
        }

        public override byte Tag() => 1;

        public override string JsonPropertyName() => "StoredContractByHash";
    }

    public class StoredContractByNameDeployItem : ExecutableDeployItem
    {
        [JsonPropertyName("name")] public string Name { get; init; } = "";

        [JsonPropertyName("entry_point")] public string EntryPoint { get; init; } = "";

        public StoredContractByNameDeployItem()
        {
        }
        
        public StoredContractByNameDeployItem(string name, string entryPoint, List<NamedArg> args = null)
        {
            Name = name;
            EntryPoint = entryPoint;
            RuntimeArgs = args ?? new List<NamedArg>();
        }

        public override byte Tag() => 2;

        public override string JsonPropertyName() => "StoredContractByName";
    }

    public class StoredVersionedContractByHashDeployItem : ExecutableDeployItem
    {
        [JsonPropertyName("hash")]
        [JsonConverter(typeof(HexBytesConverter))]
        public byte[] Hash { get; init; }

        [JsonPropertyName("version")] public uint? Version { get; init; }

        [JsonPropertyName("entry_point")] public string EntryPoint { get; init; } = "";
        
        public StoredVersionedContractByHashDeployItem()
        {
        }
        
        public StoredVersionedContractByHashDeployItem(string hash, uint? version, string entryPoint,
            List<NamedArg> args = null)
        {
            Hash = Hex.Decode(hash);
            Version = version;
            EntryPoint = entryPoint;
            RuntimeArgs = args ?? new List<NamedArg>();
        }

        public override byte Tag() => 3;

        public override string JsonPropertyName() => "StoredVersionedContractByHash";
    }

    public class StoredVersionedContractByNameDeployItem : ExecutableDeployItem
    {
        [JsonPropertyName("name")] public string Name { get; init; } = "";

        [JsonPropertyName("version")] public uint? Version { get; init; }

        [JsonPropertyName("entry_point")] public string EntryPoint { get; init; } = "";

        public StoredVersionedContractByNameDeployItem()
        {
        }
        
        public StoredVersionedContractByNameDeployItem(string name, uint? version, string entryPoint,
            List<NamedArg> args = null)
        {
            Name = name;
            Version = version;
            EntryPoint = entryPoint;
            RuntimeArgs = args ?? new List<NamedArg>();
        }

        public override byte Tag() => 4;

        public override string JsonPropertyName() => "StoredVersionedContractByName";
    }

    public class TransferDeployItem : ExecutableDeployItem
    {
        public TransferDeployItem()
        {
        }
        
        public TransferDeployItem(BigInteger amount, PublicKey target, CLValue sourcePurse, ulong? id = null)
        {
            var targetHash = target.GetAccountHash();

            RuntimeArgs = new List<NamedArg>();
            RuntimeArgs.Add(new NamedArg("amount", CLValue.U512(amount)));
            RuntimeArgs.Add(new NamedArg("target", CLValue.ByteArray(targetHash)));

            var optionValue = id == null ? CLValue.OptionNone(new CLTypeInfo(CLType.U64)) : CLValue.Option(CLValue.U64((ulong) id));
            RuntimeArgs.Add(new NamedArg("id", optionValue));
        }

        public override byte Tag() => 5;

        public override string JsonPropertyName() => "Transfer";
    }
}