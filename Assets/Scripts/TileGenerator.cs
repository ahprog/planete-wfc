using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public Transform planet;

    private Tile[] m_Tiles;
    private TileModel[] m_TileModelPrefabs;

    private void Awake()
    {
        m_Tiles = planet.GetComponentsInChildren<Tile>();
    }

    private void Start()
    {
        LoadTiles();
        GenerateTiles();
    }

    private void LoadTiles()
    {
        m_TileModelPrefabs = Resources.LoadAll<TileModel>("Prefabs/Tiles");
        Debug.Log("LOADED " + m_TileModelPrefabs.Length + " TILES");

        foreach (Tile tile in m_Tiles) {
            tile.InitTileModels(m_TileModelPrefabs);
        }
    }

    public void GenerateTiles()
    {
        RemoveTileModels();
        foreach (Tile tile in m_Tiles) {
            tile.ChooseRandomTileModel();
        }
    }

    public void RemoveTileModels()
    {
        foreach (Tile tile in m_Tiles) {
            tile.RemoveTileModel();
        }
    }
}
