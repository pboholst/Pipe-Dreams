using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PipeDreams
{
    [System.Serializable]
    public class ItemPipeFunction
    {
        public Sprite Item;
        public short ItemID;
        public short DropChance = 1;
        public NumberOfRotations NumberOfRotation;
        public bool up;
        public bool right;
        public bool down;
        public bool left;
    }
    public enum NumberOfRotations
    {
        None = 0,
        Two = 2,
        Four = 4
    }
}
