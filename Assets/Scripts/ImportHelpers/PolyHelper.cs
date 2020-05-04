using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Un utilitaire pour faire du debug/hack sur les meshes
public class PolyHelper : MonoBehaviour
{
    public Transform planet;
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

            //On place les faces
            Transform newFace = Instantiate(facetPrefab, transform);
            newFace.localPosition = face.localPosition;

            //On les oriente selon la bonne normale
            Vector3 normal = face.GetComponent<MeshFilter>().sharedMesh.normals[0];
            newFace.forward = normal;

            //Il ne manque plus que la rotation autour du vecteur Z
            Vector3 perpOld = GetPerpendicularVector(face.GetComponent<MeshFilter>().sharedMesh);
            Vector3 perpNew = GetPerpendicularVector(newFace.GetComponent<MeshFilter>().sharedMesh);

            perpNew = Quaternion.AngleAxis(newFace.localEulerAngles.x, transform.right) * perpNew;
            perpNew = Quaternion.AngleAxis(newFace.localEulerAngles.y, transform.up) * perpNew;
            
            float angleZ = Vector3.Angle(perpOld, perpNew);
            angleZ = (Vector3.Dot(perpOld, transform.up) > 0) ? -angleZ : angleZ;

            newFace.transform.localRotation = Quaternion.Euler(new Vector3(
                newFace.transform.localRotation.eulerAngles.x,
                newFace.transform.localRotation.eulerAngles.y,
                angleZ
            ));
            
            newFace.name = face.name + " (clone)";
        }
    }


    //Retourne le vecteur perpendiculaire au plus petit côté d'un triangle triangleMesh
    private Vector3 GetPerpendicularVector(Mesh triangleMesh)
    {
        (int, int) smallestSideIndexes = GetBiggestSide(triangleMesh);
        Vector3 p1 = triangleMesh.vertices[smallestSideIndexes.Item1];
        Vector3 p2 = triangleMesh.vertices[smallestSideIndexes.Item2];

        Vector3 perp = Vector3.Cross((p2 - p1).normalized, triangleMesh.normals[0].normalized);

        return perp;
    }

    //Retourne les index du plus petit côté d'un triangle triangleMesh (donc qui a vertices.Length = 3)
    private (int, int) GetBiggestSide(Mesh triangleMesh)
    {
        (int, int) indexes;

        float sizeP1P2 = Vector3.Distance(triangleMesh.vertices[0], triangleMesh.vertices[1]);
        float sizeP1P3 = Vector3.Distance(triangleMesh.vertices[0], triangleMesh.vertices[2]);
        float sizeP2P3 = Vector3.Distance(triangleMesh.vertices[1], triangleMesh.vertices[2]);

        if (sizeP1P2 >= sizeP1P3 && sizeP1P2 >= sizeP2P3) {
            indexes = (0, 1);
        }
        else if (sizeP1P3 >= sizeP1P2 && sizeP1P3 >= sizeP2P3) {
            indexes = (2, 0);
        }
        else {
            indexes = (1, 2);
        }

        return indexes;
    }
    

    //Pour linker chaque voisin
    public void LinkNeighbours()
    {
        Tile[] tiles = planet.GetComponentsInChildren<Tile>();
        bool oneEmpty = false;
        foreach (Tile tile in tiles) {
            if (tile.neighbours[(int)TileSide.AB] != null) {
                tile.neighbours[(int)TileSide.AB].neighbours[(int)TileSide.BC] = tile;
            }
            else oneEmpty = true;
            if (tile.neighbours[(int)TileSide.BC] != null) {
                tile.neighbours[(int)TileSide.BC].neighbours[(int)TileSide.AB] = tile;
            }
            else oneEmpty = true;
            if (tile.neighbours[(int)TileSide.CA] != null) {
                tile.neighbours[(int)TileSide.CA].neighbours[(int)TileSide.CA] = tile;
            }
            else oneEmpty = true;
        }
        if (oneEmpty) Debug.Log("AT LEAST ONE NEIGHBOUR MISSING");
    } 
}
