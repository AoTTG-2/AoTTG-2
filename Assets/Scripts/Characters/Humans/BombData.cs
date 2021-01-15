using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Characters.Humans
{
    [System.Serializable]
    public class BombData
    {
        public float coolDown;
        public bool immune;
        public float radius;
        public float speed;
        public float time;
        public float timeMax;
    }
}
