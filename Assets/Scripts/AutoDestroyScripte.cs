using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyScripte : MonoBehaviour
{
    public float DestroyDelay=2;
    private void Start()
    {
        Destroy(gameObject,DestroyDelay);
    }

}
