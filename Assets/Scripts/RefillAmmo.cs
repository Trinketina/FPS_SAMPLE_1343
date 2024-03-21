using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RefillAmmo : MonoBehaviour
{
    FPSController player;
    private void Start()
    {
        player = FindObjectOfType<FPSController>();
    }
    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.layer == 6)
        {
            other.GetComponent<FPSController>().PlayerInteract += Refill;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            other.GetComponent<FPSController>().PlayerInteract -= Refill;
        }
    }

    public void Refill()
    {
        Debug.Log("Refilled");
        player.IncreaseAmmo(999);
    }
}
