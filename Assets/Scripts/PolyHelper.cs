using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyHelper : MonoBehaviour
{
    public Transform facetPrefab;


    public void BuildFaces()
    {
        Debug.Log("== ADDING FACES ==");

        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform) {
            children.Add(child);
        }

        foreach (Transform face in children) {
            Debug.Log("CHILD PROCESSED : " + face.name);

            Transform newFace = Instantiate(facetPrefab, transform);
            newFace.localPosition = face.localPosition;
            Vector3 normal = face.GetComponent<MeshFilter>().sharedMesh.normals[0];
            newFace.forward = normal;
        }
    }
}
