using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorTriggerComponent : MonoBehaviour
{

    [TextArea]
    public string Text;
    public AudioClip Clip;
    [Range(0, 1)] public float Volume = 1;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            float time = 10;
            if (Clip != null) {
                time = Clip.length;
                AudioManager.Instance.Playnarrator(Clip, Volume);
            }
            other.GetComponent<CharacterMovement>().HUDCompoenent.PlayNarrator(Text ,time);
            gameObject.SetActive(false);
        }
    }
}
