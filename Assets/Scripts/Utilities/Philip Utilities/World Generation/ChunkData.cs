using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;
using UnityEngine;


public class ChunkData
{
    public Vector2Int Coordinates { private set; get; }
    public float[,] PrecipitationMap { private set; get; }
    public float[,] TemperatureMap { private set; get; }
    public float[,] BaronMap { private set; get; }
    public float[,] TropicalityMap { private set; get; }
    public GameObject ChunkGameObject { private set; get; }
    public Tilemap WalkableTilemap { private set; get; }
    public Tilemap WaterTilemap { private set; get; }
    public Tilemap ColliderTilemap { private set; get; }
    public bool IsVisible
    {
        get
        {
            return ChunkGameObject.activeSelf;
        }
    }

    public ChunkData(Vector2Int coordinates, float[,] precipitationMap, float[,] temperatureMap, float[,] baronMap, float[,] tropicalityMap)
    {
        Coordinates = coordinates;
        PrecipitationMap = precipitationMap;
        TemperatureMap = temperatureMap;
        BaronMap = baronMap;
        TropicalityMap = tropicalityMap;
    }

    public void SetupChunk(GameObject chunkObject)
    {
        ChunkGameObject = chunkObject;
        WalkableTilemap = chunkObject.transform.GetChild(0).GetComponent<Tilemap>();
        WaterTilemap = chunkObject.transform.GetChild(1).GetComponent<Tilemap>();
        ColliderTilemap = chunkObject.transform.GetChild(2).GetComponent<Tilemap>();
        //SetVisible(false);
    }

    public void SetVisible(bool value)
    {
        ChunkGameObject.SetActive(value);
    }
}

