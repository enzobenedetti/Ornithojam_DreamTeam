using System;
using UnityEngine;

public class DeathBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnManager.GetRespawned();
        }
    }
}
