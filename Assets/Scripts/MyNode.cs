using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyNode : MonoBehaviour
{
    public MyNode up;
    public MyNode down;
    public MyNode left;
    public MyNode right;


    public void Start()
    {
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, 0.5f);
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, 0.5f);
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, 0.5f);
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, 0.5f);
        for(int i = 0; i < hitDown.Length; i++)
        {
            if(hitDown[i].collider.CompareTag("Node"))
            {
                down = hitDown[i].collider.GetComponent<MyNode>();
            }
        }
        for(int i = 0; i < hitUp.Length; i++)
        {
            if(hitUp[i].collider.CompareTag("Node"))
            {
                up = hitUp[i].collider.GetComponent<MyNode>();
            }
        }
        for(int i = 0; i < hitLeft.Length; i++)
        {
            if(hitLeft[i].collider.CompareTag("Node"))
            {
                left = hitLeft[i].collider.GetComponent<MyNode>();
            }
        }
        for(int i = 0; i < hitRight.Length; i++)
        {
            if(hitRight[i].collider.CompareTag("Node"))
            {
                right = hitRight[i].collider.GetComponent<MyNode>();
            }
        }

    }
}
