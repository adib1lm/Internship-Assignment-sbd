using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
   [SerializeField] private bool hasCollided = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasCollided = true;
        //Debug.Log("col detected");
    }

    public void checkCollision()
    {
        if (hasCollided)
            Destroy(gameObject);
    }
}
