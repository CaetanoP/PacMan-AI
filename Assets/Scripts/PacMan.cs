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
    public GameObject pacArrow;

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
        if (transform.position == currentNode.transform.position)
        {
            // Define os possíveis próximos nós com base nas direções
            Dictionary<string, MyNode> directions = new Dictionary<string, MyNode>
        {
            { "up", currentNode.up },
            { "down", currentNode.down },
            { "left", currentNode.left },
            { "right", currentNode.right }
        };

            // Tenta definir o próximo nó com base na direção atual
            if (directions.TryGetValue(direction, out MyNode nextNode) && nextNode != null)
            {
                currentNode = nextNode;
                previousDirection = direction;
            }
            else
            {

                // Continua na direção anterior, se válida
                if (directions.TryGetValue(previousDirection, out nextNode) && nextNode != null)
                {
                    currentNode = nextNode;
                }
            }
        }

        switch (previousDirection)
        {
            case "up":
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case "down":
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case "left":
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case "right":
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
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
