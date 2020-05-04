using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VertexTileType
{
    Ground,
    Sea,
    City
}

//Represente un modele de tile possible
public class TileModel : MonoBehaviour
{
    /*
     * On represente un TileModel selon le biome des trois sommets A B et C de la tile
     */
    public VertexTileType AType;
    public VertexTileType BType;
    public VertexTileType CType;

    public int weight = 1;


    public bool isPossible = true;

    public bool IsCompatible(TileModel other, TileSide side)
    {
        bool isCompatible = false;
        switch (side) {
            case TileSide.AB:
                isCompatible = (AType == other.CType && BType == other.BType);
                break;
            case TileSide.BC:
                isCompatible = (BType == other.BType && CType == other.BType);
                break;
            case TileSide.CA:
                isCompatible = (CType == other.AType && AType == other.CType);
                break;
            default:
                break;
        }
        return isCompatible;
    }
}