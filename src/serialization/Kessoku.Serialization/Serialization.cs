using Kessoku.Serialization.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kessoku.Serialization
{
    public class Serialization
    {
        public static void Serialize<T>(T obj, BinaryWriter writer) where T : ISerialization
        {
            obj.Serialize(writer);
        }
        public static byte[] Serialize<T>(T obj) where T : ISerialization
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(mem))
                {
                    Serialize(obj, writer);
                    return mem.ToArray();
                }
            }
        }
        public static byte[] Serialize(ISerialization serialization)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(mem))
                {
                    serialization.Serialize(bw);
                    mem.Position = 0;
                    return mem.ToArray();
                }
            }
        }

        public static T Deserialize<T>(byte[] array) where T : ISerialization, new()
        {
            using (MemoryStream mem = new MemoryStream(array))
            {
                using (BinaryReader reader = new BinaryReader(mem))
                {
                    return Deserialize<T>(reader);
                }
            }
        }

        public static T Deserialize<T>(BinaryReader reader) where T : ISerialization, new()
        {
            T d = new T();

            d.Deserialize(reader);

            return d;
        }


        public static object Deserialize(BinaryReader reader, Type type)
        {
            var instance = Activator.CreateInstance(type);
            if (instance is ISerialization ser)
            {
                ser.Deserialize(reader);
                return instance;
            }
            else
            {
                throw new Exception("Fall to deserialize type: given type is not implements ISerialization.");
            }
        }
        public static object Deserialize(byte[] array, Type type)
        {
            using (MemoryStream mem = new MemoryStream(array))
            {
                using (BinaryReader reader = new BinaryReader(mem))
                {
                    return Deserialize(reader, type);
                }
            }
        }
    }
}
