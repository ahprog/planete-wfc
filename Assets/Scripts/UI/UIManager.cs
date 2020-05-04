using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlanetViewer planetViewer;
    public TileGenerator tileGenerator;

    public void Reset()
    {
        tileGenerator.resetsRemaining = 1;
        tileGenerator.ResetTiles();
        tileGenerator.GenerateTiles();
        StartCoroutine(planetViewer.SpawnAnimation(0.1f));
    }
}
