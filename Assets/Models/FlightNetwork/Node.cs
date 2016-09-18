using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace FlightNetwork
{
    public class Node : IXmlSerializable
    {
        private List<Edge> edges;
        private List<Node> neighbours;
        private Dictionary<Node,Edge> neighbourEdgeMap;

        public Network Network { get; private set; }

        public string ID { get; private set; }

        public Vector3 Position { get; private set; }

        public ICollection<Edge> Edges
        {
            get
            {
                return new List<Edge>(edges);
            }
        }

        public ICollection<Node> Neighbours
        {
            get
            {
                return new List<Node>(neighbours);
            }
        }

        public Node(Network network)
        {
            this.Network = network;
            edges = new List<Edge>();
            neighbours = new List<Node>();
            neighbourEdgeMap = new Dictionary<Node,Edge>();
        }

        public Node(Network network, string id, Vector3 position)
        {
            this.Network = network;
            this.ID = id;
            this.Position = position;
            edges = new List<Edge>();
            neighbours = new List<Node>();
            neighbourEdgeMap = new Dictionary<Node,Edge>();
        }

        public Edge GetEdgeToNeighbour(Node neighbour)
        {
            return neighbourEdgeMap[neighbour];
        }

        public void AddNeighbour(Node neighbour, Edge edge)
        {
            neighbours.Add(neighbour);
            edges.Add(edge);
            neighbourEdgeMap.Add(neighbour, edge);
        }

        #region IXmlSerializable implementation

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            ID = reader.GetAttribute("id");
            float x = float.Parse(reader.GetAttribute("x"));
            float y = float.Parse(reader.GetAttribute("y"));
            float z = float.Parse(reader.GetAttribute("z"));
            Position = new Vector3(x, y, z);
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}