using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Berth
{
    public Station Station { get; private set; }

    public string Name { get; private set; }

    public Vector3 Position { get; private set; }

    public Vector3 Orientation { get; private set; }

    public Vector3 Dimensions { get; private set; }

    public FlightNetwork.Node Node { get; private set; }

    public Berth(Station station)
    {
        this.Station = station;
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
                case "Name":
                    reader.Read();
                    Name = reader.ReadContentAsString();
                    break;
                case "Node":
                    string nodeID = reader.GetAttribute("id");
                    Node = new FlightNetwork.Node(Station.FlightNetwork, nodeID, Position);
                    break;
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
                case "Dimensions":
                    x = float.Parse(reader.GetAttribute("x"));
                    y = float.Parse(reader.GetAttribute("y"));
                    z = float.Parse(reader.GetAttribute("z"));
                    Dimensions = new Vector3(x,y,z);
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
