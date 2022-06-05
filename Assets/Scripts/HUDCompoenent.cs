using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class HUDCompoenent : MonoBehaviour
{
    [CanBeNull] public Image ImgZ, ImgQ, ImgS, ImgD, ImgGrabe, ImgJump;

    
    public Sprite Z, Zpress, Q, Qpress, S, SPress, D, DPress, Space, SpacePress, Grabe, GrabePress;
    public TMP_Text TxtNarrator;
    public CanvasGroup CanvasGroupNarrator;
    public float NarratorFadeOutTime = 2f;



    public void PressZ(bool value)=> ImgZ.sprite = value ? Color.gray : Color.white
    public void PressQ(bool value)=> ImgQ.sprite = value ? Color.gray : Color.white;
    public void PressS(bool value)=> ImgS.sprite = value ? Color.gray : Color.white;
    public void PressD(bool value)=> ImgD.sprite = value ? Color.gray : Color.white;
    public void PressSpace(bool value)=> ImgJump.color = value ? Color.gray : Color.white;
    public void PressGrabe(bool value)=> ImgGrabe.color = value ? Color.gray : Color.white;

    public void PlayNarrator(string text ,float time)
    {
        TxtNarrator.text = text;
        CanvasGroupNarrator.DOPause();
        CanvasGroupNarrator.alpha = 1;
        Invoke("FadeOutNarratorText",Mathf.Clamp(0,1000 , time-NarratorFadeOutTime));
    }

    private void FadeOutNarratorText() {
        CanvasGroupNarrator.DOFade(0, NarratorFadeOutTime);
    }
    
}
