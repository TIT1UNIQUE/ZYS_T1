using UnityEngine;

public class LiftDoors : MonoBehaviour
{
    public LiftDoor doorRight;
    public LiftDoor doorLeft;

    public bool DoorsClosedAndStopped()
    {
        return doorRight.closedAndStopped && doorLeft.closedAndStopped;
    }

    public bool DoorsOpenAndStopped()
    {
        return doorRight.openAndStopped && doorLeft.openAndStopped;
    }

    public bool DoorsMoving()
    {
        return !doorRight.stopped || !doorLeft.stopped;
    }
}