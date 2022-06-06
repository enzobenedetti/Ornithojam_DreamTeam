using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptEndGame : MonoBehaviour
{
   public CanvasGroup CanvasGroup;
   public float timer1 = 2;
   public float timer2 = 5;


   private float _timer;
   private bool _isfading;

   public void Update()
   {
      _timer += Time.deltaTime;
      if (_timer > timer1 && !_isfading) {
         _isfading = true;
         CanvasGroup.DOFade(1, 2);
      }

      if (_timer > timer2) {
         SceneManager.LoadScene(0);
      }
   }
}
