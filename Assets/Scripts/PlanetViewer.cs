using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetViewer : MonoBehaviour
{
    public Transform planet;
    public Transform planetWrapper;
    public Camera mainCamera;

    private Vector3 m_PrevMousePos;
    private bool m_IsMoving = false;
    public float m_Speed = 30f;


    private void Update()
    {
        MovePlanet();
    }

    //Pour faire tourner la planete
    private void MovePlanet()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform == planet) {
                    m_IsMoving = true;
                    m_PrevMousePos = Input.mousePosition;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            m_IsMoving = false;
        }

        if (m_IsMoving) {
            Vector3 mouseMove = m_PrevMousePos - Input.mousePosition;

            planet.Rotate(Vector3.right, -mouseMove.y * Time.deltaTime * m_Speed, Space.World);
            planet.Rotate(Vector3.up, mouseMove.x * Time.deltaTime * m_Speed, Space.World);

            m_PrevMousePos = Input.mousePosition;
        }
    }
}
