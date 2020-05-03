using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public Transform planet;

    private Transform[] m_TilePrefabs;

    private void Start()
    {
        m_TilePrefabs = Resources.LoadAll<Transform>("Prefabs/Tiles");
        Debug.Log("LOADED " + m_TilePrefabs.Length + " TILES");

        GenerateTiles();
    }


    public void GenerateTiles()
    {
        RemoveTiles();
        foreach (Transform child in planet) {
            Transform tile = Instantiate(m_TilePrefabs[Random.Range(0, m_TilePrefabs.Length)], child);
        }
    }

    public void RemoveTiles()
    {
        List<Tile> tiles = new List<Tile>();
        foreach (Tile tile in planet.GetComponentsInChildren<Tile>()) {
            tiles.Add(tile);
        }

        foreach (Tile tile in tiles) {
            Destroy(tile.gameObject);
        }
    }
}
