using UnityEngine;
using System.Collections;

public class ZoneManager : MonoBehaviour
{
    private static Zone currentZone;

    public static void SetCurrentZone(Zone newZone)
    {
        if (currentZone)
        {
            currentZone.SendMessage("OnLeave");
        }

        newZone.SendMessage("OnEnter");

        currentZone = newZone;
    }

    public static Zone GetCurrentZone()
    {
        return currentZone;
    }
}
