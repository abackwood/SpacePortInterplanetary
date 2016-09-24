using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace SPI.Ship
{
    public class Ship : IXmlSerializable
    {
        public delegate void ShipEventHandler(Ship ship);

        public event ShipEventHandler ShipChanged;

        private Dictionary<string,Engine> engines;
        private Vector3d _Position;
        private Vector3 _Direction;

        public string Type { get; private set; }

        public string Name { get; private set; }

        public float DryMass { get; private set; }

        public Vector3 DryCenterOfMass { get; private set; }

        public Vector3d Position
        {
            get
            {
                return _Position;
            }

            set
            {
                if (_Position != value)
                {
                    _Position = value;
                    if (ShipChanged != null)
                    {
                        ShipChanged(this);
                    }
                }
            }
        }

        public Vector3 Direction
        {
            get
            {
                return _Direction;
            }

            set
            {
                if (_Direction != value)
                {
                    _Direction = value;
                    if (ShipChanged != null)
                    {
                        ShipChanged(this);
                    }
                }
            }
        }

        public Ship(string type)
        {
            this.Type = type;
            engines = new Dictionary<string,Engine>();
        }

        public Ship(Ship proto, Vector3d position)
        {
            this.Type = proto.Type;
            this.Name = proto.Name;
            this.DryMass = proto.DryMass;
            this.Position = position;
            this.DryCenterOfMass = proto.DryCenterOfMass;
        }

        #region IXmlSerializable implementation

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            
        }

        public void WriteXml(XmlWriter writer)
        {
            
        }

        #endregion

        public void ReadPrototypeXml(XmlReader parentReader)
        {
            XmlReader reader = parentReader.ReadSubtree();
            while (reader.Read())
            {
                switch (reader.Name)
                {
                    case "Name":
                        reader.Read();
                        Name = reader.ReadContentAsString();
                        break;
                    case "Mass":
                        reader.Read();
                        DryMass = reader.ReadContentAsFloat();
                        break;
                    case "CenterOfMass":
                        float x = float.Parse(reader.GetAttribute("x"));
                        float y = float.Parse(reader.GetAttribute("y"));
                        float z = float.Parse(reader.GetAttribute("z"));
                        DryCenterOfMass = new Vector3(x,y,z);
                        break;
                    case "Engines":
                        ReadEnginesXml(reader);
                        break;
                }
            }

            reader.Close();
        }

        private void ReadEnginesXml(XmlReader parentReader)
        {
            XmlReader reader = parentReader.ReadSubtree();
            if(reader.ReadToDescendant("Engine"))
            {
                do
                {
                    string engineID = reader.GetAttribute("id");
                    Engine engine = new Engine(engineID);
                    engine.ReadXml(reader);
                    engines.Add(engineID,engine);
                }
                while(reader.ReadToNextSibling("Engine"));
            }

            reader.Close();
        }
    }
}