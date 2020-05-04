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

    [HideInInspector]
    public bool isPossible = true;
}