using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// use for random generator
using System;


namespace MonsterAdventure
{
    /*
     * \brief   The class which is in charge of the random generation.
     *          It keep the seed of the generation, and allow to regenerate
     *          exactly the same number sequence.
     */
    public class RandomGenerator : MonoBehaviour
    {

        public int seed;
        public bool generateRandomSeed;

        private System.Random _pseudoRandom;

        /*!
        * \brief   TODO
        */
        void Awake()
        {

        }

        /*!
        * \brief   TODO
        */
        void Start()
        {

        }

        /*!
         * \brief   Construct the random generator.
         * \param   a_NewRandom if we want to regenerate
         *          the same sequence (false) or not (true).
         */
        public void Construct()
        {
            if (generateRandomSeed)
            {
                seed = GenerateRandomSeed();
            }

            _pseudoRandom = ConstructPseudoRandom(seed);

            print("Random number generator done with seed :" + seed);
        }

        public void Construct(int seed)
        {
            this.seed = seed;

            _pseudoRandom = ConstructPseudoRandom(seed);

            print("Random number generator done with seed : " + seed);
        }

        /*!
         * \brief Generate a new seed using the current time 
         * \return seed 
         */
        public static int GenerateRandomSeed()
        {
            DateTime currentTime = DateTime.Now;
            return currentTime.Ticks.ToString().GetHashCode();
        }

        /*!
         * \brief   Return a save of the random at this moment
         * \return  the save
         */
        public RandomState GetState()
        {
            RandomState randomState = new RandomState();
            randomState.Save(_pseudoRandom, seed);

            return randomState;
        }

        /*!
         * \brief   Restore a save of the random
         * \param   a_RandomState the random to restore
         */
        public void RestoreState(RandomState randomState)
        {
            _pseudoRandom = randomState.Restore();
        }

        /*!
         * \brief Construct the pseudo random generator
         * \param a_Seed Used to initialize the generator
         * \return Pseudo random generator constructed
         */
        private System.Random ConstructPseudoRandom(int seed)
        {
            return new System.Random(seed);
        }

        /*!
         * \brief Get a random value in the interval chosen
         * \param a_MinValue the boundary start of the interval
         * \param a_MinValue the boundary end of the interval
         * \return random value
         */
        public int Next(int minValue, int maxValue)
        {
            return _pseudoRandom.Next(minValue, maxValue);
        }

        public int Next(int maxValue)
        {
            return _pseudoRandom.Next(maxValue);
        }

        public int Next()
        {
            return _pseudoRandom.Next();
        }

        /*!
         * \brief   Get uniformly a random boolean
         * \return  True of False, with same probability (in theory)
         */
        public bool NextBool()
        {
            if (_pseudoRandom.Next(0, 2) == 1)
                return true;

            return false;
        }

        /*!
        * \brief    Get uniformly a random value between a min value 
         *          and a maximum value outside forbidden bounds.
         * \param   a_MinValue the smallest boundary
         * \param   a_MaxValue the greatest boundary
         * \param   a_ForbiddenBoundaries a list of all forbidden boundaries.
         *          It doesn't need to be sorted and can contains some
         *          interval collisions (which will be handle).
         * \return  the random value         
        */
        public int Next(int minValue, int maxValue, List<Boundary> forbiddenBoundaries)
        {
            SortBoundaryList(forbiddenBoundaries);
            ResolveCollisions(forbiddenBoundaries);

            List<Boundary> possibleBoundaries = GetPossibleBoundaries(minValue, maxValue,
                forbiddenBoundaries);

            return GetRandomValueIntoInterval(possibleBoundaries);
        }

        /*!
         * \brief   Get the bounds where possible values are into. Possible
         *          bounds are in the gobal bounds (the smallest value, 
         *          and the greatest value) and we get them from the forbidden boundaries
         *          (SORTED AND WITHOUT INTERVAL COLLISIONS).
         * \param   a_MinValue the smallest boundary
         * \param   a_MaxValue the greatest boundary
         * \param   a_ForbiddenBoundaries a list of all forbidden boundaries.
         *          (SORTED AND WITHOUT INTERVAL COLLISIONS)
         * \return  the possible boundaries
        */
        private List<Boundary> GetPossibleBoundaries(int minValue, int maxValue,
            List<Boundary> forbiddenBoundaries)
        {
            List<Boundary> possibleBoundaries = new List<Boundary>();

            int lastEnd = minValue - 1;

            foreach (Boundary forbiddenBoundary in forbiddenBoundaries)
            {
                if (forbiddenBoundary.min > maxValue)
                    break;

                int t_Width = forbiddenBoundary.min - lastEnd;

                if (t_Width > 0)
                    possibleBoundaries.Add(new Boundary(lastEnd + 1,
                        (uint)(forbiddenBoundary.min - lastEnd - 1)));

                lastEnd = forbiddenBoundary.max;

                if (lastEnd > maxValue)
                    break;
            }

            if (lastEnd < maxValue)
            {
                possibleBoundaries.Add(new Boundary(lastEnd + 1,
                    (uint)(maxValue - lastEnd - 1)));
            }

            return possibleBoundaries;
        }

        /*!
         * \brief   Get a value into bounds by a random uniform way.
         * \param   a_PossibleBoundaries the bounds where the value will be into.
         * \return  the random uniform value, or int.MaxValue if it fails
         */
        private int GetRandomValueIntoInterval(List<Boundary> possibleBoundaries)
        {
            int totalWidth = 0;

            foreach (Boundary possibleBoundary in possibleBoundaries)
            {
                totalWidth += possibleBoundary.width;
            }

            if (totalWidth == 0)
                return int.MinValue;

            int indexValue = Next(0, totalWidth);

            int indexJumping = 0; // the value of the first interval born
            int indexJoin = 0; // the value of the current index

            foreach (Boundary possibleBoundary in possibleBoundaries)
            {
                indexJumping = possibleBoundary.min;

                if (indexJoin + possibleBoundary.width > indexValue)
                {
                    int t_Offset = indexJumping - indexJoin;
                    return indexValue + t_Offset;
                }

                indexJoin += possibleBoundary.width;
            }

            return int.MinValue; // error value
        }

        /*!
         * \brief   Sort a boundary list, growing values with the 
         *          insertion sort.
         * \param   a_BoundaryList a boundary list which also be the
         *          final sorted list
         */
        public void SortBoundaryList(List<Boundary> boundaryList)
        {
            for (int i = 1; i < boundaryList.Count; i++)
            {
                Boundary x = boundaryList[i];
                int j = i;

                while (j > 0 && boundaryList[j - 1].min > x.min)
                {
                    boundaryList[j] = boundaryList[j - 1];
                    j = j - 1;
                }

                boundaryList[j] = x;
            }
        }

        /*!
         * \brief   Resolve interval collisions in a SORTED boundary list.
         * \param   a_BoundaryList a SORTED boundary list which will also
         *          be the resolved collisions list
         */
        public void ResolveCollisions(List<Boundary> boundaryList)
        {
            for (int i = 1; i < boundaryList.Count; i++)
            {
                if (boundaryList[i].min < boundaryList[i - 1].max)
                {
                    if (boundaryList[i].max <= boundaryList[i - 1].max)
                    {
                        // absorption
                        boundaryList.RemoveAt(i);
                    }
                    else
                    {
                        // fusion
                        Boundary boundary = new Boundary(boundaryList[i - 1].min,
                            (uint)((boundaryList[i].max) - boundaryList[i - 1].min + 1));

                        boundaryList.RemoveRange(i - 1, 2);
                        boundaryList.Insert(i - 1, boundary);
                    }

                    i--;
                }
            }
        }
    }
}