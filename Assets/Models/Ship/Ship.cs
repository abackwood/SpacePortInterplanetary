using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class Ship
    {
        private Dictionary<string,Engine> engines;

        public string ID { get; private set; }

        public Vector3d Position { get; set; }

        public float Mass { get; set; }

        public Vector3 CenterOfMass { get; set; }
    }
}