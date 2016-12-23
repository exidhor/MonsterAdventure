using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public abstract class MovingObjects : MonoBehaviour
    {
        public float moveTime = 0.1f;
        public LayerMask blockingLayer;

        private BoxCollider2D boxCollider;
        private Rigidbody2D rb2D;
        private float inverseMoveTime;

        protected virtual void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            rb2D = GetComponent<Rigidbody2D>();
            inverseMoveTime = 1f/moveTime;
        }

        protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
        {
            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(xDir, yDir);

            // to be sure that we dont hit our collider
            boxCollider.enabled = false;

            hit = Physics2D.Linecast(start, end, blockingLayer);

            boxCollider.enabled = true;

            if (hit.transform == null)
            {
                SmoothMovement(end);
                return true;
            }

            return false;
        }

        protected void SmoothMovement(Vector3 end)
        {
            float sqrtRemainingDistance = (transform.position - end).sqrMagnitude;

            if (sqrtRemainingDistance > float.Epsilon)
            {
                Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime*Time.deltaTime);
                rb2D.MovePosition(newPosition);
                sqrtRemainingDistance = (transform.position - end).sqrMagnitude;
            }
        }

        protected virtual void AttemptMove<T>(int xDir, int yDir)
            where T : Component
        {
            RaycastHit2D hit;
            bool canMove = Move(xDir, yDir, out hit);

            if (hit.transform == null) // nothing hit
            {
                return;
            }

            T hitComponent = hit.transform.GetComponent<T>();

            if (!canMove && hitComponent != null)
            {
                OnCantMove(hitComponent);
            }
        }

        protected abstract void OnCantMove<T>(T component)
            where T : Component;
    }
}
