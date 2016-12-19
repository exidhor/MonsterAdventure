using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Fractal : MonoBehaviour
    {
        public int maxDepth;
        public float spawnProbability;
        public float maxTwist;

        public Mesh mesh;
        public Material material;
        public float childScale;

        private int depth;

        private void Start()
        {
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
            gameObject.AddComponent<MeshRenderer>().material = material;

            GetComponent<MeshRenderer>().material.color =
            Color.Lerp(Color.white, Color.yellow, (float)depth / maxDepth);

            transform.Rotate(UnityEngine.Random.Range(-maxTwist, maxTwist), 0f, 0f);

            if (depth < maxDepth)
            {
                StartCoroutine(CreateChildren());
            }

        }

        private void Initialize(Fractal parent, Vector3 direction)
        {
            mesh = parent.mesh;
            material = parent.material;
            maxDepth = parent.maxDepth;
            depth = parent.depth + 1;
            childScale = parent.childScale; 
            transform.parent = parent.transform;
            transform.localScale = Vector3.one*childScale;
            transform.localPosition = direction * (0.5f + 0.5f * childScale);
        }

        private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

        private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

        private IEnumerator CreateChildren()
        {
            for (int i = 0; i < childDirections.Length; i++)
            {
                if (UnityEngine.Random.value < spawnProbability)
                {
                    yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.5f));
                    new GameObject("Fractal Child").AddComponent<Fractal>().
                    Initialize(this, i);
                }
            }
        }

        private void Initialize(Fractal parent, int childIndex)
        {
            mesh = parent.mesh;
            material = parent.material;
            maxDepth = parent.maxDepth;
            depth = parent.depth + 1;
            childScale = parent.childScale;
            transform.parent = parent.transform;
            transform.localScale = Vector3.one * childScale;
            spawnProbability = parent.spawnProbability;
            maxTwist = parent.maxTwist;

            transform.localPosition =
            childDirections[childIndex] * (0.5f + 0.5f * childScale);
            transform.localRotation = childOrientations[childIndex];
        }

        private void Update()
        {
            transform.Rotate(0f, 30f * Time.deltaTime, 0f);
        }
    }
}
