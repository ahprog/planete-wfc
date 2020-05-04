using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WFCReturnState
{
    Good,
    ContradictionFound
}

public class TileGenerator : MonoBehaviour
{
    public Transform planet;

    private Tile[] m_Tiles;
    private TileModel[] m_TileModelPrefabs;

    private SortedSet<EntropyTile> m_SortedEntropies;

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

        float sumWeights = 0;
        float sumWeightsLogWeights = 0;

        foreach (TileModel tileModel in m_TileModelPrefabs) {
            sumWeights += tileModel.weight;
            sumWeightsLogWeights += tileModel.weight * Mathf.Log(tileModel.weight, 2);
        }

        foreach (Tile tile in m_Tiles) {
            tile.InitTileModels(m_TileModelPrefabs, sumWeights, sumWeightsLogWeights);
        }
    }

    //Ici on implémente le WFC
    public void GenerateTiles()
    {
        RemoveTileModels();
        foreach (Tile tile in m_Tiles) {
            tile.Collapse();
        }


        int numberOfTilesRemaining = m_TileModelPrefabs.Length;
        while (numberOfTilesRemaining > 0) {

            //On choisit une tile avec entropie minimale
            (Tile tile, WFCReturnState nextTileReturnState) = PickNextTile();

            if (nextTileReturnState != WFCReturnState.ContradictionFound) {
                //2. collapser cette tile
                tile.Collapse();

                //3. propagate
                tile.Propagate();

                numberOfTilesRemaining -= 1;
            }
            else {
                OnContradiction();
                break;
            }
        }
    }

    private (Tile, WFCReturnState) PickNextTile()
    {
        while (m_SortedEntropies.Count > 0) {
            EntropyTile entropyTile = m_SortedEntropies.Min;

            if (!entropyTile.tile.isCollapsed) {
                return (m_SortedEntropies.Min.tile, WFCReturnState.Good);
            }

            m_SortedEntropies.Remove(entropyTile);
        }

        return (null, WFCReturnState.ContradictionFound);
    }

    private void OnContradiction()
    {
        //TODO : ici il faut reset la generation
        Debug.Log("CONTRADICTION FOUND");
    }

    public void RemoveTileModels()
    {
        foreach (Tile tile in m_Tiles) {
            tile.RemoveSavedTileModel();
        }
    }
}
