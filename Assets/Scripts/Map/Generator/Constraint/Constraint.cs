using UnityEngine;
using System;
using System.Collections;

namespace MonsterAdventure.Generator
{
    [System.Serializable]
    public abstract class Constraint : MonoBehaviour
    {
        public int testtt;
        public abstract void applyConstraint();
    }
}