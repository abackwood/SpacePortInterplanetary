using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class CelestialBody : IXmlSerializable
{
    public static readonly double M_EARTH = 5.972e24;
    public static readonly double M_JUPITER = 1.898e27;
    public static readonly double M_SUN = 1.989e30;

    public string ID { get; private set; }

    public string Name { get; private set; }

    public Vector3d Position { get; private set; }

    public double Diameter { get; private set; }

    public double Mass { get; private set; }

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
                case "Position":
                    double x = double.Parse(reader.GetAttribute("x"));
                    double y = double.Parse(reader.GetAttribute("y"));
                    double z = double.Parse(reader.GetAttribute("z"));
                    Position = new Vector3d(x,y,z);
                    break;
                case "Diameter":
                    reader.Read();
                    Diameter = reader.ReadContentAsDouble();
                    break;
                case "Mass":
                    reader.Read();
                    Mass = reader.ReadContentAsDouble();
                    break;
            }
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        throw new System.NotImplementedException();
    }

    #endregion
}