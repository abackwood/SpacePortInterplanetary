using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace FlightNetwork
{
    public class Edge : IXmlSerializable
    {
        public Network Network { get; private set; }

        public string Type { get; private set; }

        public string Name { get; private set; }

        public Node Start { get; private set; }

        public Node End { get; private set; }

        public float StartSpeed { get; private set; }

        public float EndSpeed { get; private set; }

        public float Diameter { get; private set; }

        public Edge(Network network)
        {
            this.Network = network;
        }

        #region IXmlSerializable implementation

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader parentReader)
        {
            Type = parentReader.GetAttribute("type");
            XmlReader reader = parentReader.ReadSubtree();
            while(reader.Read())
            {
                switch(reader.Name)
                {
                    case "Name":
                        reader.Read();
                        Name = reader.ReadContentAsString();
                        break;
                    case "Start":
                        string startID = reader.GetAttribute("id");
                        Start = Network.GetNode(startID);
                        StartSpeed = float.Parse(reader.GetAttribute("maxSpeed"));
                        break;
                    case "End":
                        string endID = reader.GetAttribute("id");
                        End = Network.GetNode(endID);
                        EndSpeed = float.Parse(reader.GetAttribute("maxSpeed"));
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