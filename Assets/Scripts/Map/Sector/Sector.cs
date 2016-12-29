using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [System.Serializable]
    public class Sector
    {
        private Vector2 _centeredPosition;
        private Rect _bounds;
        private static uint ChildrenNumber = 4;

        private uint _level;
        private Sector[] _children;

        public Sector(uint level, Rect bounds)
        {
            _bounds = bounds;

            _level = level;
            _children = null;

            ComputeCenteredPosition();
        }

        public void Divide(uint maxLevel)
        {
            if (_level < maxLevel)
            {
                _children = new Sector[ChildrenNumber];

                Rect[] childBounds = ComputeChildBounds();

                for (int i = 0; i < _children.Length; i++)
                {
                    _children[i] = new Sector(_level + 1, childBounds[i]);
                    _children[i].Divide(maxLevel);
                }
            }
        }

        private void ComputeCenteredPosition()
        {
            _centeredPosition.x = _bounds.x + _bounds.width/2;
            _centeredPosition.y = _bounds.y + _bounds.height/2;
        }

        private Rect[] ComputeChildBounds()
        {
            Rect[] childBounds = new Rect[ChildrenNumber];

            Vector2 dividedSize = _bounds.size/2;

            for (int i = 0; i < childBounds.Length; i++)
            {
                childBounds[i] = new Rect(_centeredPosition, dividedSize);
            }

            // right - bot
            // nothing to change

            // left - bot
            childBounds[1].x -= dividedSize.x;

            // left - top
            childBounds[2].x -= dividedSize.x;
            childBounds[2].y -= dividedSize.y;

            // right - top
            childBounds[3].y -= dividedSize.y;

            return childBounds;
        }

        public Sector[] GetChildren()
        {
            return _children;
        }

        public Rect GetBounds()
        {
            return _bounds;;
        }
    }
}