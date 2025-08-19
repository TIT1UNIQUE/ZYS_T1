using System.Collections;
using TMPro;
using UnityEngine;
using com;

public class LiftSystem : MonoBehaviour
{
    public static LiftSystem instance;

    public LiftControlPanel liftControlPanel;

    private float _crtFloor;//rounded up for the int floor number
    public int currentFloor { get { return Mathf.RoundToInt(_crtFloor); } }

    public TextMeshProUGUI liftNumLabel;

    public float liftSpeed;
    private float _targetFloor;
    private bool _goDownDirection;

    LiftDoors _liftDoors;
    FloorSwitcher _floorSwitcher;

    public GameObject floor1Dark;
    public GameObject floor2Dark;
    public GameObject floor3Dark;
    public GameObject floor4Dark;
    public GameObject floor5Dark;

    public GameObject scp_floor1Light;
    public GameObject scp_floor2Light;
    public GameObject scp_floor3Light;
    public GameObject scp_floor4Light;
    public GameObject scp_floor5Light;
    public GameObject scp_RedLight;
    private void Awake()
    {
        instance = this;
        _liftDoors = GetComponent<LiftDoors>();
        _floorSwitcher = GetComponent<FloorSwitcher>();
    }

    private void Start()
    {
        _crtFloor = 5;
        _targetFloor = 5;

        _liftDoors.doorRight.Set(true, true);
        _liftDoors.doorLeft.Set(true, true);

        SyncLiftDisplayer();
        SyncFloorLight();
        SyncSmallControlPanel();
        StartCoroutine(SmallPanelRedLightBlinking());
    }

    private bool _lastIsLiftRunning = false;
    public bool IsLiftRunning
    {
        get
        {
            var delta = Mathf.Abs(_crtFloor - _targetFloor);
            //Debug.Log(delta);
            return delta > 0f;
        }
    }

    void SyncFloorLight()
    {
        floor1Dark.SetActive(currentFloor != 1);
        floor2Dark.SetActive(currentFloor != 2);
        floor3Dark.SetActive(currentFloor != 3);
        floor4Dark.SetActive(currentFloor != 4);
        floor5Dark.SetActive(currentFloor != 5);
    }

    private void Update()
    {
        if (IsLiftRunning)
        {
            Sync();
            _lastIsLiftRunning = true;
        }
        else
        {
            if (_lastIsLiftRunning)
            {
                _lastIsLiftRunning = false;
                Sync();
            }
        }
    }

    void Sync()
    {
        MoveLift();
        SyncLiftDisplayer();
        SyncFloorLight();
        SyncSmallControlPanel();
    }

    void MoveLift()
    {
        if (_liftDoors.DoorsClosedAndStopped())
        {
            _crtFloor += GameTime.deltaTime * liftSpeed * (_goDownDirection ? -1f : 1f);
            if (Mathf.Abs(_crtFloor - _targetFloor) <= 0.25f)
            {
                _crtFloor = _targetFloor;
                OnArrived();
            }
        }
    }

    void SyncLiftDisplayer()
    {
        liftNumLabel.text = currentFloor.ToString();
    }

    void SyncSmallControlPanel()
    {
        var b = IsLiftRunning;
        scp_floor1Light.SetActive(b && _targetFloor == 1);
        scp_floor2Light.SetActive(b && _targetFloor == 2);
        scp_floor3Light.SetActive(b && _targetFloor == 3);
        scp_floor4Light.SetActive(b && _targetFloor == 4);
        scp_floor5Light.SetActive(b && _targetFloor == 5);
    }

    IEnumerator SmallPanelRedLightBlinking()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.125f);
            if (LiftState_doorMoving())
                scp_RedLight.SetActive(!scp_RedLight.activeSelf);
            else if (LiftState_moving())
                scp_RedLight.SetActive(true);
            else
                scp_RedLight.SetActive(false);
        }
    }

    //open stop dark
    //door moving blinking
    //moving lit
    bool LiftState_moving()
    {
        return IsLiftRunning && _liftDoors.DoorsClosedAndStopped();
    }

    bool LiftState_idle()
    {
        return !IsLiftRunning && _liftDoors.DoorsOpenAndStopped();
    }

    bool LiftState_doorMoving()
    {
        return _liftDoors.DoorsMoving();
    }

    public bool lockLift;

    public void TryShowLiftControlPanel()
    {
        Debug.Log("TryShowLiftControlPanel");
        if (lockLift)
        {
            Debug.Log("lift locked!");
            return;
        }

        if (LiftState_idle())
            ShowLiftControlPanel();
    }

    public void ShowLiftControlPanel()
    {
        liftControlPanel.gameObject.SetActive(true);
        liftControlPanel.ResetBtns();
    }

    public void HideLiftControlPanel()
    {
        liftControlPanel.gameObject.SetActive(true);
    }

    public void OnLiftDestinationSet(int intTargetFloor)
    {
        if (intTargetFloor <= 0 || intTargetFloor > 5)
            return;

        _targetFloor = (float)intTargetFloor;
        var delta = _targetFloor - _crtFloor;
        var duration = Mathf.Abs(delta) / liftSpeed;
        _goDownDirection = delta < 0;

        _liftDoors.doorLeft.Set(false, false);
        _liftDoors.doorRight.Set(false, false);

        SoundSystem.instance.Play("tuto");
    }

    void OnArrived()
    {
        _liftDoors.doorLeft.Set(true, false);
        _liftDoors.doorRight.Set(true, false);
        _floorSwitcher.SetFloor(currentFloor);

        SoundSystem.instance.Play("pistol drop");
    }
}