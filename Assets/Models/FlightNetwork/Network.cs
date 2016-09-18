using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace FlightNetwork
{
    public class Network : IXmlSerializable
    {
        private Dictionary<string,Node> nodes;
        private List<Node> approachNodes, departureNodes;
        private List<Edge> edges;

        public ICollection<Node> Nodes
        {
            get
            {
                return new List<Node>(nodes.Values);
            }
        }

        public ICollection<Edge> Edges
        {
            get
            {
                return new List<Edge>(edges);
            }
        }

        public Network()
        {
            nodes = new Dictionary<string,Node>();
            approachNodes = new List<Node>();
            departureNodes = new List<Node>();
            edges = new List<Edge>();
        }

        public Node GetNode(string id)
        {
            return nodes[id];
        }

        public void AddNode(Node node)
        {
            nodes.Add(node.ID, node);
        }

        #region IXmlSerializable implementation

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader parentReader)
        {
            XmlReader reader = parentReader.ReadSubtree();

            if(reader.ReadToDescendant("Nodes"))
            {
                ReadNodesXml(reader);
            }
            else
            {
                Debug.LogError("No Nodes tag found");
            }

            if(reader.ReadToNextSibling("Edges"))
            {
                ReadEdgesXml(reader);
            }
            else
            {
                Debug.LogError("No Edges tag found");
            }

            reader.Close();
        }

        public void WriteXml(XmlWriter writer)
        {
            
        }

        #endregion

        private void ReadNodesXml(XmlReader parentReader)
        {
            XmlReader reader = parentReader.ReadSubtree();
            if(reader.ReadToDescendant("Node"))
            {
                do
                {
                    Node node = new Node(this);
                    node.ReadXml(reader);
                    nodes.Add(node.ID,node);

                    bool isApproach, isDeparture;
                    if(bool.TryParse(reader.GetAttribute("approach"), out isApproach))
                    {
                        approachNodes.Add(node);
                    }

                    if(bool.TryParse(reader.GetAttribute("departure"), out isDeparture))
                    {
                        departureNodes.Add(node);
                    }
                }
                while(reader.ReadToNextSibling("Node"));
            }

            reader.Close();
        }

        private void ReadEdgesXml(XmlReader parentReader)
        {
            XmlReader reader = parentReader.ReadSubtree();
            if(reader.ReadToDescendant("Edge"))
            {
                do
                {
                    Edge edge = new Edge(this);
                    edge.ReadXml(reader);
                    edges.Add(edge);
                    edge.Start.AddNeighbour(edge.End, edge);
                    edge.End.AddNeighbour(edge.Start, edge);
                }
                while(reader.ReadToNextSibling("Edge"));
            }

            reader.Close();
        }
    }
}