using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blinky : MonoBehaviour
{
    // Public variables
    public float speed = 4f;

    // Private variables
    public MyNode currentNode;
    private string direction;
    public GameObject pacMan;
    private string currentState;
    private MyNode targetNode;
    //Priority decision
    List<string> priorityOrder = new List<string> { "up", "down", "left", "right" };
    public void Start()
    {
        direction = "left";
        currentState = "Chase";
        transform.position = currentNode.transform.position;
    }

    public void ChangeState(string state)
    {
        switch (state)
        {
            case "Scatter":
                currentState = "Scatter";
                break;
            case "Chase":
                currentState = "Chase";
                break;
            case "Frightened":
                currentState = "Frightened";
                break;
            case "Dead":
                currentState = "Dead";
                break;
            default:
                Debug.LogError("Invalid state: " + state);
                break;
        }
    }

    public void Update()
    {
        //Verify if we are in the center of the current node
        if (transform.position==currentNode.transform.position)
        {
            //Make a decision
            Debug.Log("Making a decision");

            //Normal behavior
            if (currentNode.GetNeighborByString(direction) != null && currentNode.GetComponent<CornerNode>() == null)
            {
                direction = currentNode.GetDirectionByNode(currentNode.GetNeighborByString(direction));
                currentNode = currentNode.GetNeighborByString(direction);
            }
            else if(currentNode.GetNeighborByString(direction) == null && currentNode.GetComponent<CornerNode>() == null)
            {
                //Take the direction that is not null and is not the reverse direction
                MyNode[] neighbors = currentNode.GetComponent<MyNode>().neighbors;
                var validDirections = neighbors.Where(x => x != null && x != currentNode.GetNeighborByString(direction.ReverseDirection()));
                direction = currentNode.GetDirectionByNode(validDirections.First());
                currentNode = currentNode.GetNeighborByString(direction);
            }
            
            //Decicion Behavior this overrides the normal behavior
            if (currentState == "Scatter")
            {
                Scatter();
            }
            else if (currentState == "Chase")
            {
                Chase();
            }
            else if (currentState == "Frightened")
            {
                Frightened();
            }
            else if (currentState == "Dead")
            {
                Dead();
            }

        }

        // Move the character towards the next node
        transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);
    }

    void Scatter()
    {
        // Implement Scatter behavior
        Debug.Log("Scatter mode");
    }
    void Chase()
    {
        if (currentNode.GetComponent<CornerNode>() == null)
        {
            return;
        }
        // Take all the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<CornerNode>().neighbors;

        // Using Linq To get the next node closest to the target with priority
        var nextNode = neighbors
        .Where(x => x != null && x != currentNode.GetNeighborByString(direction.ReverseDirection())) // Exclui direção reversa
        .GroupBy(x => x.HeuristicCost(pacMan)) // Agrupa pelos custos heurísticos
        .OrderBy(g => g.Key) // Ordena os grupos pelo custo heurístico
        .FirstOrDefault()
        ?.OrderBy(x => priorityOrder.IndexOf(currentNode.GetDirectionByNode(x))) // Desempata pela prioridade dentro do grupo
        .FirstOrDefault();

        // Atualiza a direção para o nó mais próximo
        if (nextNode != null&& transform.position == currentNode.transform.position)
        {
            Debug.Log("Next node: " + nextNode.name);
            direction = currentNode.GetDirectionByNode(nextNode);
            currentNode = nextNode;
        }
    }
    void Frightened()
    {
        // Implement Frightened behavior
        Debug.Log("Frightened mode");
    }
    void Dead()
    {
        // Implement Dead behavior
        Debug.Log("Dead mode");
    }
}

// Extension method to reverse the direction string
public static class StringExtensions
{
    public static string ReverseDirection(this string direction)
    {
        switch (direction)
        {
            case "up":
                return "down";
            case "down":
                return "up";
            case "left":
                return "right";
            case "right":
                return "left";
            default:
                return "";
        }
    }
}


