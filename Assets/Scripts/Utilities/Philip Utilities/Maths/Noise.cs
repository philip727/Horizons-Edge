using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Philip.Utilities.Math 
{
    public static class Noise
    {
        public enum NormalizeMode {
            Local, 
            Global,
        }

        public static float[,] GenerateMap(int mapWidth, int mapHeight, int seed, Vector2 offset, int octaves = 4, float persistence = 0.5f, float lacunarity = 2f, float scale = 1f, NormalizeMode normalizeMode=NormalizeMode.Local)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            System.Random prng = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            float maxPossibleHeight = 0;
            float amplitude = 1;
            float frequency = 1;

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) - offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                amplitude *= persistence;
            }

            float halfWidth = mapWidth / 2f;
            float halfHeight = mapHeight / 2f;


            if (scale <= 0)
                scale = 0.0001f;

            float maxLocalNoiseHeight = float.MinValue;
            float minLocalNoiseHeight = float.MaxValue;

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    amplitude = 1;
                    frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                        float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxLocalNoiseHeight)
                    {
                        maxLocalNoiseHeight = noiseHeight;
                    } 
                    else if (noiseHeight < minLocalNoiseHeight)
                    {
                        minLocalNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    if (normalizeMode == NormalizeMode.Local)
                    {
                        noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                    } 
                    else
                    {
                        float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxLocalNoiseHeight / 1.75f);
                        noiseMap[x, y] = normalizedHeight;
                    }
                }
            }

            return noiseMap;
        }

        public static float GenerateHeight(int x, int y, int seed, Vector2 offset, int octaves = 4, float persistence = 0.5f, float lacunarity = 2f, float scale = 1f)
        {
            System.Random prng = new System.Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            float maxPossibleHeight = 0;
            float amplitude = 1;
  

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = prng.Next(-100000, 100000) + offset.x;
                float offsetY = prng.Next(-100000, 100000) - offset.y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);

                maxPossibleHeight += amplitude;
                amplitude *= persistence;
            }

            float halfWidth = x / 2f;
            float halfHeight = y / 2f;

            if (scale <= 0)
                scale = 0.0001f;

            float noiseHeight = 0;
            float frequency = 1;
            amplitude = 1;

            for (int i = 0; i < octaves; i++)
            {
                float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                noiseHeight += perlinValue * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            return noiseHeight;
        }
    }
}
