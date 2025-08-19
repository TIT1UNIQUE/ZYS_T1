using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClockBehaviour : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, ICancelHandler
{
    bool _on;
    bool _in;
    public RectTransform minuteArrow;
    public RectTransform hourArrow;
    public float timeInHourMin = 0;
    public float timeInHourMax = 24;
    private float _timeInHour;
    private Vector2 _startPos;
    private float _lastRadian;
    private Vector2 _lastPos;

    public TimeflowScene[] timeflowScenes;

    public UnityEvent evtEndTime;
    void Start()
    {
        SetTime(0);
    }

    void SetTime(float t)
    {
        _timeInHour = Mathf.Clamp(t, timeInHourMin, timeInHourMax);
        minuteArrow.localEulerAngles = new Vector3(0, 0, -_timeInHour * 360f + 90);
        hourArrow.localEulerAngles = new Vector3(0, 0, -_timeInHour * 30f + 90);
        foreach (var s in timeflowScenes)
        {
            s.Tick(_timeInHour);
        }
        if (_timeInHour == timeInHourMax)
        {
            evtEndTime?.Invoke();
        }
    }

    void AddTime(float d)
    {
        _timeInHour += d;
        SetTime(_timeInHour);
    }

    [SerializeField] Camera cam;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_in)
        {
            _on = true;
            _startPos = eventData.position;
            //Debug.Log(_startPos);
            var r = GetClockRadian(_startPos);
            _lastRadian = r;
            //Debug.Log("_startRadian " + _lastRadian);
            //Debug.Log(PointerDataToRelativePos(eventData));
        }
    }

    private Vector2 PointerDataToRelativePos(PointerEventData eventData)
    {
        Vector2 result;
        Vector2 clickPosition = eventData.position;
        RectTransform thisRect = GetComponent<RectTransform>();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(thisRect, clickPosition, null, out result);
        result += thisRect.sizeDelta / 2;

        return result;
    }

    float GetClockRadian(float y, float x)
    {
        var r = Mathf.Atan2(y, x);
        return 0.5f * Mathf.PI - r;
    }

    void Update()
    {
        var pos = Input.mousePosition;
        if (!_on)
            return;

        if ((Vector2)pos == _lastPos)
            return;

        _lastPos = pos;
        var r = GetClockRadian(_lastPos);
        var lastT = _lastRadian / (Mathf.PI * 2);
        var t = r / (Mathf.PI * 2);
        _lastRadian = r;
        //Debug.Log("r " + r);
        //Debug.Log("t " + t);
        //SetTime(t);
        AddTime(t - lastT);

    }

    float GetClockRadian(Vector3 screenPointerPos)
    {
        var centerScreenPos = cam.WorldToScreenPoint(minuteArrow.transform.position);
        var deltaPos = new Vector2(screenPointerPos.x - centerScreenPos.x, screenPointerPos.y - centerScreenPos.y);
        var r = GetClockRadian(deltaPos.y, deltaPos.x);
        while (_lastRadian - Mathf.PI >= r)
            r += Mathf.PI * 2;
        while (_lastRadian + Mathf.PI <= r)
            r -= Mathf.PI * 2;
        return r;
    }

    public void OnPointerMove(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _in = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _in = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _on = false;
    }

    public void OnCancel(BaseEventData eventData)
    {
        _on = false;
    }
}
