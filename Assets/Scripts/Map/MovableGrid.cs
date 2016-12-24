using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterAdventure
{
    public enum Direction
    {
        NEGATIVE = -1,
        NONE = 0,
        POSITIVE = 1
    }

    public class MovableGrid : MonoBehaviour
    {
        public Text text;
        public ChunkManager chunkManager;
         
        private Chunk[,] _chunks;

        private void Awake()
        {
            _chunks = new Chunk[3, 3];
        }

        private void Update()
        {
            // todo : rotate the grid when the player move
            // desynchroniser la position du player (coords) avec le centre de la grille

            text.text = "Coords (" + _chunks[1, 1].GetX() + ", " + _chunks[1, 1].GetY() + ")";
        }

        public void SetPosition(int centerCoords_x, int centerCoords_y)
        {
            // put the coords to the bot left (make it easier to manipulate)
            centerCoords_x--;
            centerCoords_y--;

            Vector2 currentCoords = new Vector2();

            for (Int32 i = 0; i < _chunks.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _chunks.GetLength(1); j++)
                {
                    currentCoords.x = centerCoords_x + i;
                    currentCoords.y = centerCoords_y + j;

                    _chunks[i, j] = GetNextChunk(i, j);
                }
            }
        }


        public void Move(Direction abs, Direction ord)
        {
            // everything is hard coded to improve the perfomance

            if (abs == Direction.POSITIVE)
            {
                // the coords of the top-left corner of the grid
                Int32 newLastCoord_x = _chunks[2, 0].GetX() + 1;
                Int32 newLastCoord_y = _chunks[2, 0].GetY();

                // We rotate the grid
                _chunks[0, 0].SetActive(false);
                _chunks[0, 0] = _chunks[1, 0];
                _chunks[1, 0] = _chunks[2, 0];

                // Set the coords for the new Coords (Only the last line or column)
                _chunks[2, 0] = GetNextChunk(newLastCoord_x, newLastCoord_y);

                // repeat this algo for each line or culumn (depending if we check for the abs or the ord)
                // ...

                _chunks[0, 1].SetActive(false);
                _chunks[0, 1] = _chunks[1, 1];
                _chunks[1, 1] = _chunks[2, 1];
                _chunks[2, 1] = GetNextChunk(newLastCoord_x, newLastCoord_y + 1);

                _chunks[0, 2].SetActive(false);
                _chunks[0, 2] = _chunks[1, 2];
                _chunks[1, 2] = _chunks[2, 2];
                _chunks[2, 2] = GetNextChunk(newLastCoord_x, newLastCoord_y + 2);
            }

            else if (abs == Direction.NEGATIVE)
            {
                Int32 newFirstCoord_x = _chunks[0, 0].GetX() - 1;
                Int32 newFirstCoord_y = _chunks[0, 0].GetY();

                _chunks[2, 0].SetActive(false);
                _chunks[2, 0] = _chunks[1, 0];
                _chunks[1, 0] = _chunks[0, 0];
                _chunks[0, 0] = GetNextChunk(newFirstCoord_x, newFirstCoord_y);

                _chunks[2, 1].SetActive(false);
                _chunks[2, 1] = _chunks[1, 1];
                _chunks[1, 1] = _chunks[0, 1];
                _chunks[0, 1] = GetNextChunk(newFirstCoord_x, newFirstCoord_y + 1);

                _chunks[2, 2].SetActive(false);
                _chunks[2, 2] = _chunks[1, 2];
                _chunks[1, 2] = _chunks[0, 2];
                _chunks[0, 2] = GetNextChunk(newFirstCoord_x, newFirstCoord_y + 2);
            }

            if (ord == Direction.NEGATIVE)
            {
                Int32 newLastCoord_x = _chunks[0, 0].GetX();
                Int32 newLastCoord_y = _chunks[0, 0].GetY() - 1;

                _chunks[0, 2].SetActive(false);
                _chunks[0, 2] = _chunks[0, 1];
                _chunks[0, 1] = _chunks[0, 0];
                _chunks[0, 0] = GetNextChunk(newLastCoord_x, newLastCoord_y);

                _chunks[1, 2].SetActive(false);
                _chunks[1, 2] = _chunks[1, 1];
                _chunks[1, 1] = _chunks[1, 0];
                _chunks[1, 0] = GetNextChunk(newLastCoord_x + 1, newLastCoord_y);

                _chunks[2, 2].SetActive(false);
                _chunks[2, 2] = _chunks[2, 1];
                _chunks[2, 1] = _chunks[2, 0];
                _chunks[2, 0] = GetNextChunk(newLastCoord_x + 2, newLastCoord_y);
            }

            else if (ord == Direction.POSITIVE)
            {

                Int32 newFirstCoord_x = _chunks[0, 2].GetX();
                Int32 newFirstCoord_y = _chunks[0, 2].GetY() + 1;

                _chunks[0, 0].SetActive(false);
                _chunks[0, 0] = _chunks[0, 1];
                _chunks[0, 1] = _chunks[0, 2];
                _chunks[0, 2] = GetNextChunk(newFirstCoord_x, newFirstCoord_y);

                _chunks[1, 0].SetActive(false);
                _chunks[1, 0] = _chunks[1, 1];
                _chunks[1, 1] = _chunks[1, 2];
                _chunks[1, 2] = GetNextChunk(newFirstCoord_x + 1, newFirstCoord_y);

                _chunks[2, 0].SetActive(false);
                _chunks[2, 0] = _chunks[2, 1];
                _chunks[2, 1] = _chunks[2, 2];
                _chunks[2, 2] = GetNextChunk(newFirstCoord_x + 2, newFirstCoord_y);
            }
        }

        public List<Chunk> GetNeighbours(UInt32 x, UInt32 y)
        {
            Int32 abs = (Int32)x;
            Int32 ord = (Int32)y;

            List<Chunk> neighbours = new List<Chunk>();

            for (Int32 i = abs - 1; i <= abs + 1; i++)
            {
                // we skip if the case is outside the grid
                if (i < 0 || i > 2)
                {
                    continue;
                }

                for (Int32 j = ord - 1; j <= ord + 1; j++)
                {
                    // the same : we skip if the case is outside the grid
                    if (j < 0 || j > 2)
                    {
                        continue;
                    }

                    // if we reach this instruction, that mean that the chunk position is possible
                    // according to the bounds of the grid. Then, we can add it
                    neighbours.Add(_chunks[i, j]);
                }
            }

            return neighbours;
        }

        public Chunk Get(UInt32 abs, UInt32 ord)
        {
            return _chunks[abs, ord];
        }

        public void Set(UInt32 abs, UInt32 ord, Chunk chunk)
        {
            _chunks[abs, ord] = chunk;
        }

        public Int32 GetLength(Int32 index)
        {
            return _chunks.GetLength(index);
        }

        private Chunk GetNextChunk(int x, int y)
        {
            if (x < 0 || y < 0)
                return null;

            Chunk newChunk = chunkManager.Get(x, y);

            newChunk.SetActive(true);

            return newChunk;
        }
    }
}
