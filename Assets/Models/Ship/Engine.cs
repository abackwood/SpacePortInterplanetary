using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Ship
{
    public class Engine : IXmlSerializable
    {
        public string ID { get; private set; }

        public Vector3 Position { get; private set; }

        public Vector3 Orientation { get; private set; }

        public float MaxThrust { get; private set; }

        public float Diameter { get; private set; }

        public float Power { get; set; }

        public Engine(string id)
        {
            this.ID = id;
        }

        public Engine(string id, Engine proto)
        {
            this.ID = id;
            this.MaxThrust = proto.MaxThrust;
            this.Diameter = proto.Diameter;
        }

        #region IXmlSerializable implementation

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader parentReader)
        {
            XmlReader reader = parentReader.ReadSubtree();
            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "Position":
                        float x = float.Parse(reader.GetAttribute("x"));
                        float y = float.Parse(reader.GetAttribute("y"));
                        float z = float.Parse(reader.GetAttribute("z"));
                        Position = new Vector3(x,y,z);
                        break;
                    case "Orientation":
                        x = float.Parse(reader.GetAttribute("x"));
                        y = float.Parse(reader.GetAttribute("y"));
                        z = float.Parse(reader.GetAttribute("z"));
                        Orientation = new Vector3(x,y,z);
                        break;
                    case "Thrust":
                        reader.Read();
                        MaxThrust = reader.ReadContentAsFloat();
                        break;
                    case "Diameter":
                        reader.Read();
                        Diameter = reader.ReadContentAsFloat();
                        break;
                }
            }

            reader.Close();
        }

        public void WriteXml(XmlWriter writer)
        {
            
        }

        #endregion
    }
}