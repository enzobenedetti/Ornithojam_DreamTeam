using System.Collections;
using System.Collections.Generic;
using System.Timers;
using DG.Tweening;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScripteIntro : MonoBehaviour
{
    public List<AudioClip> Audio;
    public List<Vector3> Pos;
    public AudioSource AudioSource;
    public Camera Camera;

    private int _index=0;
    private float _timer;
    private float length;
    void Start() {
        _timer += Time.deltaTime;
        Camera.transform.position = Pos[_index];
        AudioSource.clip = Audio[_index];
        AudioSource.Play();
        length = Audio[_index].length;
    }

    // Update is called once per frame
    void Update() {
        _timer += Time.deltaTime;
        if (_timer > length + 2) {
            _index++;
            if (_index == Audio.Count) {
                SceneManager.LoadScene(1);
            } 
            Camera.transform.DOMove(Pos[_index], 2);
            AudioSource.clip = Audio[_index];
            AudioSource.Play();
            length = Audio[_index].length;
            _timer = 0;
        }
    }
}
