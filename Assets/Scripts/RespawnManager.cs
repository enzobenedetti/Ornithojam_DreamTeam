using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RespawnManager : MonoBehaviour
{
    public static Vector3 respawPos;
    public static UnityEvent Respawned = new UnityEvent();
    public static UnityEvent NewSpawn = new UnityEvent();

    private void Awake()
    {
        respawPos = FindObjectOfType<CharacterMovement>().transform.position;
    }

    public static void UpdateRespawn(Vector3 newPos)
    {
        respawPos = newPos;
        NewSpawn.Invoke();
    }

    public static void GetRespawned()
    {
        FindObjectOfType<CharacterMovement>().DoARespawn(respawPos);
        Respawned.Invoke();
    }
}
