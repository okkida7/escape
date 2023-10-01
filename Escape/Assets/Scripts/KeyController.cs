using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyController : MonoBehaviour
{
    public bool isNear = false;
    public bool isPickedUp = false;
    public void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.Space) && !isPickedUp)
        {
            PickUpKey();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
        }
    }
    public void PickUpKey()
    {
        isPickedUp = true;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
