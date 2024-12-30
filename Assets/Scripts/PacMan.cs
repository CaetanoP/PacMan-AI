using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Barracuda;

public class PacMan : Agent
{
    public string direction;
    public string previousDirection;
    public float speed = 8f;
    public MyNode currentNode;

    public override void OnEpisodeBegin()
    {



    }
    public void Start()
    {
        direction = "left";
    }
    public void Update()
    {

            // Move o personagem em direção ao nó atual
    transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

    // Verifica se estamos no centro do node atual
    if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y)
    {
        Debug.Log("Node atual: " + currentNode.name);

        // Define o próximo node com base na direção
        MyNode nextNode = null;

        if (currentNode.up != null && direction == "up")
        {
            nextNode = currentNode.up;
            previousDirection = "up";
        }
        else if (currentNode.down != null && direction == "down")
        {
            nextNode = currentNode.down;
            previousDirection = "down";
        }
        else if (currentNode.left != null && direction == "left")
        {
            nextNode = currentNode.left;
            previousDirection = "left";
        }
        else if (currentNode.right != null && direction == "right")
        {
            nextNode = currentNode.right;
            previousDirection = "right";
        }
        else
        {
            Debug.Log("Sem direção na direção solicitada, mantendo a direção anterior.");

            // Se a direção atual não for válida, continua na direção anterior
            if (previousDirection == "up" && currentNode.up != null)
            {
                nextNode = currentNode.up;
            }
            else if (previousDirection == "down" && currentNode.down != null)
            {
                nextNode = currentNode.down;
            }
            else if (previousDirection == "left" && currentNode.left != null)
            {
                nextNode = currentNode.left;
            }
            else if (previousDirection == "right" && currentNode.right != null)
            {
                nextNode = currentNode.right;
            }
        }

        // Atualiza o nó atual, se houver um próximo nó válido
        if (nextNode != null)
        {
            currentNode = nextNode;
        }
    }

    }
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Interpretar as ações recebidas
        var actions = actionBuffers.DiscreteActions;
        int moveDirection = actions[0];
        switch (moveDirection)
        {
            case 0: // Para baixo
                direction = "down";
                break;
            case 1: // Para cima
                direction = "up";
                break;
            case 2: // Para direita
                direction = "right";
                break;
            case 3: // Para esquerda
                direction = "left";
                break;
        }
    }

    public void OnEpisodeEnd()
    {
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("Heuristic");
        var discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActions[0] = 1; // Mover para cima
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            discreteActions[0] = 0; // Mover para baixo
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            discreteActions[0] = 2; // Mover para direita
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            discreteActions[0] = 3; // Mover para esquerda
        }
        else
        {
            discreteActions[0] = -1; // Sem movimento
        }


    }
}
