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

}
