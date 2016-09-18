using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class Station : IXmlSerializable
{
    public delegate void StationEventHandler(Station station);

    private List<Berth> berths;

    public string ID { get; private set; }

    public string Name { get; private set; }

    public Vector3d Position { get; private set; }

    public FlightNetwork.Network FlightNetwork { get; private set; }

    public Station()
    {
        berths = new List<Berth>();
        FlightNetwork = new FlightNetwork.Network();
    }

    public ICollection<Berth> Berths
    {
        get
        {
            return new List<Berth>(berths);
        }
    }

    #region IXmlSerializable implementation

    public System.Xml.Schema.XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader parentReader)
    {
        ID = parentReader.GetAttribute("id");
        XmlReader reader = parentReader.ReadSubtree();
        while(reader.Read())
        {
            switch(reader.Name)
            {
                case "Name":
                    reader.Read();
                    Name = reader.ReadContentAsString();
                    break;
                case "Orbit":
                    string bodyID = reader.GetAttribute("body");
                    double height = double.Parse(reader.GetAttribute("height"));
                    float inclination = float.Parse(reader.GetAttribute("inclination"));

                    CelestialBody body = World.Current.GetCelestialBody(bodyID);
                    this.Position = body.Position + new Vector3d(height, 0, 0);
                    break;
                case "Berths":
                    ReadBerthsXml(reader);
                    break;
                case "FlightNetwork":
                    FlightNetwork.ReadXml(reader);
                    break;
            }
        }

        reader.Close();
    }

    public void WriteXml(XmlWriter writer)
    {

    }

    #endregion

    private void ReadBerthsXml(XmlReader parentReader)
    {
        XmlReader reader = parentReader.ReadSubtree();
        if(reader.ReadToDescendant("Berth"))
        {
            do
            {
                Berth berth = new Berth(this);
                berth.ReadXml(reader);
                berths.Add(berth);
                FlightNetwork.AddNode(berth.Node);
            }
            while(reader.ReadToNextSibling("Berth"));
        }

        reader.Close();
    }
}