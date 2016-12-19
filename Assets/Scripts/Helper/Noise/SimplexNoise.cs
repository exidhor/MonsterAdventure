using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    // source : http://stackoverflow.com/questions/18279456/any-simplex-noise-tutorials-or-resource
    public class SimplexNoise
    {
        SimplexNoise_octave[] octaves;
        double[] frequencys;
        double[] amplitudes;

        //int largestFeature;
        SimplexNoiseEntry config;
        private RandomGenerator rand;

        public SimplexNoise(SimplexNoiseEntry config, RandomGenerator random)
        {
            this.config = config;
            rand = random;

            //recieves a number (eg 128) and calculates what power of 2 it is (eg 2^7)
            int numberOfOctaves = (int)Math.Ceiling(Math.Log10(config.largestFeature) / Math.Log10(2));

            octaves = new SimplexNoise_octave[numberOfOctaves];
            frequencys = new double[numberOfOctaves];
            amplitudes = new double[numberOfOctaves];

            for (int i = 0; i < numberOfOctaves; i++)
            {
                octaves[i] = new SimplexNoise_octave(rand);

                //frequencys[i] = Math.Pow(2, i);
                //amplitudes[i] = Math.Pow(persistence, octaves.Length - i);
                frequencys[i] = Math.Pow(config.zoomAcceleration, i*config.zoom - config.frequency);
                amplitudes[i] = Math.Pow(config.WhiteDetails, octaves.Length - i * config.contrast - config.colorOffset);
            }

        }


        public double GetNoise(int x, int y)
        {

            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                //double frequency = Math.pow(2,i);
                //double amplitude = Math.pow(persistence,octaves.length-i);

                result = result + octaves[i].noise(x / frequencys[i], y / frequencys[i]) * amplitudes[i];
            }


            return result;

        }

        public double GetNoise(int x, int y, int z)
        {

            double result = 0;

            for (int i = 0; i < octaves.Length; i++)
            {
                double frequency = Math.Pow(2, i);
                double amplitude = Math.Pow(config.WhiteDetails, octaves.Length - i);

                result = result + octaves[i].noise(x / frequency, y / frequency, z / frequency) * amplitude;
            }
            return result;

        }
    }
}
