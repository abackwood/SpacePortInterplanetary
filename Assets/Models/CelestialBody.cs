using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class CelestialBody : IXmlSerializable
{
    public static readonly float M_EARTH = 5.972e24f;
    public static readonly float M_JUPITER = 1.898e27f;
    public static readonly float M_SUN = 1.989e30f;

    public string ID { get; private set; }

    public Vector3 Position { get; private set; }

    public float Diameter { get; private set; }

    public float Mass { get; private set; }

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
                case "Position":
                    float x = float.Parse(reader.GetAttribute("x"));
                    float y = float.Parse(reader.GetAttribute("y"));
                    float z = float.Parse(reader.GetAttribute("z"));
                    Position = new Vector3(x,y,z);
                    break;
                case "Diameter":
                    reader.Read();
                    Diameter = reader.ReadContentAsFloat();
                    break;
                case "Mass":
                    reader.Read();
                    Mass = reader.ReadContentAsFloat();
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