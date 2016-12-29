using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer _spriteRender;

        private void Awake()
        {
            _spriteRender = GetComponent<SpriteRenderer>();
        }

        public void SetSprite(Sprite sprite)
        {
            _spriteRender.color = new Color(0, 0, 0, 0);
            //_spriteRender.sprite = sprite;
        }
    }
}
