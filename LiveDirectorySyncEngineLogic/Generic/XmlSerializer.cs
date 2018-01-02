using System;
using System.Collections.Generic;
using System.Text;

namespace LiveDirectorySyncEngineLogic.Generic
{
    public class XmlSerializer<T> where T : class
    {
        ////private string m_sConfigFileName;
        //private T data = default(T);

        //public XmlSerializer(string fileName)
        //{
        //    m_sConfigFileName = fileName + ".xml";
        //}

        //public T Data
        //{
        //    get { return data; }
        //    set { data = value; }
        //}

        // Load file
        public T Load(string fileName)
        {
            T data = default(T);
            if (System.IO.File.Exists(fileName))
            {
                System.IO.StreamReader srReader = System.IO.File.OpenText(fileName);
                Type tType = typeof(T); // data.GetType();
                System.Xml.Serialization.XmlSerializer xsSerializer = new System.Xml.Serialization.XmlSerializer(tType);
                object oData = xsSerializer.Deserialize(srReader);
                data = (T)oData;
                srReader.Close();
            }
            return data;
        }

        // Save file
        public void Save(string fileName, T data)
        {
            if (data == null)
            {
                throw new NullReferenceException("parameter data is null.");
            }
            Type tType = data.GetType();
            if (!tType.IsSerializable)
            {
                throw new InvalidOperationException($"Object of type {tType} is not serializable");
            }
            System.IO.StreamWriter swWriter = System.IO.File.CreateText(fileName);
            System.Xml.Serialization.XmlSerializer xsSerializer = new System.Xml.Serialization.XmlSerializer(tType);
            xsSerializer.Serialize(swWriter, data);
            swWriter.Close();
        }
    }
}
