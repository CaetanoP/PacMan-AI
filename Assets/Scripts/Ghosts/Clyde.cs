using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public sealed class Clyde : Ghost
{
    private MyNode[] neighbors;

    protected override void Chase()
    {
        //Take All the neighbors of the current node
        neighbors = currentNode.GetComponent<DecisionNode>().neighbors;
        if (math.abs(Vector3.Distance(pacman.transform.position, transform.position)) > 8 * tileSize)
        {
            //Act like blinky if far than 8 tiles
            if (pacman != null)
            {
                //Verifica se o current node do pacman é nulo
                if (pacman.GetComponent<PacMan>().currentNode == null)
                {
                    Debug.Log("Pacman current node is null");
                    return;
                }
                MyNode nextNode = SelectOptimalNeighborByNode(neighbors, pacman.GetComponent<PacMan>().currentNode);
                UpdateCurrentNode(nextNode);
            }
        }
        else
        {
            if (scatterNode != null)
            {
                MyNode nextNode = SelectOptimalNeighborByNode(neighbors, scatterNode);
                UpdateCurrentNode(nextNode);
            }
            else
            {
                Debug.LogError("scatterNode not found! add the node in inspector");
            }
        }

    }
    protected override void Scatter()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;
        Vector3 customTarget = scatterNode.transform.position + new Vector3(0, -2 * tileSize, 0);
        MyNode nextNode = SelectOptimalNeighborByDistance(neighbors, customTarget);
        UpdateCurrentNode(nextNode);
    }
}
