using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inky : Ghost
{
    public GameObject pacman;
    public GameObject blinky;
    public float tileSize = 0.5f;
    protected override void Chase()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;

        if (pacman != null)
        {
            //Verifica se o current node do pacman é nulo
            if (pacman.GetComponent<PacMan>().currentNode == null)
            {
                Debug.Log("Pacman current node is null");
                return;
            }
            switch (pacman.GetComponent<PacMan>().previousDirection)
            {
                case "up":
                    MyNode nextNode = SelectOptimalNeighborByDistance(neighbors, InkyTargetPosition(Vector3.up));
                    UpdateCurrentNode(nextNode);
                    break;

                case "down":
                    nextNode = SelectOptimalNeighborByDistance(neighbors, InkyTargetPosition(Vector3.down));
                    UpdateCurrentNode(nextNode);
                    break;

                case "left":
                    nextNode = SelectOptimalNeighborByDistance(neighbors, InkyTargetPosition(Vector3.left));
                    UpdateCurrentNode(nextNode);
                    break;

                case "right":
                    nextNode = SelectOptimalNeighborByDistance(neighbors, InkyTargetPosition(Vector3.right));
                    UpdateCurrentNode(nextNode);
                    break;
            }
        }
    }

    /// <summary>
    /// Return the target position for Inky based on the direction of Pacman and the distance from Blinky
    /// </summary>
    /// <param name="pacDirection"></param>
    /// <returns></returns>
    public Vector3 InkyTargetPosition(Vector3 pacDirection)
    {
        Vector3 targetPosition;
        if (pacDirection == Vector3.up)
        {
            targetPosition = pacman.GetComponent<PacMan>().transform.position + (pacDirection + Vector3.left) * 2 * tileSize;
        }
        else
        {
            targetPosition = pacman.GetComponent<PacMan>().transform.position + pacDirection * 2 * tileSize;
        }
        //Vector3 targetPosition = pacman.GetComponent<PacMan>().transform.position + pacDirection * 2 * tileSize;
        Vector3 blinkyToTarget = targetPosition - blinky.transform.position;
        targetPosition += blinkyToTarget * 2;
        return targetPosition;
    }
    /// <summary>
    /// Update the current node and direction
    /// </summary>
    /// <param name="nextNode"></param>
    public void UpdateCurrentNode(MyNode nextNode)
    {
        direction = currentNode.GetDirectionByNode(nextNode);
        currentNode = nextNode;

    }
}

