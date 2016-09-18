using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class World
{
    public static readonly string BODIES_DIR = "Bodies";
    public static readonly string STATIONS_DIR = "Stations";

    public static World Current { get; private set; }

    private Dictionary<string,CelestialBody> celestialBodies;
    private Dictionary<string,Station> stations;

    public ICollection<CelestialBody> Bodies
    {
        get
        {
            return new List<CelestialBody>(celestialBodies.Values);
        }
    }

    public ICollection<Station> Stations
    {
        get
        {
            return new List<Station>(stations.Values);
        }
    }

    public World()
    {
        if (Current != null)
        {
            Debug.LogError("Trying to create two world");
        }
        Current = this;

        celestialBodies = new Dictionary<string,CelestialBody>();
        stations = new Dictionary<string,Station>();

        LoadWorld();
    }

    public CelestialBody GetCelestialBody(string id)
    {
        if (celestialBodies.ContainsKey(id) == false)
        {
            Debug.LogError("GetCelestialBody -- No body registered to this ID: " + id);
            return null;
        }

        return celestialBodies[id];
    }

    public Station GetStation(string id)
    {
        if (stations.ContainsKey(id) == false)
        {
            Debug.LogError("GetStation -- No station registered to this ID: " + id);
            return null;
        }

        return stations[id];
    }

    private void LoadWorld()
    {
        LoadCelestialBodies();
        LoadStations();

        Debug.Log("Bodies loaded: " + celestialBodies.Count);
        Debug.Log("Stations loaded: " + stations.Count);
    }

    private void LoadCelestialBodies()
    {
        string bodiesPath = Path.Combine(Application.streamingAssetsPath, BODIES_DIR);
        string[] files = Directory.GetFiles(bodiesPath);
        foreach (string file in files)
        {
            if (Path.GetExtension(file) == ".xml")
            {
                XmlReader reader = XmlReader.Create(file);

                if (reader.ReadToDescendant("Bodies"))
                {
                    ReadBodiesXml(reader);
                }
                else
                {
                    Debug.LogError("LoadCelestialBodies -- Bodies tag not found: " + reader.Name);
                }
            }
        }
    }

    private void ReadBodiesXml(XmlReader parentReader)
    {
        XmlReader reader = parentReader.ReadSubtree();
        if (reader.ReadToDescendant("Body"))
        {
            do
            {
                CelestialBody body = new CelestialBody();
                body.ReadXml(reader);
                celestialBodies.Add(body.ID, body);
            }
            while(reader.ReadToNextSibling("Body"));
        }
        else
        {
            Debug.LogError("ReadBodiesXml -- Body tag not found: " + reader.Name);
        }
    }

    private void LoadStations()
    {
        string stationsPath = Path.Combine(Application.streamingAssetsPath, STATIONS_DIR);
        string[] files = Directory.GetFiles(stationsPath);
        foreach (string file in files)
        {
            if (Path.GetExtension(file) == ".xml")
            {
                XmlReader reader = XmlReader.Create(file);

                if (reader.ReadToDescendant("Stations"))
                {
                    ReadStationsXml(reader);
                }
                else
                {
                    Debug.LogError("LoadStations -- Stations tag not found: " + reader.Name);
                }
            }
        }
    }

    private void ReadStationsXml(XmlReader parentReader)
    {
        XmlReader reader = parentReader.ReadSubtree();
        if (reader.ReadToDescendant("Station"))
        {
            do
            {
                Station station = new Station();
                station.ReadXml(reader);
                stations.Add(station.ID, station);
            }
            while(reader.ReadToNextSibling("Station"));
        }
        else
        {
            Debug.LogError("ReadStationsXml -- Station tag not found: " + reader.Name);
        }
    }
}