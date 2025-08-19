using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftControlPanel : MonoBehaviour
{
    //点击按钮，按钮亮
    //点击按钮，其它按钮暗
    //点击按钮，红灯亮
    //红灯亮，过一会暗

    public List<LiftControlPanelButton> buttons = new List<LiftControlPanelButton>();//列表

    public GameObject redLightOn;
    public GameObject exitBtn;

    private Coroutine _redLightCoroutine;
    public void ResetBtns()
    {
        OnButtonClicked(0);
        exitBtn.SetActive(false);
        if (_redLightCoroutine != null)
            StopCoroutine(_redLightCoroutine);
    }

    public void OnButtonClicked(int btnFloor)
    {
        Debug.Log("按下了button floor " + btnFloor + " currentFloor " + LiftSystem.instance.currentFloor);

        var sameFloor = btnFloor == LiftSystem.instance.currentFloor;
        foreach (LiftControlPanelButton btn in buttons)
        {
            if (btnFloor == btn.floor && !sameFloor)
            {
                btn.btnLight.SetActive(true);
                btn.btnHalo.SetActive(true);
            }
            else
            {
                btn.btnLight.SetActive(false);
                btn.btnHalo.SetActive(false);
            }
        }

        if (sameFloor || btnFloor <= 0 || btnFloor > 5)
            return;

        _redLightCoroutine = StartCoroutine(CloseDoorSequence());
        LiftSystem.instance.OnLiftDestinationSet(btnFloor);
    }

    public void OnClickExit()
    {
        exitBtn.SetActive(false);
        gameObject.SetActive(false);
    }

    IEnumerator CloseDoorSequence()
    {
        exitBtn.SetActive(false);
        for (int i = 0; i <= 3; i++)
        {
            redLightOn.SetActive(true);
            yield return new WaitForSeconds(0.125f);
            redLightOn.SetActive(false);
            yield return new WaitForSeconds(0.125f);
        }
        // exitBtn.SetActive(true);
        // exitBtn.SetActive(false);
        gameObject.SetActive(false);
    }
}
