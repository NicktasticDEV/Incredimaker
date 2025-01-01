using System.Collections.Generic;
using UnityEngine;


namespace Extrite
{
    [System.Serializable]
    public struct Animation
    {
        // Stuff in JSON
        public string name;
        public string prefix;
        public int fps;
        public bool loop;
        public Vector2 offset;
    }
}