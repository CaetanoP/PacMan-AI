using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Prefab do Pac Man
    public GameObject pacManPrefab;
    // Prefab do Fantasma
    public GameObject[] ghostPrefab;
    //Initial Position
    public Vector2 pacManInitialPosition = new Vector2(0,-4);

    public void SetupEpisode()
    {
        //Teleporta o Pac
        pacManPrefab.transform.position = pacManInitialPosition;
    }
}
