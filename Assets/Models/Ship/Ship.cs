using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Ship
{
    public class Ship : IXmlSerializable
    {
        private Dictionary<string,Engine> engines;

        public string ID { get; private set; }

        public string Name { get; private set; }

        public Vector3d Position { get; set; }

        public float DryMass { get; set; }

        public Vector3 CenterOfMass { get; set; }

        public Ship(string id)
        {
            this.ID = id;
            engines = new Dictionary<string,Engine>();
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
                        CenterOfMass = new Vector3(x,y,z);
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