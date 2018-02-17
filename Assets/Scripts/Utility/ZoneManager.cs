using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class ZoneManager : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private static Zone CurrentZone;
        
        /// <summary>
        /// 
        /// </summary>
        private static Zone PreviousZone;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextZone"></param>
        public static void SetCurrentZone(Zone nextZone)
        {
            if (CurrentZone)
            {
                CurrentZone.SendMessage("OnLeave", nextZone, SendMessageOptions.RequireReceiver);
                PreviousZone = CurrentZone;
            }

            if (!CurrentZone)
            {
                nextZone.SendMessage("OnFirstEnter", null, SendMessageOptions.RequireReceiver);
            }
            else
            {
                nextZone.SendMessage("OnEnter", CurrentZone, SendMessageOptions.RequireReceiver);
            }

            CurrentZone = nextZone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Zone GetCurrentZone()
        {
            return CurrentZone;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RestorePreviousZone()
        {
            SetCurrentZone(PreviousZone);
        }
    }
}