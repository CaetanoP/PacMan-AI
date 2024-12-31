using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyNode : MonoBehaviour
{
    [Header("Neighbors")]
    public MyNode up;
    public MyNode down;
    public MyNode left;
    public MyNode right;
    public MyNode[] neighbors;
    [Header("Pac")] public GameObject closestPacMan;
    [Header("Costs")]
    public float pathCost = 1;
    public void Start()
    {
        FindNeighborNodes();
        FindPacman();
        neighbors = new MyNode[] { up, down, left, right };
    }

    private void FindNeighborNodes()
    {
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, 0.5f);
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 0.5f);
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 0.5f);
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, 0.5f);
        for (int i = 0; i < hitDown.Length; i++)
        {
            if (hitDown[i].collider.CompareTag("Node"))
            {
                down = hitDown[i].collider.GetComponent<MyNode>();
            }
        }
        for (int i = 0; i < hitUp.Length; i++)
        {
            if (hitUp[i].collider.CompareTag("Node"))
            {
                up = hitUp[i].collider.GetComponent<MyNode>();
            }
        }
        for (int i = 0; i < hitLeft.Length; i++)
        {
            if (hitLeft[i].collider.CompareTag("Node"))
            {
                left = hitLeft[i].collider.GetComponent<MyNode>();
            }
        }
        for (int i = 0; i < hitRight.Length; i++)
        {
            if (hitRight[i].collider.CompareTag("Node"))
            {
                right = hitRight[i].collider.GetComponent<MyNode>();
            }
        }
    }
    private void FindPacman()
    {
        closestPacMan = GameObject.Find("PacMan");
    }

    public float HeuristicCost(GameObject target)
    {
        return Vector2.Distance(transform.position, target.transform.position);
        //return Mathf.Abs(transform.position.x - target.transform.position.x) + Mathf.Abs(transform.position.y - target.transform.position.y);
    }

    public MyNode GetNeighborByString(string direction)
    {
        switch (direction)
        {
            case "up":
                return up;
            case "down":
                return down;
            case "left":
                return left;
            case "right":
                return right;
            default:
                return null;
        }
    }
    public string GetDirectionByNode(MyNode targetNode)
    {
        if (targetNode == up) return "up";
        if (targetNode == down) return "down";
        if (targetNode == left) return "left";
        if (targetNode == right) return "right";

        return null; // Caso não esteja conectado a esse nó
    }



}
