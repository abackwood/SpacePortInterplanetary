using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StationController : MonoBehaviour {
    public GameObject parentObject;

    private Dictionary<Station,GameObject> stationObjectMap;

	private void Start () {
        stationObjectMap = new Dictionary<Station,GameObject>();

        foreach (Station station in World.Current.Stations)
        {
            AddStationObject(station);
            UpdateStationPosition(station);
        }
	}

    private void AddStationObject(Station station)
    {
        GameObject station_go = BuildStationObject(station);
        station_go.transform.SetParent(parentObject.transform, false);

        stationObjectMap.Add(station, station_go);
    }

    private GameObject BuildStationObject(Station station)
    {
        GameObject station_go = new GameObject("Station - " + station.Name);

        foreach (Berth berth in station.Berths)
        {
            GameObject berth_go = BuildBerthObject(berth);
            berth_go.transform.SetParent(station_go.transform, false);
        }

        GameObject networkParentObject = new GameObject("Network");
        networkParentObject.transform.SetParent(station_go.transform);
        foreach (FlightNetwork.Edge edge in station.FlightNetwork.Edges)
        {
            GameObject edge_go = BuildEdgeObject(edge);
            edge_go.transform.SetParent(networkParentObject.transform);
        }

        return station_go;
    }

    private GameObject BuildBerthObject(Berth berth)
    {
        GameObject berth_go = new GameObject("Berth - " + berth.Name);
        berth_go.transform.localPosition = berth.Position / WorldController.METERS_PER_UNIT;
        berth_go.transform.eulerAngles = berth.Orientation;

        GameObject boxMarker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boxMarker.transform.SetParent(berth_go.transform, false);
        boxMarker.transform.localScale = berth.Dimensions / WorldController.METERS_PER_UNIT;

        boxMarker.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/BerthMaterial");

        return berth_go;
    }

    private GameObject BuildEdgeObject(FlightNetwork.Edge edge)
    {
        GameObject edge_go = new GameObject("Edge - " + edge.Name);

        LineRenderer lineRenderer = edge_go.AddComponent<LineRenderer>();
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, edge.Start.Position / WorldController.METERS_PER_UNIT);
        lineRenderer.SetPosition(1, edge.End.Position / WorldController.METERS_PER_UNIT);
        lineRenderer.SetWidth(0.001f, 0.001f);
        lineRenderer.material = Resources.Load<Material>("Materials/FlightEdgeMaterial");
        lineRenderer.useWorldSpace = false;

        return edge_go;
    }

    private void UpdateStationPosition(Station station)
    {
        GameObject station_go = stationObjectMap[station];

        station_go.transform.localPosition = station.Position / WorldController.METERS_PER_UNIT;
    }
}
