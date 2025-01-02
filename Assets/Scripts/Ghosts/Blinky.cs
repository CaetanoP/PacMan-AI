using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blinky : Ghost
{
    public GameObject pacman;
    protected override void Chase()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;

        if (pacman != null)
        {
            Debug.Log("Pinky Chase");
            //Verifica se o current node do pacman é nulo
            if (pacman.GetComponent<PacMan>().currentNode == null)
            {
                Debug.Log("Pacman current node is null");
                return;
            }
            //Seleciona o nó alvo
            MyNode nextNode = SelectOptimalNeighborByNode(neighbors, pacman.GetComponent<PacMan>().currentNode);
            // Atualiza a direção e o nó atual
            if (nextNode != null)
            {
                direction = currentNode.GetDirectionByNode(nextNode);
                currentNode = nextNode;
            }
        }
    }
}


