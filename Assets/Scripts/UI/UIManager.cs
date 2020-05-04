using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlanetViewer planetViewer;
    public TileGenerator tileGenerator;

    public void Reset()
    {
        tileGenerator.ResetTiles();
        tileGenerator.GenerateTiles();
    }
}
