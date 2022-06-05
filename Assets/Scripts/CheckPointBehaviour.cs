using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || transform.position == RespawnManager.respawPos) return;
        RespawnManager.UpdateRespawn(transform.position);
    }
}
