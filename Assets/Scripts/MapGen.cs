using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public GameObject pacDotPrefab; // Prefab do Pac-Dot
    public GameObject superPacDotPrefab; // Prefab do Super Pac-Dot
    public GameObject cornerNodePrefab; // Prefab do nó
    public GameObject nodePrefab; // Prefab do nó de esquina
    public GameObject powerUpPrefab; // Prefab do Power-Up
    public Vector2 gridSize;      // Tamanho do labirinto (x, y)
    public float cellSize = 0.5f; // Tamanho da célula do grid
    public Vector2 offset;        // Offset do grid
    //Lista de vetores2
    public List<Vector2> cornerNodesPositions = new List<Vector2>();
    public List<Vector2> powerUpPositions = new List<Vector2>();
    public List<Vector2> pacDotCenterBlockedPositions = new List<Vector2>();
    public void Awake()
    {
        SpawnMap();
    }

    void SpawnMap()
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
                    //Verifica se não é uma posição de Power-Up
                    if (!powerUpPositions.Contains(new Vector2(x, y)))
                    {
                        //Instancia o Pac-Dot
                        SpawnPacDot(y, x, position);
                    }
                    else
                    {
                        //Instancia o Power-Up
                        SpawnPowerUp(y, x, position);
                    }
                    //Verifica se existe um nó de esquina para ser instanciado
                    if (nodeCount < cornerNodesPositions.Count && y == cornerNodesPositions[nodeCount].y && x == cornerNodesPositions[nodeCount].x)
                    {
                        //Instancia o nó de esquina
                        SpawnCornerNode(y, x, position);
                        nodeCount++;
                    }
                    else
                    {
                        //Instancia um nó normal
                        SpawnNode(y, x, position);
                    }
                }
                X += cellSize;
            }
            Y += cellSize;
        }
    }

    private void SpawnCornerNode(float y, float x, Vector3 position)
    {
        var instance_node = Instantiate(cornerNodePrefab, position, Quaternion.identity, GameObject.Find("CornerNodes").transform);
        instance_node.name = $"Node ({x}, {y})";
    }

    private void SpawnPowerUp(float y, float x, Vector3 position)
    {
        var instance_powerUp = Instantiate(powerUpPrefab, position, Quaternion.identity, GameObject.Find("PowerUps").transform);
        instance_powerUp.name = $"Power-Up ({x}, {y})";
    }

    private void SpawnPacDot(float y, float x, Vector3 position)
    {
        //Verifica se é uma posição de Pac-Dot bloqueada
        if (pacDotCenterBlockedPositions.Contains(new Vector2(x, y)))
        {
            return;
        }
        //Instancia o Pac-Dot
        var instance_pac = Instantiate(pacDotPrefab, position, Quaternion.identity, GameObject.Find("PacDots").transform);
        instance_pac.name = $"Pac-Dot ({x}, {y})";
    }
    private void SpawnNode(float y, float x, Vector3 position)
    {
        //Instancia o nó
        var instance_node = Instantiate(nodePrefab, position, Quaternion.identity, GameObject.Find("Nodes").transform);
        instance_node.name = $"Node ({x}, {y})";
    }
}
