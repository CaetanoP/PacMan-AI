using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDotGen : MonoBehaviour
{
    public GameObject pacDotPrefab; // Prefab do Pac-Dot
    public GameObject superPacDotPrefab; // Prefab do Super Pac-Dot
    public GameObject nodePrefab; // Prefab do nó
    public GameObject wallParent;  // Objeto pai que contém as paredes
    public Vector2 gridSize;      // Tamanho do labirinto (x, y)
    public float cellSize = 0.5f;   // Tamanho da célula do grid
    public Vector2 offset;         // Offset do grid
    //Lista de vetores2
    public List<Vector2> nodePositions = new List<Vector2>();
    void Start()
    {
        SpawnPacDots();
    }

    void SpawnPacDots()
    {
        float Y = transform.position.y - offset.x;
        float X;
        int nodeCount = 0;
        for (float y = 0; y < gridSize.y; y += 1)
        {

            X = transform.position.x - offset.y;
            for (float x = 0; x < gridSize.x; x += 1)
            {
                Vector3 position = new Vector3(X, Y, 0);

                // Verifica se existe uma parede no local
                Collider2D hit = Physics2D.OverlapCircle(new Vector2(X, Y), 0.1f);
                if (hit == null || !hit.CompareTag("Wall"))
                {
                    // Instancia o Pac-Dot se não houver uma parede
                    var instance_pac = Instantiate(pacDotPrefab, position, Quaternion.identity, transform);
                    instance_pac.name = $"Pac-Dot ({x}, {y})";
                    //Verifica se existe um nó no local
                    if (nodeCount < nodePositions.Count && y == nodePositions[nodeCount].y && x == nodePositions[nodeCount].x)
                    {
                        //Instancia o nó se não houver uma parede
                        var instance_node = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                        instance_node.name = $"Node ({x}, {y})";
                        nodeCount++;
                    }
                }


                X += cellSize;
            }
            Y += cellSize;
        }
    }
}
