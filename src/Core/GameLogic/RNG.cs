using System;

namespace cr_mono.Core.GameLogic
{
    internal class RNG
    {
        //Psuedo random number generator based on xoshiro256**
        // https://prng.di.unimi.it/

        private ulong[] state = new ulong[4];

        internal RNG(ulong seed) 
        {
            Seed(seed);
        }

        internal ulong[] GetState() 
        {
            return (ulong[])state.Clone();
        }

        internal void SetState(ulong[] newState)
        {
            if (newState.Length != 4) 
            {
                throw new ArgumentException("Invalid State Length");
            }
            Array.Copy(newState, state, 4);
        }

        internal int Next(int min, int max) 
        {
            return (int)(NextUlong() % (ulong)(max - min)) + min;
        }

        internal double NextDouble() 
        {
            return (NextUlong() >> 11) * (1.0 / (1UL << 53));
        }

        internal void NextBytes(byte[] buffer) 
        {
            for (int i = 0; i < buffer.Length; i++) 
            {
                ulong rand = NextUlong();
                for (int j = 0; j < 8 && (i + j) < buffer.Length; j++)
                {
                    buffer[i + j] = (byte)(rand >> (j * 8));
                }
            }
        }

        private void Seed(ulong seed) 
        {
            state[0] = SplitMix64(ref seed);
            state[1] = SplitMix64(ref seed);
            state[2] = SplitMix64(ref seed);
            state[3] = SplitMix64(ref seed);
        }

        private static ulong SplitMix64(ref ulong seed) 
        {
            ulong z = (seed += 0x9E3779B97F4A7C15);
            z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9;
            z = (z ^ (z >> 27)) * 0x94D049BB133111EB;
            return z ^ (z >> 31);
        }

        private ulong NextUlong() 
        {
            ulong result = state[0] + state[3];

            ulong t = state[1] << 17;

            state[2] ^= state[0];
            state[3] ^= state[1];
            state[1] ^= state[2];
            state[0] ^= state[3];

            state[2] ^= t;

            state[3] = RotateLeft(state[3], 45);

            return result;
        }

        private static ulong RotateLeft(ulong x, int k) 
        {
            return (x << k) | (x >> (64 - k));
        }
    }
}
