using System;
using UnityEngine;

public class DeathBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("nonoono");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Die!");
            RespawnManager.GetRespawned();
        }
    }
}
