using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   public static AudioManager Instance;
   public static float FXVolume=1;
   public static float NarratorVolume=1;
   public static float MusicVolume=1;

   private void Awake() {
      if (Instance!=null&&Instance != this) {
         Destroy(this);
         Debug.LogWarning("AUDIO MANAGER: Deux Singlthon dans la même Scène");
      }
      else {
         Instance = this;
      }
   }

   public void PlaySound(AudioClip clip, float volume) {
      AudioSource audioSource = transform.AddComponent<AudioSource>();
      audioSource.clip = clip;
      audioSource.volume = FXVolume * volume;
      audioSource.Play();
      Destroy(audioSource , clip.length);
   }
   public void Playnarrator(AudioClip clip, float volume) {
      AudioSource audioSource = transform.AddComponent<AudioSource>();
      audioSource.clip = clip;
      audioSource.volume = NarratorVolume * volume;
      audioSource.Play();
      Destroy(audioSource , clip.length);
   }
}
