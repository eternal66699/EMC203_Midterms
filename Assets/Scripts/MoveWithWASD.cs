using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithWASD : MonoBehaviour
{
    // Speed at which the object moves
    public float moveSpeed = 5f;

    void Update()
    {
        // Get input from WASD keys
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate the new position
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        // Move the object
        transform.Translate(move, Space.World);
    }
}

