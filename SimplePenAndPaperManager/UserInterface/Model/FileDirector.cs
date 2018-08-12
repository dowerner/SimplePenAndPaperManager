using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Windows.Ink;

namespace SimplePenAndPaperManager.UserInterface.Model
{
    public class FileDirector
    {
        public void SaveToXml<T>(string path, T data)
        {
            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

                using (XmlWriter writer = XmlWriter.Create(path, settings))
                {
                    serializer.WriteObject(writer, data);
                }
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
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                T data;

                using (XmlReader reader = XmlReader.Create(path))
                {
                    data = (T)serializer.ReadObject(reader);
                }

                return data;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public void SaveTerrain(string path, StrokeCollection terrain)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Create);
                terrain.Save(fileStream);
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public StrokeCollection LoadTerrain(string path)
        {
            FileStream fileStream = null;
            StrokeCollection terrain = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                terrain = new StrokeCollection(fileStream);
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return terrain;
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
