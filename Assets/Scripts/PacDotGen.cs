using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDotGen : MonoBehaviour
{
   public GameObject pacDotPrefab; // Prefab do Pac-Dot
    public GameObject wallParent;  // Objeto pai que contém as paredes
    public Vector2 gridSize;      // Tamanho do labirinto (x, y)
    public float cellSize = 0.5f;   // Tamanho da célula do grid
    public Vector2 offset;         // Offset do grid
    void Start()
    {
        SpawnPacDots();
    }

    void SpawnPacDots()
    {
        float X = transform.position.x -  offset.x;
        float Y = transform.position.y -  offset.y;
        for (float x = 0; x < gridSize.x; x += 1)
        {
            Y = transform.position.y -  offset.y;
            for (float y = 0; y < gridSize.y; y += 1)
            {
                Vector3 position = new Vector3(X, Y, 0);

                // Verifica se existe uma parede no local
                Collider2D hit = Physics2D.OverlapCircle(new Vector2(X,Y), 0.1f);
                if (hit == null || !hit.CompareTag("Wall"))
                {
                    // Instancia o Pac-Dot se não houver uma parede
                    var instance = Instantiate(pacDotPrefab, position, Quaternion.identity, transform);
                    instance.name = $"Pac-Dot ({x}, {y})";
                }
                Y += cellSize;
            }
            X += cellSize;
        }
    }
}
