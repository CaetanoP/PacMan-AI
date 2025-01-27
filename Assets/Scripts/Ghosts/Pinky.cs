using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Pinky : Ghost
{
    protected override void Chase()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;
        //Select the optimal neighbor based on the target node
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
                    MyNode nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + (Vector3.up + Vector3.left) * 4 * tileSize);
                    UpdateCurrentNode(nextNode);
                    break;

                case "down":
                    nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + Vector3.down * 4 * tileSize);
                    UpdateCurrentNode(nextNode);
                    break;

                case "left":
                    nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + Vector3.left * 4 * tileSize);
                    UpdateCurrentNode(nextNode);
                    break;

                case "right":
                    nextNode = SelectOptimalNeighborByDistance(neighbors, pacman.GetComponent<PacMan>().transform.position + Vector3.right * 4 * tileSize);
                    UpdateCurrentNode(nextNode);
                    break;
            }


        }
    }

    protected override void Scatter()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;
        Vector3 customTarget = scatterNode.transform.position + new Vector3(0, 4 * tileSize, 0);
        MyNode nextNode = SelectOptimalNeighborByDistance(neighbors, customTarget);
        UpdateCurrentNode(nextNode);
    }
}
