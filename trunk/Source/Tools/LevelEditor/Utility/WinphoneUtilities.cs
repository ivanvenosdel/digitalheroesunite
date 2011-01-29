#region Using Statements
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
#endregion

namespace MonsterEscape.Utility
{
    /// <summary>
    /// Random Helper Functions
    /// </summary>
    public static class WinphoneUtilities
    {
        /// <summary>
        /// For Save Dialog
        /// </summary>
        public static void SaveSerializeToXML<T>(T obj, string location)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(location);
            serializer.Serialize(writer, obj);
            writer.Close();
        }

        /// <summary>
        /// For Open Dialog
        /// </summary>
        public static T OpenDeserializeFromXML<T>(string location)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(location);
            T obj = (T)deserializer.Deserialize(reader);
            reader.Close();

            return obj;
        }


        public static void SerializeToXML<T>(T obj, string location)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(TitleContainer.OpenStream(DeviceManager.Instance.ContentManager.RootDirectory + location));
            serializer.Serialize(writer, obj);
            writer.Close();
        }

        public static T DeserializeFromXML<T>(string location)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(T));
            TextReader reader = new StreamReader(TitleContainer.OpenStream(DeviceManager.Instance.ContentManager.RootDirectory + "/" + location));
            T obj = (T)deserializer.Deserialize(reader);
            reader.Close();

            return obj;
        }
    }
}
