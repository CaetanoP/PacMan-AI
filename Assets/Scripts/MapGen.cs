using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    [Header("Itens Prefabs")]
    public GameObject powerUpPrefab; // Prefab do Power-Up
    public GameObject pacDotPrefab; // Prefab do Pac-Dot


    [Header("Nodes Prefabs")]
    public GameObject decisionNodePrefab; // Prefab do nó
    public GameObject pathNodePrefab; // Prefab do nó de esquina
    public GameObject wallNodePrefab; // Prefab do wall

    [Header("Grid Configuration")]
    public Vector2 gridSize;      // Tamanho do labirinto (x, y)
    public float cellSize = 0.5f; // Tamanho da célula do grid
    public Vector2 offset;        // Offset do grid

    [Header("Nodes Configuration")]
    //Lista de vetores2
    public List<Vector2> decisionNodesPositions = new List<Vector2>();
    public List<Vector2> powerUpPositions = new List<Vector2>();
    public List<Vector2> pacDotCenterBlockedPositions = new List<Vector2>();

    [Header("Gen Configuration")]
    public bool generateItens = true;
    public bool generateNodes = true;


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

                    if (nodeCount < decisionNodesPositions.Count && y == decisionNodesPositions[nodeCount].y && x == decisionNodesPositions[nodeCount].x)
                    {
                        //Instancia o nó de decisão
                        SpawnDecisionNode(y, x, position);
                        nodeCount++;
                    }
                    else
                    {
                        //Instancia um nó de decisão
                        SpawnPathNode(y, x, position);
                    }
                }
                else
                {
                    //Instancia o wall
                    SpawnWallNode(y, x, position);
                }
                X += cellSize;
            }
            Y += cellSize;
        }
    }



    private void SpawnPowerUp(float y, float x, Vector3 position)
    {
        if (generateItens == false)
        {
            return;
        }
        var instance_powerUp = Instantiate(powerUpPrefab, position, Quaternion.identity, GameObject.Find("PowerUps").transform);
        instance_powerUp.name = $"Power-Up ({x}, {y})";
    }

    private void SpawnPacDot(float y, float x, Vector3 position)
    {
        //Verifica se é uma posição de Pac-Dot bloqueada
        if (pacDotCenterBlockedPositions.Contains(new Vector2(x, y)) || generateItens == false)
        {
            return;
        }
        //Instancia o Pac-Dot
        var instance_pac = Instantiate(pacDotPrefab, position, Quaternion.identity, GameObject.Find("PacDots").transform);
        instance_pac.name = $"Pac-Dot ({x}, {y})";
    }
    private void SpawnDecisionNode(float y, float x, Vector3 position)
    {
        if (generateNodes == false)
        {
            return;
        }
        var instance_node = Instantiate(decisionNodePrefab, position, Quaternion.identity, GameObject.Find("DecisionNodes").transform);
        instance_node.name = $"Node ({x}, {y})";
    }

    private void SpawnPathNode(float y, float x, Vector3 position)
    {
        if (generateNodes == false)
        {
            return;
        }
        //Instancia o nó
        var instance_node = Instantiate(pathNodePrefab, position, Quaternion.identity, GameObject.Find("Nodes").transform);
        instance_node.name = $"Node ({x}, {y})";
    }

    private void SpawnWallNode(float y, float x, Vector3 position)
    {
        if (generateNodes == false)
        {
            return;
        }
        try
        {
            //Instancia o wall
            var instance_wall = Instantiate(wallNodePrefab, position, Quaternion.identity, GameObject.Find("Walls").transform);
            instance_wall.name = $"Wall ({x}, {y})";

        }
        catch (System.Exception)
        {
            Debug.LogError("Wall prefab not found");
        }

    }

}
