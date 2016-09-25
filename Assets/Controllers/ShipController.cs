using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPI.Ship;

public class ShipController : MonoBehaviour {
    public GameObject parentObject;
    public GameObject enginePrefab;

    private Dictionary<string,Mesh> meshes;
    private Dictionary<Ship,ShipInfo> shipObjectMap;

    public Mesh GetMeshForType(string type)
    {
        return meshes.ContainsKey(type) ? meshes[type] : null;
    }

	private void Start () {
        meshes = new Dictionary<string,Mesh>();
        shipObjectMap = new Dictionary<Ship,ShipInfo>();

        LoadMeshes();

        foreach (Ship ship in World.Current.Ships)
        {
            OnShipCreated(ship);
            OnShipChanged(ship);
        }

        World.Current.ShipCreated += OnShipCreated;
	}

    private void LoadMeshes()
    {
        Mesh[] meshArray = Resources.LoadAll<Mesh>("Models/Ships");
        foreach (Mesh mesh in meshArray)
        {
            meshes.Add(mesh.name, mesh);
            Debug.Log("Loaded mesh: " + mesh.name);
        }
    }

    private void OnShipCreated(Ship ship)
    {
        ShipInfo shipInfo = BuildShipObject(ship);
        shipInfo.ShipGO.transform.SetParent(parentObject.transform, false);
        shipObjectMap.Add(ship, shipInfo);

        ship.ShipChanged += OnShipChanged;
    }

    private void OnShipChanged(Ship ship)
    {
        GameObject ship_go = shipObjectMap[ship].ShipGO;

        ship_go.transform.localPosition = ship.Position / WorldController.METERS_PER_UNIT;
        ship_go.transform.eulerAngles = ship.Direction;
    }

    private void OnEngineChanged(Engine engine)
    {
        GameObject engine_go = shipObjectMap[engine.Ship].EngineMap[engine.ID];

        ParticleSystem ps = engine_go.GetComponent<ParticleSystem>();
        ps.maxParticles = Mathf.FloorToInt(1000 * engine.Power);
    }

    private ShipInfo BuildShipObject(Ship ship)
    {
        ShipInfo shipInfo = new ShipInfo(new GameObject("Ship"));

        GameObject model_go = GameObject.Instantiate(Resources.Load<GameObject>("Models/Ships/" + ship.Type));
        model_go.transform.SetParent(shipInfo.ShipGO.transform, false);
        model_go.transform.localScale = Vector3.one / WorldController.METERS_PER_UNIT;

        foreach (Engine engine in ship.Engines)
        {
            GameObject engine_go = BuildEngineObject(ship, engine);
            engine_go.transform.SetParent(shipInfo.ShipGO.transform, false);
            shipInfo.EngineMap.Add(engine.ID, engine_go);

            engine.EngineChanged += OnEngineChanged;
        }

        return shipInfo;
    }

    private GameObject BuildEngineObject(Ship ship, Engine engine)
    {
        GameObject engine_go = GameObject.Instantiate(enginePrefab);
        engine_go.name = "Engine - " + engine.ID;
        engine_go.transform.localPosition = engine.Position / WorldController.METERS_PER_UNIT;
        engine_go.transform.LookAt(engine_go.transform.position + engine.Orientation);

        ParticleSystem particleSystem = engine_go.GetComponent<ParticleSystem>();
        particleSystem.startSize = 1f / WorldController.METERS_PER_UNIT;
        particleSystem.maxParticles = 0;

        ParticleSystem.VelocityOverLifetimeModule volModule = particleSystem.velocityOverLifetime;
        volModule.x = new ParticleSystem.MinMaxCurve(0,0);
        volModule.y = new ParticleSystem.MinMaxCurve(0,0);
        volModule.z = new ParticleSystem.MinMaxCurve(1f, 5f);

        ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Cone;
        shapeModule.radius = engine.Diameter / 2;
        shapeModule.angle = 0;

        return engine_go;
    }

    private struct ShipInfo
    {
        public GameObject ShipGO { get; private set; }

        public Dictionary<string,GameObject> EngineMap { get; private set; }

        public ShipInfo(GameObject ship_go) : this()
        {
            this.ShipGO = ship_go;
            EngineMap = new Dictionary<string,GameObject>();
        }
    }
}
