using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileSide { AB, BC, CA}

//Represente un triangle du pentakis dodecahedre
public class Tile : MonoBehaviour
{
    /*
     * Chaque tile est un triangle isocele tel que :
     *       B 
     *     /  \ 
     *    A -- C
     * avec AB == BC
     */

    public Tile[] neighbours = new Tile[3];

    [HideInInspector]
    public bool isCollapsed = false;

    private TileModel[] m_PossibleTileModels;
    private TileModel m_SavedTileModel;

    private float m_SumAllWeights = 0;
    private float m_SumAllWeightsLogWeights = 0;


    public void InitTileModels(TileModel[] tileModels, float sumWeights, float sumWeightsLogWeights)
    {
        m_PossibleTileModels = (TileModel[]) tileModels.Clone();
        m_SumAllWeights = sumWeights;
        m_SumAllWeightsLogWeights = sumWeightsLogWeights;
    }

    //On choisit le modele de la tile
    public void Collapse()
    {
        int tileModelIndex = GetPossibleTileIndex();
        m_SavedTileModel = Instantiate(m_PossibleTileModels[tileModelIndex].transform, transform).GetComponent<TileModel>();

        for (int i = 0; i < m_PossibleTileModels.Length; ++i) {
            if (i != tileModelIndex) {
                m_PossibleTileModels[i].isPossible = false;
            }
        }
    }

    private int GetPossibleTileIndex()
    {
        float remaining = m_SumAllWeights;

        for (int index = 0; index < m_PossibleTileModels.Length; ++index) {
            if (m_PossibleTileModels[index].isPossible) {
                if (remaining >= m_PossibleTileModels[index].weight) {
                    remaining -= m_PossibleTileModels[index].weight;
                }
                else {
                    return index;
                }
            }
        }

        throw new System.ArithmeticException("Erreur : m_SumAllWeights ne reflete pas la somme des probas de toute les tiles possibles");
    }

    public void Propagate()
    {

    }

    public float Entropy()
    {
        return Mathf.Log(m_SumAllWeights, 2) - (m_SumAllWeightsLogWeights / m_SumAllWeights);
    }

    public void RemovePossibleTileModel(int tileModelIndex)
    {
        m_PossibleTileModels[tileModelIndex].isPossible = false;
    }

    public void RemoveSavedTileModel()
    {
        if (m_SavedTileModel != null) {
            Destroy(m_SavedTileModel.gameObject);
        }
    }

    public static TileSide GetOppositeSide(TileSide side)
    {
        TileSide opposite = TileSide.AB;

        switch (side) {
            case TileSide.AB:
                opposite = TileSide.BC;
                break;
            case TileSide.BC:
                opposite = TileSide.AB;
                break;
            case TileSide.CA:
                opposite = TileSide.CA;
                break;
            default:
                break;
        }

        return opposite;
    }
}