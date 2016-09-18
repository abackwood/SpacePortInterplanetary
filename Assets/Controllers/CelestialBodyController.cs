using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CelestialBodyController : MonoBehaviour
{
    public GameObject parentObject;

    private Dictionary<CelestialBody,GameObject> bodyObjectMap;

    private void Start()
    {
        bodyObjectMap = new Dictionary<CelestialBody,GameObject>();

        foreach (CelestialBody body in World.Current.Bodies)
        {
            AddBodyObject(body);
            UpdateBodyPosition(body);
        }
    }

    private void AddBodyObject(CelestialBody body)
    {
        GameObject body_go = BuildBodyObject(body);
        body_go.transform.SetParent(parentObject.transform, false);

        bodyObjectMap.Add(body, body_go);
    }

    private GameObject BuildBodyObject(CelestialBody body)
    {
        GameObject body_go = new GameObject("Body - " + body.Name);

        GameObject model_go = BuildModelObject(body);
        model_go.transform.SetParent(body_go.transform, false);

        return body_go;
    }

    private GameObject BuildModelObject(CelestialBody body)
    {
        GameObject model_go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        model_go.name = "Model";

        model_go.transform.localScale = (body.Diameter / WorldController.METERS_PER_UNIT) * Vector3d.one;

        model_go.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/PlanetMaterial");

        return model_go;
    }

    private void UpdateBodyPosition(CelestialBody body)
    {
        GameObject body_go = bodyObjectMap[body];

        body_go.transform.localPosition = body.Position / WorldController.METERS_PER_UNIT;
    }
}
