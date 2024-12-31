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
        MoveToCurrentNode();
        // Atualiza o nó apenas quando o personagem está no nó
        if (IsNearNode())
            UpdateCurrentNode();
        // Atualiza a rotação do personagem com base na direção
        UpdateRotation();
    }

    /// <summary>
    /// Move o personagem em direção ao nó atual se não estiver muito longe,
    /// caso contrário, o personagem é teletransportado para o nó atual.
    /// </summary>
    private void MoveToCurrentNode()
    {
        if (!IsFarFromNode())
        {
            transform.position = Vector3.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = currentNode.transform.position;
        }
    }

    private bool IsFarFromNode()
    {
        return Vector2.Distance(transform.position, currentNode.transform.position) > 1f;
    }
    private bool IsNearNode()
    {
        return transform.position == currentNode.transform.position;
    }

    private void UpdateCurrentNode()
    {
        // Tenta obter o próximo nó com base na direção atual
        MyNode nextNode = currentNode.GetNeighborByString(direction);

        if (nextNode != null)
        {
            // Atualiza o nó e armazena a direção como a última usada
            currentNode = nextNode;
            previousDirection = direction;
        }
        else
        {
            // Se não há um nó na direção atual, tenta continuar na direção anterior
            nextNode = currentNode.GetNeighborByString(previousDirection);
            if (nextNode != null)
            {
                currentNode = nextNode;
            }
        }
    }

    private void UpdateRotation()
    {
        float angle = previousDirection switch
        {
            "up" => 90,
            "down" => -90,
            "left" => 180,
            "right" => 0,
            _ => 0
        };

        transform.rotation = Quaternion.Euler(0, 0, angle);
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PacDot"))
        {
            AddReward(1f);
            Destroy(collision.gameObject);
        }
    }

}
