using System;
using System.IO;
using System.Xml.Serialization;

namespace SimplePenAndPaperManager.UserInterface.Model
{
    public class FileDirector
    {
        public void SaveToXml<T>(string path, T data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                TextWriter writer = new StreamWriter(path);
                serializer.Serialize(writer, data);
                writer.Close(); 
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public T LoadFromXml<T>(string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(path);
                T data = (T)serializer.Deserialize(reader);
                return data;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        #region Singleton
        private FileDirector() { }

        public static FileDirector Instance
        {
            get
            {
                if (_instance == null) _instance = new FileDirector();
                return _instance;
            }
        }
        private static FileDirector _instance;
        #endregion
    }
}
