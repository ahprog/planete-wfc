using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gere les interactions avec les boutons UI
public class UIManager : MonoBehaviour
{
    public PlanetViewer planetViewer;
    public TileGenerator tileGenerator;

    private Coroutine m_CachedGenerateCoroutine;

    public void DefaultReset()
    {
        if (m_CachedGenerateCoroutine != null) StopCoroutine(m_CachedGenerateCoroutine);

        tileGenerator.resetsRemaining = 5;
        tileGenerator.ResetTiles();

        m_CachedGenerateCoroutine = StartCoroutine(tileGenerator.GenerateTiles());
        StartCoroutine(planetViewer.SpawnAnimation(0.1f));
    }

    public void QuickAnimReset()
    {
        if (m_CachedGenerateCoroutine != null) StopCoroutine(m_CachedGenerateCoroutine);

        tileGenerator.resetsRemaining = 5;
        tileGenerator.ResetTiles();

        m_CachedGenerateCoroutine = StartCoroutine(tileGenerator.GenerateTiles(true, 0.1f));
        StartCoroutine(planetViewer.SpawnAnimation(0.1f));
    }

    public void SlowAnimReset()
    {
        if (m_CachedGenerateCoroutine != null) StopCoroutine(m_CachedGenerateCoroutine);

        tileGenerator.resetsRemaining = 5;
        tileGenerator.ResetTiles();

        m_CachedGenerateCoroutine = StartCoroutine(tileGenerator.GenerateTiles(true, 0.75f));
        StartCoroutine(planetViewer.SpawnAnimation(0.1f));
    }
}
