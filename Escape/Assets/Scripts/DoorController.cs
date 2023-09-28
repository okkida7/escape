using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isNear = false; // A flag to check if player is near the door.
    private bool isOpen = false; // To check if door is already open or closed.
    private KeyController key;
    void Start()
    {
        key = GameObject.FindWithTag("Key").GetComponent<KeyController>();
    }
    void Update()
    {
        // Check if player is near and presses space
        if(key.isPickedUp)
        {
            Debug.Log("Key is picked up");
        }
        if (isNear && Input.GetKeyDown(KeyCode.Space) && !isOpen && key.isPickedUp)
        {
            OpenDoor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Assuming player has a tag named "Player"
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        Destroy(gameObject);
    }
}

