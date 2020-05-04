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
    public List<TileModel> possibleTileModels;

    [HideInInspector]
    private TileModel m_SavedTileModel;

    public void InitTileModels(TileModel[] tileModels)
    {
        possibleTileModels = new List<TileModel>(tileModels);
    }

    public void ChooseRandomTileModel()
    {
        int tileModelIndex = Random.Range(0, possibleTileModels.Count);
        m_SavedTileModel = Instantiate(possibleTileModels[tileModelIndex].transform, transform).GetComponent<TileModel>();

        for (int i = 0; i < possibleTileModels.Count; ++i) {
            if (i != tileModelIndex) {
                possibleTileModels[i].isPossible = false;
            }
        }
    }

    public void RemoveTileModel()
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
