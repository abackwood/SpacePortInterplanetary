using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using SPI.Ship;

public class World
{
    public static readonly string BODIES_DIR = "Bodies";
    public static readonly string STATIONS_DIR = "Stations";
    public static readonly string SHIPS_DIR = "Ships";

    public static World Current { get; private set; }

    public event Ship.ShipEventHandler ShipCreated;

    private Dictionary<string,Ship> shipPrototypes;

    private Dictionary<string,CelestialBody> celestialBodies;
    private Dictionary<string,Station> stations;
    private List<Ship> ships;

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

    public ICollection<Ship> Ships
    {
        get
        {
            return new List<Ship>(ships);
        }
    }

    public World()
    {
        if (Current != null)
        {
            Debug.LogError("Trying to create two world");
        }
        Current = this;

        shipPrototypes = new Dictionary<string,Ship>();

        celestialBodies = new Dictionary<string,CelestialBody>();
        stations = new Dictionary<string,Station>();
        ships = new List<Ship>();

        LoadWorld();

        CreateShip("planetary_shuttle", new Vector3d(0, 0, -60000));
    }

    #region getters
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

    public Ship GetShipPrototype(string type)
    {
        if (shipPrototypes.ContainsKey(type) == false)
        {
            Debug.LogError("GetShipPrototype -- No prototype for: " + type);
            return null;
        }

        return shipPrototypes[type];
    }
    #endregion

    public Ship CreateShip(string type, Vector3d position)
    {
        Ship ship = new Ship(GetShipPrototype(type), position);

        if (ShipCreated != null)
        {
            ShipCreated(ship);
        }

        ships.Add(ship);

        return ship;
    }

    #region loading world
    private void LoadWorld()
    {
        LoadCelestialBodies();
        LoadStations();
        LoadShipPrototypes();

        Debug.Log("Bodies loaded: " + celestialBodies.Count);
        Debug.Log("Stations loaded: " + stations.Count);
        Debug.Log("Ship prototypes loaded: " + shipPrototypes.Count);
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

    private void LoadShipPrototypes()
    {
        string shipsPath = Path.Combine(Application.streamingAssetsPath, SHIPS_DIR);
        string[] files = Directory.GetFiles(shipsPath);
        foreach (string file in files)
        {
            if (Path.GetExtension(file) == ".xml")
            {
                XmlReader reader = XmlReader.Create(file);

                if (reader.ReadToDescendant("Ships"))
                {
                    ReadShipsXml(reader);
                }
                else
                {
                    Debug.LogError("LoadShipPrototypes -- Ships tag not found: " + reader.Name);
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

    private void ReadShipsXml(XmlReader parentReader)
    {
        XmlReader reader = parentReader.ReadSubtree();
        if (reader.ReadToDescendant("Ship"))
        {
            do
            {
                string shipType = reader.GetAttribute("type");
                Ship prototype = new Ship(shipType);
                prototype.ReadPrototypeXml(reader);
                shipPrototypes.Add(shipType, prototype);
            }
            while(reader.ReadToNextSibling("Ship"));
        }
        else
        {
            Debug.LogError("ReadShipsXml -- Ship tag not found: " + reader.Name);
        }
    }
    #endregion
}