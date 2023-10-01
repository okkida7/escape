using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isNear = false; // A flag to check if player is near the door.
    public bool isOpen = false; // To check if door is already open or closed.
    private KeyController key;
    public Animator animator; // The animator component attached to the door.
    public Collider collider; // The collider component attached to the door.
    void Start()
    {
        key = GameObject.FindWithTag("Key").GetComponent<KeyController>();
    }
    void Update()
    {
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
        animator.SetBool("open", true);
        collider.enabled = false;
    }
}

