using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace SPI.Ship
{
    public class Engine : IXmlSerializable
    {
        public delegate void EngineEventHandler(Engine engine);

        public event EngineEventHandler EngineChanged;

        private float _Power;

        public Ship Ship { get; private set; }

        public string ID { get; private set; }

        public Vector3 Position { get; private set; }

        public Vector3 Orientation { get; private set; }

        public float MaxThrust { get; private set; }

        public float Diameter { get; private set; }

        public float Power
        {
            get
            {
                return _Power;
            }

            set
            {
                if (_Power != value)
                {
                    _Power = value;
                    if (EngineChanged != null)
                    {
                        EngineChanged(this);
                    }
                }
            }
        }

        public Engine(string id)
        {
            this.Ship = null;
            this.ID = id;
        }

        public Engine(string id, Engine proto)
        {
            this.Ship = null;
            this.ID = id;
            this.MaxThrust = proto.MaxThrust;
            this.Diameter = proto.Diameter;
        }

        public Engine(Ship ship, Engine proto)
        {
            this.Ship = ship;
            this.ID = proto.ID;
            this.Position = proto.Position;
            this.Orientation = proto.Orientation;
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