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
    public GameObject pacman;
    public float tileSize = 0.5f;
    public enum GhostState
    {
        Chase,
        Scatter,
        Frightened,
        Dead,
    }
    protected List<string> priorityOrder = new List<string> { "up", "down", "left", "right" };
    public GhostState currentState;
    [SerializeField] protected MyNode scatterNode;
    [Header("Animation")]
    public Animator animator;

    public virtual void Start()
    {
        //Can be overriden by child classes
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

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        //Change the bool parameter in the animator
        //Reset the other bool parameters
        switch (direction)
        {
            case "up":
                animator.SetBool("is_Down", false);
                animator.SetBool("is_Left", false);
                animator.SetBool("is_Right", false);
                animator.SetBool("is_Up", true);
                break;
            case "down":
                animator.SetBool("is_Up", false);
                animator.SetBool("is_Left", false);
                animator.SetBool("is_Right", false);
                animator.SetBool("is_Down", true);
                break;
            case "left":
                animator.SetBool("is_Up", false);
                animator.SetBool("is_Down", false);
                animator.SetBool("is_Right", false);
                animator.SetBool("is_Left", true);
                break;
            case "right":
                animator.SetBool("is_Up", false);
                animator.SetBool("is_Down", false);
                animator.SetBool("is_Left", false);
                animator.SetBool("is_Right", true);
                break;
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
        //Update the direction and the current node




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
    /// Seleciona o vizinho ótimo com base no critério fornecido (nó alvo ou posição).
    /// </summary>
    /// <param name="neighbors">Array de nós vizinhos.</param>
    /// <param name="targetNodePosition">Posição do nó alvo.</param>
    /// <param name="heuristicFunc">Função heurística para calcular o custo.</param>
    /// <returns>O nó vizinho ótimo.</returns>
    public virtual MyNode SelectOptimalNeighbor(MyNode[] neighbors, System.Func<MyNode, float> heuristicFunc)
    {
        return neighbors
            .Where(x => x != null && x != currentNode.GetNeighborByString(direction.ReverseDirection()))
            .GroupBy(heuristicFunc) // Agrupa pelos custos heurísticos calculados
            .OrderBy(g => g.Key) // Ordena os grupos pelo custo heurístico
            .FirstOrDefault() // Pega o grupo com menor custo heurístico
            ?.OrderBy(x => priorityOrder.IndexOf(currentNode.GetDirectionByNode(x))) // Desempata pela prioridade dentro do grupo
            .FirstOrDefault(); // Pega o primeiro nó do grupo baseado na prioridade
    }

    /// <summary>
    /// Select the optimal neighbor based on a target node object position.
    /// </summary>
    /// <param name="neighbors"></param>
    /// <param name="targetNode"></param>
    /// <returns>Optimal Choice</returns>
    public MyNode SelectOptimalNeighborByNode(MyNode[] neighbors, MyNode targetNode)
    {
        return SelectOptimalNeighbor(neighbors,
            neighbor => neighbor.HeuristicCost(targetNode.gameObject.transform.position));
    }

    /// <summary>
    /// Select the optimal neighbor based on a global world position.
    /// </summary>
    /// <param name="neighbors"></param>
    /// <param name="targetNodePosition"></param>
    /// <returns>Optimal Choice</returns>
    public MyNode SelectOptimalNeighborByDistance(MyNode[] neighbors, Vector3 targetNodePosition)
    {
        return SelectOptimalNeighbor(neighbors,
            neighbor => neighbor.HeuristicCost(targetNodePosition));
    }
}
