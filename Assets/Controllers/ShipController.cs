using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SPI.Ship;

public class ShipController : MonoBehaviour {
    public GameObject parentObject;
    public GameObject enginePrefab;

    private Dictionary<string,Mesh> meshes;
    private Dictionary<Ship,GameObject> shipObjectMap;

    public Mesh GetMeshForType(string type)
    {
        return meshes.ContainsKey(type) ? meshes[type] : null;
    }

	private void Start () {
        meshes = new Dictionary<string,Mesh>();
        shipObjectMap = new Dictionary<Ship,GameObject>();

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
        GameObject ship_go = BuildShipObject(ship);
        ship_go.transform.SetParent(parentObject.transform, false);
        shipObjectMap.Add(ship, ship_go);

        ship.ShipChanged += OnShipChanged;
    }

    private void OnShipChanged(Ship ship)
    {
        GameObject ship_go = shipObjectMap[ship];

        ship_go.transform.localPosition = ship.Position / WorldController.METERS_PER_UNIT;
        ship_go.transform.eulerAngles = ship.Direction;
    }

    private GameObject BuildShipObject(Ship ship)
    {
        GameObject ship_go = new GameObject("Ship");

        GameObject model_go = GameObject.Instantiate(Resources.Load<GameObject>("Models/Ships/" + ship.Type));
        model_go.transform.SetParent(ship_go.transform, false);
        model_go.transform.localScale = Vector3.one / WorldController.METERS_PER_UNIT;

        //MeshFilter meshFilter = model_go.AddComponent<MeshFilter>();
        //meshFilter.mesh = GetMeshForType(ship.Type);

        //MeshRenderer meshRenderer = model_go.AddComponent<MeshRenderer>();
        //meshRenderer.material = Resources.Load<Material>("Materials/ShipMaterial");

        foreach (Engine engine in ship.Engines)
        {
            GameObject engine_go = BuildEngineObject(ship, engine);
            engine_go.transform.SetParent(ship_go.transform, false);
        }

        return ship_go;
    }

    private GameObject BuildEngineObject(Ship ship, Engine engine)
    {
        GameObject engine_go = GameObject.Instantiate(enginePrefab);
        engine_go.name = "Engine - " + engine.ID;
        engine_go.transform.localPosition = engine.Position / WorldController.METERS_PER_UNIT;
        engine_go.transform.LookAt(engine_go.transform.position + engine.Orientation);

        ParticleSystem particleSystem = engine_go.GetComponent<ParticleSystem>();
        particleSystem.startSize = 1f / WorldController.METERS_PER_UNIT;
        particleSystem.maxParticles = 100;

        ParticleSystem.VelocityOverLifetimeModule volModule = particleSystem.velocityOverLifetime;
        volModule.x = new ParticleSystem.MinMaxCurve(0,0);
        volModule.y = new ParticleSystem.MinMaxCurve(0,0);
        volModule.z = new ParticleSystem.MinMaxCurve(2.5f, 5f);    // Should depend on engine power

        ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
        shapeModule.shapeType = ParticleSystemShapeType.Cone;
        shapeModule.radius = engine.Diameter / 2;
        shapeModule.angle = 0;

        return engine_go;
    }
}
