using UnityEngine;

public class FloorSwitcher : MonoBehaviour
{
    public GameObject floor1;
    public GameObject floor2;
    public GameObject floor3;
    public GameObject floor4;
    public GameObject floor5;

    // Use this for initialization
    void Start()
    {

    }

    public void SetFloor(int floor)
    {
        floor1.SetActive(floor == 1);
        floor2.SetActive(floor == 2);
        floor3.SetActive(floor == 3);
        floor4.SetActive(floor == 4);
        floor5.SetActive(floor == 5);
    }
}