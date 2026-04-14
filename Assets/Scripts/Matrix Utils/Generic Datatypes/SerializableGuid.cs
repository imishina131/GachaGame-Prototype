using System;
using UnityEngine;

/// <summary>
/// Represents a globally unique identifier (GUID) that is serializable with Unity and usable in game scripts.
/// </summary>
[Serializable]
public struct SerializableGuid : IEquatable<SerializableGuid> {
    [SerializeField, HideInInspector] public uint Part1;
    [SerializeField, HideInInspector] public uint Part2;
    [SerializeField, HideInInspector] public uint Part3;
    [SerializeField, HideInInspector] public uint Part4;

    public static SerializableGuid Empty => new(0, 0, 0, 0);

    public SerializableGuid(uint val1, uint val2, uint val3, uint val4) {
        Part1 = val1;
        Part2 = val2;
        Part3 = val3;
        Part4 = val4;
    }

    public SerializableGuid(Guid guid) {
        byte[] bytes = guid.ToByteArray();
        Part1 = BitConverter.ToUInt32(bytes, 0);
        Part2 = BitConverter.ToUInt32(bytes, 4);
        Part3 = BitConverter.ToUInt32(bytes, 8);
        Part4 = BitConverter.ToUInt32(bytes, 12);
    }

    public static SerializableGuid NewGuid() => Guid.NewGuid().ToSerializableGuid();

    public static SerializableGuid FromHexString(string hexString) {
        if (hexString.Length != 32) return Empty;
        uint p1 = Convert.ToUInt32(hexString.Substring(0, 8), 16);
        uint p2 = Convert.ToUInt32(hexString.Substring(8, 8), 16);
        uint p3 = Convert.ToUInt32(hexString.Substring(16, 8), 16);
        uint p4 = Convert.ToUInt32(hexString.Substring(24, 8), 16);
        p1 = ((p1 & 0xFF000000) >> 24) | ((p1 & 0x00FF0000) >> 8) | ((p1 & 0x0000FF00) << 8) | ((p1 & 0x000000FF) << 24);
        p2 = ((p2 & 0xFFFF0000) >> 16) | ((p2 & 0x0000FFFF) << 16);
        p3 = ((p3 & 0xFF000000) >> 24) | ((p3 & 0x00FF0000) >> 8) | ((p3 & 0x0000FF00) << 8) | ((p3 & 0x000000FF) << 24);
        return new(p1, p2, p3, p4);
    }

    public string ToHexString() {
        uint p1 = ((Part1 & 0xFF000000) >> 24) | ((Part1 & 0x00FF0000) >> 8) | ((Part1 & 0x0000FF00) << 8) | ((Part1 & 0x000000FF) << 24);
        uint p2 = ((Part2 & 0xFFFF0000) >> 16) | ((Part2 & 0x0000FFFF) << 16);
        uint p3 = ((Part3 & 0xFF000000) >> 24) | ((Part3 & 0x00FF0000) >> 8) | ((Part3 & 0x0000FF00) << 8) | ((Part3 & 0x000000FF) << 24);
        return $"{p1:X8}{p2:X8}{p3:X8}{Part4:X8}";
    }

    public Guid ToGuid() {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(Part1).CopyTo(bytes, 0);
        BitConverter.GetBytes(Part2).CopyTo(bytes, 4);
        BitConverter.GetBytes(Part3).CopyTo(bytes, 8);
        BitConverter.GetBytes(Part4).CopyTo(bytes, 12);
        return new Guid(bytes);
    }

    public static implicit operator Guid(SerializableGuid serializableGuid) => serializableGuid.ToGuid();  
    public static implicit operator SerializableGuid(Guid guid) => new(guid);

    public override bool Equals(object obj) {
        return obj is SerializableGuid guid && Equals(guid);
    }

    public bool Equals(SerializableGuid other) {
        return Part1 == other.Part1 && Part2 == other.Part2 && Part3 == other.Part3 && Part4 == other.Part4;
    }

    public override int GetHashCode() {
        return HashCode.Combine(Part1, Part2, Part3, Part4);
    }

    public static bool operator ==(SerializableGuid left, SerializableGuid right) => left.Equals(right);
    public static bool operator !=(SerializableGuid left, SerializableGuid right) => !(left == right); 
}
public static class SerializableGuidExtensions {
    public static SerializableGuid ToSerializableGuid(this Guid systemGuid) {
        byte[] bytes = systemGuid.ToByteArray();
        return new(
            BitConverter.ToUInt32(bytes, 0),
            BitConverter.ToUInt32(bytes, 4),
            BitConverter.ToUInt32(bytes, 8),
            BitConverter.ToUInt32(bytes, 12)
        );
    }

    public static Guid ToSystemGuid(this SerializableGuid serializableGuid) {
        byte[] bytes = new byte[16];
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part1), 0, bytes, 0, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part2), 0, bytes, 4, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part3), 0, bytes, 8, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(serializableGuid.Part4), 0, bytes, 12, 4);
        return new Guid(bytes);
    }
}