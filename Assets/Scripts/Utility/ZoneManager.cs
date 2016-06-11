using UnityEngine;
using System.Collections;

/**
 * 
 */
public class ZoneManager : MonoBehaviour
{
    private static Zone currentZone;
    private static Zone previousZone;

    /**
     * 
     */
    public static void SetCurrentZone(Zone nextZone)
    {
        if (currentZone)
        {
            currentZone.SendMessage("OnLeave", nextZone, SendMessageOptions.RequireReceiver);
            previousZone = currentZone;
        }

        if (!currentZone)
        {
            nextZone.SendMessage("OnFirstEnter", null, SendMessageOptions.RequireReceiver);
        }
        else
        {
            nextZone.SendMessage("OnEnter", currentZone, SendMessageOptions.RequireReceiver);
        }

        currentZone = nextZone;
    }

    /**
     * 
     */
    public static Zone GetCurrentZone()
    {
        return currentZone;
    }

    /**
     * 
     */
    public static void RestorePreviousZone()
    {
        SetCurrentZone(previousZone);
    }
}
