using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Pinky : Ghost
{
    public GameObject pacman;
    public float tileSize = 0.5f;
    protected override void Chase()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;
        //Select the optimal neighbor based on the target node
        if (pacman != null)
        {
            Debug.Log("Pinky Chase");
            //Verifica se o current node do pacman é nulo
            if (pacman.GetComponent<PacMan>().currentNode == null)
            {
                Debug.Log("Pacman current node is null");
                return;
            }
            switch (pacman.GetComponent<PacMan>().previousDirection)
            {
                case "up":
                    //Seleciona o nó alvo
                    MyNode nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + (Vector3.up+Vector3.left)*4*tileSize);
                    // Atualiza a direção e o nó atual
                    if (nextNode != null)
                    {
                        direction = currentNode.GetDirectionByNode(nextNode);
                        currentNode = nextNode;
                    }
                    break;
               case "down":
                    //Seleciona o nó alvo
                    nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + Vector3.down *4*tileSize);
                    // Atualiza a direção e o nó atual
                    if (nextNode != null)
                    {
                        direction = currentNode.GetDirectionByNode(nextNode);
                        currentNode = nextNode;
                    }
                    break;
                case "left":
                    //Seleciona o nó alvo
                    nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + Vector3.left *4*tileSize);
                    // Atualiza a direção e o nó atual
                    if (nextNode != null)
                    {
                        direction = currentNode.GetDirectionByNode(nextNode);
                        currentNode = nextNode;
                    }
                    break;
                case "right":
                    //Seleciona o nó alvo
                    nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + Vector3.right *4*tileSize);
                    // Atualiza a direção e o nó atual
                    if (nextNode != null)
                    {
                        direction = currentNode.GetDirectionByNode(nextNode);
                        currentNode = nextNode;
                    }
                    break;
            }


        }
    }
}
