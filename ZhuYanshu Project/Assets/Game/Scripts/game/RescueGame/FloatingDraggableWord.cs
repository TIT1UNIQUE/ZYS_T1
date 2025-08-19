using com;
using Rescue;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloatingDraggableWord : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    Canvas canvas;

    RectTransform _rectTrans;
    public RectTransform constrainRect;
    bool _floating;
    public float speed;

    public float floatingTurnTimeMin = 0.5f;
    public float floatingTurnTimeMax = 2f;
    float _floatingTurnTimer;
    public float floatingTurnAngleMin = 120f;
    public float floatingTurnAngleMax = 240f;
    float _floatingAngle;
    Vector2 _goodAnchoredPos;
    public UnityEvent endDragCallback;
    public UnityEvent endPuzzleCallback;
    private void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
        GetComponent<Image>().raycastTarget = true;
        _floating = false;

    }

    public void StartFloat()
    {
        _goodAnchoredPos = _rectTrans.anchoredPosition;
        StartFirstFloatTurn();
        _floating = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SoundSystem.instance.Play("click");
        GetComponent<Image>().raycastTarget = false;
        _floating = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTrans.anchoredPosition += eventData.delta / canvas.scaleFactor;
        LimitPosition();
    }

    void LimitPosition()
    {
        var x = _rectTrans.anchoredPosition.x;
        var y = _rectTrans.anchoredPosition.y;
        var w = constrainRect.sizeDelta.x * 0.5f;
        var h = constrainRect.sizeDelta.y * 0.5f;
        x = Mathf.Clamp(x, -w, w);
        y = Mathf.Clamp(y, -h, h);
        _rectTrans.anchoredPosition = new Vector2(x, y);
    }

    void StartFirstFloatTurn()
    {
        //randomly distribute word position
        var w = constrainRect.sizeDelta.x * 0.5f;
        var h = constrainRect.sizeDelta.y * 0.5f;
        _rectTrans.anchoredPosition = new Vector2(Random.Range(-w, w), Random.Range(-h, h));
        //initialize turn parameters
        _floatingTurnTimer = Random.Range(floatingTurnTimeMin, floatingTurnTimeMax);
        _floatingAngle = Random.Range(0, 360);
    }

    void StartNewFloatTurn()
    {
        _floatingTurnTimer = Random.Range(floatingTurnTimeMin, floatingTurnTimeMax);
        _floatingAngle += Random.Range(floatingTurnAngleMin, floatingTurnAngleMax);
    }

    void Float()
    {
        _floatingTurnTimer -= Time.deltaTime;
        if (_floatingTurnTimer < 0)
        {
            StartNewFloatTurn();
        }

        var radian = _floatingAngle * Mathf.Deg2Rad;
        var x = Mathf.Sin(radian);
        var y = Mathf.Cos(radian);

        _rectTrans.anchoredPosition += new Vector2(x, y) * speed;

        LimitPosition();
    }

    void Update()
    {
        if (_floating)
            Float();
    }


    public float correctThreshold = 1f;
    public void OnEndDrag(PointerEventData eventData)
    {
        endDragCallback?.Invoke();

        var deltaPos = _rectTrans.anchoredPosition - _goodAnchoredPos;
        var dist = deltaPos.magnitude;
        //Debug.Log("距离 " + dist);

        if (dist <= correctThreshold)
        {
            SoundSystem.instance.Play("cloth picked");
            this.enabled = false;
            GetComponent<Image>().raycastTarget = false;

            endPuzzleCallback?.Invoke();
            return;
        }

        GetComponent<Image>().raycastTarget = true;
        _floating = true;
    }
}