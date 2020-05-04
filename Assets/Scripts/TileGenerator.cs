using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Peuple la planete de TileModel en appliquant le WFC algorithm
//Ressources:https://gridbugs.org/wave-function-collapse/
public class TileGenerator : MonoBehaviour
{
    public Transform planet;
    public PlanetViewer planetViewer;

    private Tile[] m_Tiles;
    private TileModel[] m_TileModelPrefabs;

    private SortedSet<EntropyTile> m_SortedEntropies;

    private float m_CachedBaseSumWeights;
    private float m_CachedBaseSumWeightsLogWeights;

    [HideInInspector]
    public int resetsRemaining = 5;

    private void Awake()
    {
        m_Tiles = planet.GetComponentsInChildren<Tile>();
        m_SortedEntropies = new SortedSet<EntropyTile>();
        m_SortedEntropies.Add(new EntropyTile(m_Tiles[0], m_Tiles[0].Entropy()));
    }

    private void Start()
    {
        LoadTiles();
        StartCoroutine(GenerateTiles());
    }

    //Charge les prefabs des TileModel dans chaque Tile
    private void LoadTiles()
    {
        m_TileModelPrefabs = Resources.LoadAll<TileModel>("Prefabs/Tiles");
        Debug.Log("LOADED " + m_TileModelPrefabs.Length + " TILES MODEL ; " + m_Tiles.Length + " TILES");

        m_CachedBaseSumWeights = 0;
        m_CachedBaseSumWeightsLogWeights = 0;

        //On en profite pour mettre en cache les deux principaux paramètres de la fonction d'entropie
        foreach (TileModel tileModel in m_TileModelPrefabs) {
            m_CachedBaseSumWeights += tileModel.weight;
            m_CachedBaseSumWeightsLogWeights += tileModel.weight * Mathf.Log(tileModel.weight, 2);
        }

        foreach (Tile tile in m_Tiles) {
            tile.InitTileModels(m_TileModelPrefabs, m_CachedBaseSumWeights, m_CachedBaseSumWeightsLogWeights);
        }
    }

    public IEnumerator GenerateTiles(bool slow = false, float time = 0)
    {
        int numberOfTilesRemaining = m_Tiles.Length;
        while (numberOfTilesRemaining > 0) {
            Tile tile;
            //On choisit une tile avec entropie minimale
            if ((tile = PickNextTile()) != null) {
                //On lui applique un de ses model au hasard
                tile.Collapse();

                //pour l'animation
                if (slow) {
                    planetViewer.AlignOnTile(tile);
                    yield return new WaitForSeconds(time);
                }

                //on met à jour les models possibles des tiles environnantes
                tile.LaunchPropagation();

                numberOfTilesRemaining -= 1;
            }
            else {
                OnContradiction();
                break;
            }
        }
    }

    //Retourne la tile avec la plus petite entropie qui n'a pas encore de TileModel
    private Tile PickNextTile()
    {
        EntropyTile nextEntropyTile = null;
        List<EntropyTile> entropyTilesToRemove = new List<EntropyTile>();
        foreach (EntropyTile entropyTile in m_SortedEntropies) {
            if (!entropyTile.tile.isCollapsed) {
                nextEntropyTile = entropyTile;
                break;
            }
            entropyTilesToRemove.Add(entropyTile);
        }

        if (nextEntropyTile == null) {
            OnContradiction();
        }

        //Les tiles deja traitées sont enlevées de l'arbre
        foreach (EntropyTile rem in entropyTilesToRemove) {
            m_SortedEntropies.Remove(rem);
        }

        return nextEntropyTile?.tile;
    }

    //Il est possible qu'un set de TileModel ne permette pas toujours d'avoir une génération qui fonctionne du premier coup
    //On se permet plusieurs essais
    public void OnContradiction()
    {
        Debug.Log("CONTRADICTION FOUND");
        if (resetsRemaining > 0) {
            ResetTiles();
            StartCoroutine(GenerateTiles());
            resetsRemaining -= 1;
        }
    }

    public void RegisterNewEntropy(Tile tile)
    {
        //Debug.Log("NEW ENTROPY ADDED ; ENTROPY = " + tile.Entropy());
        EntropyTile newEntropy = new EntropyTile(tile, tile.Entropy());
        m_SortedEntropies.Add(newEntropy);
    }

    public void ResetTiles()
    {
        foreach (Tile tile in m_Tiles) {
            tile.ResetTile(m_CachedBaseSumWeights, m_CachedBaseSumWeightsLogWeights);
        }
        m_SortedEntropies.Clear();
        m_SortedEntropies.Add(new EntropyTile(m_Tiles[0], m_Tiles[0].Entropy()));
    }
}
