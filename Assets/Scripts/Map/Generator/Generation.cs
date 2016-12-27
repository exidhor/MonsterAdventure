using UnityEngine;
using System.Collections.Generic;

namespace MonsterAdventure.Generator
{
    [RequireComponent(typeof(Rules))]
    public class Generation : MonoBehaviour
    {
        public GameObject generatedObject;
        public uint numberOfGeneratedObject;
        public GenerationType generationType;

        private Rules _rules;

        private void Awake()
        {
            _rules = GetComponent<Rules>();
        }

        // Use this for initialization
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}