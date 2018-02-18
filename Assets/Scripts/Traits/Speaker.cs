using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Speaker : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public string[] Speech;
        public Vector2 SpeechOffset = new Vector2(0, 0.5f);
        public Vector2 SpeechSize = new Vector2(320, 100);
        public float SpeechDistance = 5f;
        public bool AutoSpeak;

        /// <summary>
        /// 
        /// </summary>
        private int SpeechIndex = -1;
        private Interactor Listener;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collider"></param>
        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (AutoSpeak)
            {
                var interactor = collider.GetComponent<Interactor>();

                if (interactor)
                {
                    StartSpeaking(interactor);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void OnInteraction(Interactor interactor)
        {
            if (!IsSpeaking())
            {
                StartSpeaking(interactor);
            }
            else if (SpeechIndex < Speech.Length - 1)
            {
                ContinueSpeaking();
            }
            else
            {
                FinishSpeaking();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        private void StartSpeaking(Interactor interactor)
        {
            Listener = interactor;
            SpeechIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ContinueSpeaking()
        {
            SpeechIndex++;
        }

        /// <summary>
        /// 
        /// </summary>
        private void FinishSpeaking()
        {
            SpeechIndex = -1;
        }

        public bool IsSpeaking()
        {
            return SpeechIndex > -1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnGUI()
        {
            if (!IsSpeaking())
            {
                return;
            }

            float distance2D = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(Listener.transform.position.x, Listener.transform.position.y));
            if (distance2D > SpeechDistance)
            {
                FinishSpeaking();
                return;
            }

            Vector3 positionOnCamera = Camera.main.WorldToScreenPoint(transform.position + new Vector3(SpeechOffset.x, SpeechOffset.y, 0));
            Rect textBoxPosition = new Rect(positionOnCamera.x - SpeechSize.x / 2, Screen.height - positionOnCamera.y - SpeechSize.y, SpeechSize.x, SpeechSize.y);

            Texture2D speechBackground = new Texture2D(1, 1);
            speechBackground.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
            speechBackground.Apply();

            GUIStyle speechStyle = GUI.skin.GetStyle("Box");
            speechStyle.padding = new RectOffset(20, 20, 20, 20);
            speechStyle.normal.background = speechBackground;
            speechStyle.alignment = TextAnchor.MiddleCenter;
            speechStyle.wordWrap = true;

            GUI.Box(
                textBoxPosition,
                Speech[SpeechIndex],
                speechStyle
            );
        }
    }
}