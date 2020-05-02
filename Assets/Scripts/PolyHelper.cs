using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyHelper : MonoBehaviour
{
    public Transform facetPrefab;


    public void BuildFaces()
    {
        Debug.Log("== ADDING FACES ==");

        //On prend toutes les faces du polyèdre
        List<Transform> children = new List<Transform>();
        foreach (MeshFilter child in GetComponentsInChildren<MeshFilter>()) {
            children.Add(child.transform);
        }
        children.Remove(transform);

        //On crée les nouvelles faces
        foreach (Transform face in children) {
            Debug.Log("CHILD PROCESSED : " + face.name);

            Transform newFace = Instantiate(facetPrefab, transform);
            newFace.localPosition = face.localPosition;
            Vector3 normal = face.GetComponent<MeshFilter>().sharedMesh.normals[0];
            newFace.forward = normal;
        }
    }


    //Retourne le vecteur perpendiculaire au plus petit côté d'un triangle triangleMesh
    private Vector3 GetPerpendicularVector(Mesh triangleMesh)
    {
        (int, int) smallestSideIndexes = GetSmallestSide(triangleMesh);
        Vector3 p1 = triangleMesh.vertices[smallestSideIndexes.Item1];
        Vector3 p2 = triangleMesh.vertices[smallestSideIndexes.Item2];

        return Vector3.Cross(p2 - p1, triangleMesh.normals[0]);
    }

    //Retourne les index du plus petit côté d'un triangle triangleMesh (donc qui a vertices.Length = 3)
    private (int, int) GetSmallestSide(Mesh triangleMesh)
    {
        (int, int) indexes = (0, 0);
        Vector3 p1 = triangleMesh.vertices[0];
        Vector3 p2 = triangleMesh.vertices[1];
        Vector3 p3 = triangleMesh.vertices[2];

        float sizeP1P2 = Vector3.Distance(triangleMesh.vertices[0], triangleMesh.vertices[1]);
        float sizeP1P3 = Vector3.Distance(triangleMesh.vertices[0], triangleMesh.vertices[2]);
        float sizeP2P3 = Vector3.Distance(triangleMesh.vertices[1], triangleMesh.vertices[2]);

        if (sizeP1P2 <= sizeP1P3 && sizeP1P2 <= sizeP2P3) {
            indexes = (0, 1);
        }
        else if (sizeP1P3 <= sizeP1P2 && sizeP1P3 <= sizeP2P3) {
            indexes = (1, 2);
        }
        else {
            indexes = (2, 0);
        }

        return indexes;
    }
    
}
