using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class RemarkablePlace : MonoBehaviour
    {
        public Chunk _container;

        public void SetChunk(Chunk chunk)
        {
            _container = chunk;
        }
    }
}
