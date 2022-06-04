using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester1 : MonoBehaviour
{
    public List<AudioClip> Clips;
    [Range(0, 1)] public float Volume=1;

    public float Intervale=0.5f;

    private float _timer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= Intervale) {
            AudioManager.Instance.PlaySound(Clips[Random.Range(0,Clips.Count)],Volume);
            _timer = 0;
        }
    }
}
