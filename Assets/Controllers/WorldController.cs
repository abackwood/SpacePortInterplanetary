using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {
    public static WorldController Instance { get; private set; }

    public World World { get; private set; }

	private void Awake () {
        if (Instance != null)
        {
            Debug.LogError("Trying to make two WorldController objects");
        }
        Instance = this;

        World = new World();
	}
	
	private void Update () {
	    
	}
}
