using com;
using Rescue;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableToPool : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    Canvas canvas;

    RectTransform _rectTrans;

    [SerializeField] RectTransform boyPoolDragTarget;
    [SerializeField] float boyPoolDragDistanceThreshold;
    Vector2 _startPos;

    private void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
        GetComponent<Image>().raycastTarget = true;
    }

    void Start()
    {
        _startPos = _rectTrans.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var dist = _rectTrans.anchoredPosition - boyPoolDragTarget.anchoredPosition;
        if (dist.magnitude > boyPoolDragDistanceThreshold)
        {
            _rectTrans.anchoredPosition = _startPos;
            GetComponent<Image>().raycastTarget = true;
            SoundSystem.instance.Play("pool fail");
        }
        else
        {
            SoundSystem.instance.Play("pool suc");
            RescueSystem.instance.OnDragToPoolPuzzleFinished();
        }
    }
}