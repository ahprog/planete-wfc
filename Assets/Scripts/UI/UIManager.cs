using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlanetViewer planetViewer;
    public TileGenerator tileGenerator;

    public void DefaultReset()
    {
        tileGenerator.resetsRemaining = 5;
        tileGenerator.ResetTiles();
        tileGenerator.GenerateTiles();
        StartCoroutine(planetViewer.SpawnAnimation(0.1f));
    }

    public void SlowReset()
    {
        tileGenerator.resetsRemaining = 5;
        tileGenerator.ResetTiles();
        tileGenerator.GenerateTiles();
        StartCoroutine(planetViewer.SpawnAnimation(0.1f));
    }
}
