using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Tile neighbourAB;
    public Tile neighbourBC;
    public Tile neighbourCA;

}
