namespace cr_mono.Core.GameLogic
{
    // class for generating some basic noise i can use to sample
    // the isometric maps to place features such as water, elevation etc.

    // source: http://devmag.org.za/2009/04/25/perlin-noise/

    internal static class Noise
    {
        private static float Interpolate(float x, float y, float alpha) {
            return x * (1 - alpha) + alpha * y;
        }

        private static T[][] GetEmptyArray<T>(int width, int height) {
            T[][] image = new T[width][];

            for (int i = 0; i < width; i++) {
                image[i] = new T[height];
            }

            return image;
        }

        private static float[][] GenerateWhiteNoise(int width, int height, RNG rng)
        {
            float[][] noise = GetEmptyArray<float>(width, height);

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    noise[i][j] = (float)rng.NextDouble();
                }    
            }

            return noise;
        }

        private static float[][] GenerateSmoothNoise(float[][] baseNoise, int octave) {
            int width = baseNoise.Length;
            int height = baseNoise[0].Length;

            float[][] smoothNoise = GetEmptyArray<float>(width, height);

            int samplePeriod = 1 << octave;
            float sampleFrequency = 1.0f / samplePeriod;

            for (int i = 0; i < width; i++) {
                // doing (x/samplePeriod)*samplePeriod gives floored value
                int sample_i0 = (i / samplePeriod) * samplePeriod;
                int sample_i1 = (sample_i0 + samplePeriod) % width;
                float horizontalBlend = (i - sample_i0) * sampleFrequency;

                for (int j = 0; j < height; j++) {
                    int sample_j0 = (j / samplePeriod) * samplePeriod;
                    int sample_j1 = (sample_j0 + samplePeriod) % height;
                    float verticalBlend = (j - sample_j0) * sampleFrequency;

                    float top = Interpolate(
                        baseNoise[sample_i0][sample_j0],
                        baseNoise[sample_i1][sample_j0],
                        horizontalBlend);

                    float bottom = Interpolate(
                        baseNoise[sample_i0][sample_j1],
                        baseNoise[sample_i1][sample_j1],
                        horizontalBlend);

                    smoothNoise[i][j] = Interpolate(top, bottom, verticalBlend);
                }
            }

            return smoothNoise;
        }

        internal static float[][] GeneratePerlinNoise(
            RNG rng, int width, int height, int octaveCount) {
            float[][] baseNoise = GenerateWhiteNoise(width, height, rng);
            float[][][] smoothNoise = new float[octaveCount][][];
            
            float persistence = 0.5f;

            for (int i = 0; i < octaveCount; i++) {
                smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
            }

            float[][] perlinNoise = GetEmptyArray<float>(width, height);
            float amplitude = 1.0f;
            float totalAmplitude = 0.0f;

            for (int octave = octaveCount - 1; octave >= 0; octave--) {
                amplitude *= persistence;
                totalAmplitude += amplitude;

                for (int i = 0; i < width; i++) {
                    for (int j = 0; j < height; j++) {
                        perlinNoise[i][j] += smoothNoise[octave][i][j] * amplitude;
                    }
                }
            }

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    perlinNoise[i][j] /= totalAmplitude;
                }
            }

            return perlinNoise;
        }
    }
}
