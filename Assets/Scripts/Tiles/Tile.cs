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
    public TileGenerator tileGenerator;

    [HideInInspector]
    public bool isCollapsed = false;

    private TileModel[] m_TileModels;
    private TileModel m_SavedTileModel;

    private float m_SumAllWeights = 0;
    private float m_SumAllWeightsLogWeights = 0;

    private float m_EntropyNoise;


    public void InitTileModels(TileModel[] tileModels, float sumWeights, float sumWeightsLogWeights)
    {
        m_TileModels = (TileModel[]) tileModels.Clone();
        m_SumAllWeights = sumWeights;
        m_SumAllWeightsLogWeights = sumWeightsLogWeights;
        m_EntropyNoise = Random.Range(0.0f, 0.00001f);
    }

    //On choisit le modele de la tile
    public void Collapse()
    {
        int tileModelIndex = GetPossibleTileIndex();
        m_SavedTileModel = Instantiate(m_TileModels[tileModelIndex].transform, transform).GetComponent<TileModel>();
        isCollapsed = true;

        for (int i = 0; i < m_TileModels.Length; ++i) {
            if (i != tileModelIndex) {
                m_TileModels[i].isPossible = false;
            }
        }
    }

    private int GetPossibleTileIndex()
    {
        float remaining = m_SumAllWeights;

        Debug.Log(m_SumAllWeights);

        for (int index = 0; index < m_TileModels.Length; ++index) {
            if (m_TileModels[index].isPossible) {
                if (remaining >= m_TileModels[index].weight) {
                    remaining -= m_TileModels[index].weight;
                }
                else {
                    return index;
                }
            }
        }

        throw new System.ArithmeticException("Erreur : m_SumAllWeights ne reflete pas la somme des probas de toute les tiles possibles");
    }


    public void LaunchPropagation()
    {
        neighbours[(int)TileSide.AB].Propagate(this, TileSide.AB);
        neighbours[(int)TileSide.BC].Propagate(this, TileSide.BC);
        neighbours[(int)TileSide.CA].Propagate(this, TileSide.CA);
    }

    private void Propagate(Tile prev, TileSide side)
    {
        if (prev.HasNoPossibleTiles()) {
            tileGenerator.OnContradiction();
            return;
        }

        bool hasChanged = false;

        foreach ((TileModel possibleTileModel, int id) in new PossibleTileModelIterator(m_TileModels)) {
            bool foundCompatible = false;
            foreach ((TileModel prevPossibleTileModel, int idPrev) in new PossibleTileModelIterator(prev.GetTileModels())) {
                if (prevPossibleTileModel.IsCompatible(possibleTileModel, side)) {
                    foundCompatible = true;
                    break;
                }
            }

            if (!foundCompatible) {
                RemovePossibleTileModel(id);
                hasChanged = true;
            }
        }

        //On continue la propagation si on a supprimé au moins une tile possible
        if (hasChanged) {
            tileGenerator.RegisterNewEntropy(this);
            switch (side) {
                case TileSide.AB:
                    Propagate(this, TileSide.BC);
                    Propagate(this, TileSide.CA);
                    break;
                case TileSide.BC:
                    Propagate(this, TileSide.CA);
                    Propagate(this, TileSide.AB);
                    break;
                case TileSide.CA:
                    Propagate(this, TileSide.AB);
                    Propagate(this, TileSide.BC);
                    break;
                default:
                    break;
            }
        }
    }

    private bool HasNoPossibleTiles()
    {
        int cpt = 0;
        foreach ((TileModel possibleTileModel, int id) in new PossibleTileModelIterator(m_TileModels)) {
            cpt++;
        }
        return (cpt < 1);
    }

    private void RemovePossibleTileModel(int index)
    {
        m_TileModels[index].isPossible = false;
        m_SumAllWeights -= m_TileModels[index].weight;
        m_SumAllWeightsLogWeights -= m_TileModels[index].weight * Mathf.Log(m_TileModels[index].weight, 2);
    }

    public float Entropy()
    {
        return (Mathf.Log(m_SumAllWeights, 2) - (m_SumAllWeightsLogWeights / m_SumAllWeights)) + m_EntropyNoise;
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

    public TileModel[] GetTileModels()
    {
        return m_TileModels;
    }
}

//Pour itérer seulement sur les TileModel possibles
public class PossibleTileModelIterator : IEnumerable<(TileModel, int)>
{
    private TileModel[] m_TileModels;
    public PossibleTileModelIterator(TileModel[] tileModels)
    {
        m_TileModels = tileModels;
    }

    public IEnumerator<(TileModel, int)> GetEnumerator()
    {
        for (int i = 0; i < m_TileModels.Length; ++i) {
            if (m_TileModels[i].isPossible) {
                yield return (m_TileModels[i], i);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}