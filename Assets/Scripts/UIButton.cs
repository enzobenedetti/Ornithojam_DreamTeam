using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour ,IPointerEnterHandler ,IPointerExitHandler, ISelectHandler , IDeselectHandler
{
    public float AnimationTime=0.2f;
    public float AnimationSize=1.1f;
    public Vector3 AnimationRotation = new Vector3(0, 0, 2);
    public AnimationCurve AnimationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    public bool IsSelectable;
    

    

    public void OnPointerEnter(PointerEventData eventData) {
        if (IsSelectable)
        {
            transform.DOPause();
            transform.localScale = Vector3.one;
            transform.eulerAngles = Vector3.zero;
            transform.DOScale(AnimationSize, AnimationTime).SetEase(AnimationCurve);
            transform.DORotate(AnimationRotation, AnimationTime).SetEase(AnimationCurve);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (IsSelectable)
        {
            transform.DOPause();
            transform.localScale = Vector3.one * AnimationSize;
            transform.eulerAngles = AnimationRotation;
            transform.DOScale(Vector3.one, AnimationTime).SetEase(AnimationCurve);
            transform.DORotate(Vector3.zero, AnimationTime).SetEase(AnimationCurve);
        }
    }

    public void OnSelect(BaseEventData eventData) {
        if (IsSelectable)
        {
            transform.DOPause();
            transform.localScale = Vector3.one;
            transform.eulerAngles = Vector3.zero;
            transform.DOScale(AnimationSize, AnimationTime).SetEase(AnimationCurve);
            transform.DORotate(AnimationRotation, AnimationTime).SetEase(AnimationCurve);
        }
    }

    public void OnDeselect(BaseEventData eventData) {
        if (IsSelectable)
        {
            transform.DOPause();
            transform.localScale = Vector3.one * AnimationSize;
            transform.eulerAngles = AnimationRotation;
            transform.DOScale(Vector3.one, AnimationTime).SetEase(AnimationCurve);
            transform.DORotate(Vector3.zero, AnimationTime).SetEase(AnimationCurve);
        }
    }
}
