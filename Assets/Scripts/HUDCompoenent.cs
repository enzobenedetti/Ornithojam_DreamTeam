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

    
    public Sprite Key , KeyPress, Space, SpacePress, Mouse, MousePress;
    public TMP_Text TxtNarrator;
    public CanvasGroup CanvasGroupNarrator;
    public float NarratorFadeOutTime = 2f;



    public void PressZ(bool value) => ImgZ.sprite = value ? KeyPress : Key;
    public void PressQ(bool value)=> ImgQ.sprite = value ? KeyPress :Key;
    public void PressS(bool value)=> ImgS.sprite = value ? KeyPress :Key;
    public void PressD(bool value)=> ImgD.sprite = value ? KeyPress :Key;
    public void PressSpace(bool value)=> ImgJump.sprite = value ? SpacePress : Space;
    public void PressGrabe(bool value)=> ImgGrabe.sprite = value ? MousePress : Mouse;

    public void PlayNarrator(string text ,float time)
    {
        TxtNarrator.text = text;
        CanvasGroupNarrator.DOPause();
        CanvasGroupNarrator.alpha = 1;
        Invoke("FadeOutNarratorText",time);
    }

    private void FadeOutNarratorText() {
        CanvasGroupNarrator.DOFade(0, NarratorFadeOutTime);
    }
    
}
