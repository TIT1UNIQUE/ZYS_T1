using com;
using DG.Tweening;
using Rescue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCloth : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    Canvas canvas;

    RectTransform rectTrans;

    [SerializeField]
    RectTransform draggingParent;

    public DragDropContainer[] containers;
    [SerializeField]
    DragDropContainer _startDDC;
    Vector3 _startPos;

    [SerializeField]
    DraggableCloth clothInFront;

    public bool isFinalCloth;

    private Vector2 _startingAnchoredPos;

    public float distanceTolerance = 10;

    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _startingAnchoredPos = rectTrans.anchoredPosition;
        SetToDragDropContrainer(_startDDC);
    }


    void SetToDragDropContrainer(DragDropContainer ddc)
    {
        _startDDC = ddc;
        rectTrans.SetParent(ddc.transform);
        GetComponent<Image>().raycastTarget = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (clothInFront != null && clothInFront.gameObject.activeSelf)
            return;


        _startPos = rectTrans.anchoredPosition;
        GetComponent<Image>().raycastTarget = false;
        rectTrans.SetParent(draggingParent.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (clothInFront != null && clothInFront.gameObject.activeSelf)
            return;

        rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;

        var wfswb = GetComponent<WashFaceSwipeWoundBehaviour>();
        if (wfswb != null)
            wfswb.OnSwiping(rectTrans.anchoredPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragDropContainer endContainer = _startDDC;

        var distDir = _startingAnchoredPos - rectTrans.anchoredPosition;
        var dist = distDir.magnitude;
        Debug.Log(dist);
        if (dist > distanceTolerance)
        {
            SetToDragDropContrainer(endContainer);
            ApplyEndFeedback();
            //drop the cloth
        }
        else
        {
            SetToDragDropContrainer(endContainer);
            rectTrans.anchoredPosition = _startingAnchoredPos;
        }
    }

    public void ApplyEndFeedback()
    {
        if (isFinalCloth)
        {
            SoundSystem.instance.Play("cloth picked");
            this.enabled = false;
            RescueSystem.instance.FocusViewOnPickedCloth(rectTrans.anchoredPosition);
            return;
        }

        //drop
        SoundSystem.instance.Play("cloth drop");
        rectTrans.DOMoveY(-80, 2f).SetEase(Ease.InCubic).OnComplete(

            () =>
            {
                gameObject.SetActive(false);
            }
            );
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown ");
    }
}
