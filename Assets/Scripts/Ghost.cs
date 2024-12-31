using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class Ghost : MonoBehaviour
{
    [Header("Ghost Attributes")]
    public float speed;
    protected string direction;
    public MyNode currentNode;
    public enum GhostState
    {
        Chase,
        Scatter,
        Frightened,
        Dead,
    }
    protected List<string> priorityOrder = new List<string> { "up", "down", "left", "right" };
    public GhostState currentState;
    protected MyNode scatterNode;

    public virtual void Start()
    {
        //Can be overriden by child classes
        speed = 5f;
        direction = "lef";
        currentState = GhostState.Chase;
    }
    public virtual void Update()
    {
        //Move towards the current node
        MoveToCurrentNode();

        //Update the node only when the character is on the node
        if (IsNearNode())
        {
            //If the current node is a path node, advance to the next node
            if (IsPathNode())
            {
                AdvanceToNextNode();
            }
            //If the current node is a decision node, make a decision
            else if (IsDecisionNode())
            {
                //Make a decision based on the current state
                if (currentState == GhostState.Chase)
                {
                    Chase();
                }
                else if (currentState == GhostState.Scatter)
                {
                    Scatter();
                }
                else if (currentState == GhostState.Frightened)
                {
                    Frightened();
                }
                else if (currentState == GhostState.Dead)
                {
                    Dead();
                }
            }
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
    private bool IsPathNode()
    {
        return currentNode.GetComponent<PathNode>() != null;
    }
    private bool IsDecisionNode()
    {
        return currentNode.GetComponent<DecisionNode>() != null;
    }


    private void AdvanceToNextNode()
    {
        // Tenta obter o próximo nó com base na direção atual
        MyNode nextNode = currentNode.GetNeighborByString(direction);

        if (nextNode == null)
        {
            // Seleciona um vizinho válido que não seja na direção reversa
            nextNode = currentNode.neighbors
                .FirstOrDefault(n => n != null && n != currentNode.GetNeighborByString(direction.ReverseDirection()));

            // Atualiza a direção para o novo nó, se encontrado
            if (nextNode != null)
            {
                direction = currentNode.GetDirectionByNode(nextNode);
            }
        }

        // Atualiza o currentNode para o próximo nó encontrado
        if (nextNode != null)
        {
            currentNode = nextNode;
        }
    }

    /// <summary>
    /// Para se mover é necessário alterar a variável currentNode
    /// </summary>
    protected virtual void MoveToCurrentNode()
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
    protected virtual void Scatter()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;

        //Select the optimal neighbor based on the scatter node
        currentNode = SelectOptimalNeighbor(neighbors, scatterNode);

    }

    protected virtual void Chase()
    {

    }
    protected virtual void Frightened()
    {
        //Take All the neighbors of the current node
        MyNode[] neighbors = currentNode.GetComponent<DecisionNode>().neighbors;

        //Remove the reverse direction from the neighbors
        var validDirections = neighbors.Where(x => x != null && x != currentNode.GetNeighborByString(direction.ReverseDirection()));


        //select a random node from the valid directions
        MyNode nextNode = validDirections.ElementAt(Random.Range(0, validDirections.Count()));
        //Atualiza a direção e o nó atual
        if (nextNode != null)
        {
            direction = currentNode.GetDirectionByNode(nextNode);
            currentNode = nextNode;
        }

    }
    public void Dead()
    {

    }

    /// <summary>
    /// Seleciona o vizinho ótimo com base no nó alvo
    /// </summary>
    /// <param name="neighbors"></param>
    /// <param name="targetNode"></param>
    /// <returns></returns>
    public virtual MyNode SelectOptimalNeighbor(MyNode[] neighbors, MyNode targetNode)
    {
        var optimalNeighbor = neighbors
        .Where(x => x != null && x != currentNode.GetNeighborByString(direction.ReverseDirection()))
        .GroupBy(x => x.HeuristicCost(targetNode.gameObject)) // Agrupa pelos custos heurísticos
        .OrderBy(g => g.Key) // Ordena os grupos pelo custo heurístico
        .FirstOrDefault() // Pega o grupo com menor custo heurístico
        ?.OrderBy(x => priorityOrder.IndexOf(currentNode.GetDirectionByNode(x)))// Desempata pela prioridade dentro do grupo
        .FirstOrDefault();// Pega o primeiro nó do grupo Baseado na prioridade

        return optimalNeighbor;
    }
}
