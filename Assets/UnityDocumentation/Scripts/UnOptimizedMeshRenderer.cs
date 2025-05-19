
using UnityEngine;
using System.Collections.Generic;
public class UnOptimizedMeshRenderer : MonoBehaviour
{
    public Material material;
    public GameObject mesh;

    public int spawnCount = 10000;


    private void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {

            Instantiate(mesh, new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)), Quaternion.identity);
        }
    }
}