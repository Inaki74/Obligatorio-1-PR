using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Domain.BusinessObjects
{
    public static class DeepCopyExtensionMethod
    {
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T) formatter.Deserialize(stream);
            }
        }
    }
}
