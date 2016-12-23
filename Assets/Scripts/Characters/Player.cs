using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Player : MovingObjects
    {
        protected override void Start()
        {
            base.Start();

        }

        protected override void AttemptMove<T>(int xDir, int yDir)
        {
            // when player move
            base.AttemptMove<T>(xDir, yDir);
        }

        private void FixedUpdate()
        {
            int horizontal = 0;
            int vertical = 0;

            horizontal = (int) Input.GetAxisRaw("Horizontal");
            vertical = (int) Input.GetAxisRaw("Vertical");

            if (horizontal != 0)
            {
                vertical = 0;
            }

            if(horizontal != 0 || vertical != 0)
            {
                RaycastHit2D hit;
                Move(horizontal, vertical, out hit);
            }
        }

        protected override void OnCantMove<T>(T component)
        {
            //Wall hitWall = component as Wall;
        }
    }
}
