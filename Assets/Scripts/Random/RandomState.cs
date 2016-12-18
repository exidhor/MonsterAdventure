using System;
using System.Collections.Generic;

// use for state save
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonsterAdventure

{
    /*!
     * \brief   This is a container which store the state of the System.Random object
     *          at a given instant. It allowed to save and then restore the Random.
     *          Source : http://stackoverflow.com/questions/8188844/is-there-a-way-to-grab-the-actual-state-of-system-random/8188878#8188878
     *          Other source : http://stackoverflow.com/questions/19512210/how-to-save-the-state-of-a-random-gen
     */
    public class RandomState
    {
        private int _seed;
        private byte[] _save;

        public int seed
        {
            get { return _seed; }
        }

        /*!
        * \brief   TODO
        */
        public RandomState()
        {

        }

        /*!
         * \brief   Save a random at this very moment and its seed
         * \param   a_Random the random which has to be saved
         * \param   a_Seed the seed of the random (not necessary)
        */
        public void Save(System.Random random, int seed)
        {
            _seed = seed;

            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, random);

            _save = stream.ToArray();
        }

        /*!
         * \brief   Get the random saved
         * \return  The random saved
        */
        public System.Random Restore()
        {
            MemoryStream stream = new MemoryStream(_save);
            BinaryFormatter formatter = new BinaryFormatter();

            return (System.Random)formatter.Deserialize(stream);
        }
    }
}
