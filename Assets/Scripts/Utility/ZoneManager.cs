using UnityEngine;
using System.Collections;

public class ZoneManager : MonoBehaviour
{
    private static Zone currentZone;
    private static Zone previousZone;

    public static void SetCurrentZone(Zone newZone)
    {
        if (currentZone)
        {
            currentZone.SendMessage("OnLeave");
            previousZone = currentZone;
        }

        newZone.SendMessage("OnEnter");

        currentZone = newZone;
    }

    public static Zone GetCurrentZone()
    {
        return currentZone;
    }

    public static void RestorePreviousZone()
    {
        SetCurrentZone(previousZone);
    }
}
