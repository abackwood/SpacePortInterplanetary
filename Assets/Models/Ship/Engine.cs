using System.Collections.Generic;
using UnityEngine;

namespace Ship
{
    public class Engine
    {
        public string ID { get; private set; }

        public Vector3 Position { get; private set; }

        public Vector3 Direction { get; private set; }

        public float MaxThrust { get; private set; }

        public float Power { get; set; }
    }
}