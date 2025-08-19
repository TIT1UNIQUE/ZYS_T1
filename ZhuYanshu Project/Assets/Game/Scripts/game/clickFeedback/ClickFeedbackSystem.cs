
using com;
using UnityEngine;

public class ClickFeedbackSystem : MonoBehaviour
{
    public ClickFeedbackBehaviour clickFeedback;
    Camera _cam;
    public Transform cfbParent;
    public Canvas canvas;
    private void Start()
    {
        _cam = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            AddClickFeedback();

    }

    public void AddClickFeedback()
    {
        var cfb = Instantiate(clickFeedback, cfbParent);
        // var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
        var sf = canvas.scaleFactor;
        cfb.mousePos = Input.mousePosition;

        //Debug.Log(Input.mousePosition);
        //Debug.Log(sf);
    }
}