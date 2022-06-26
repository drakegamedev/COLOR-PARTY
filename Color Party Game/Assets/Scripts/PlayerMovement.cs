using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move Up
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            this.transform.position += new Vector3(0, movementSpeed, 0) * Time.deltaTime;
        }

        // Move Left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.transform.position -= new Vector3(movementSpeed, 0, 0) * Time.deltaTime;
        }

        // Move Down
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            this.transform.position -= new Vector3(0, movementSpeed, 0) * Time.deltaTime;
        }

        // Move Right
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            this.transform.position += new Vector3(movementSpeed, 0, 0) * Time.deltaTime;
        }
    }
}
