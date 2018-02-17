using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Zone : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public string Title;
        
        /// <summary>
        /// 
        /// </summary>
        public List<Zone> TitleTriggers;
        
        /// <summary>
        /// 
        /// </summary>
        public AudioClip Theme;

        /// <summary>
        /// 
        /// </summary>
        private bool ShouldDrawTitle;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();

            if (player)
            {
                ZoneManager.SetCurrentZone(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerExit2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();

            if (player && IsCurrentZone())
            {
                ZoneManager.RestorePreviousZone();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsCurrentZone()
        {
            return this == ZoneManager.GetCurrentZone();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFirstEnter()
        {
            ShowTitle();

            PlayTheme();

            ShrinkCamera();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="previousZone"></param>
        public virtual void OnEnter(Zone previousZone)
        {
            if (TitleTriggers.Contains(previousZone))
            {
                ShowTitle();
            }

            PlayTheme();

            ShrinkCamera();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextZone"></param>
        public virtual void OnLeave(Zone nextZone)
        {
            PixelArtCamera pixelArtCamera = Camera.main.GetComponent<PixelArtCamera>();

            if (pixelArtCamera)
            {
                pixelArtCamera.SetOrthographicSize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ShowTitle()
        {
            if (string.IsNullOrEmpty(Title))
            {
                return;
            }

            ShouldDrawTitle = true;

            StartCoroutine(HideTitle());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IEnumerator HideTitle()
        {
            yield return new WaitForSeconds(3f);

            ShouldDrawTitle = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void PlayTheme()
        {
            AudioSource audioSource = Camera.main.GetComponent<AudioSource>();

            if (audioSource)
            {
                if (!Theme)
                {
                    audioSource.Stop();
                    audioSource.clip = null;
                }
                else if (!audioSource.clip || audioSource.clip.name != Theme.name)
                {
                    audioSource.Stop();
                    audioSource.clip = Theme;
                    audioSource.PlayDelayed(0.5f);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShrinkCamera()
        {
            float vSize = Camera.main.orthographicSize;
            float hSize = vSize * Screen.width / Screen.height;

            Rect cameraExtents = new Rect(
                Camera.main.transform.position.x - hSize,
                Camera.main.transform.position.y - vSize,
                hSize * 2,
                vSize * 2
            );

            Rect extents = GetExtents();

            float orthographicScale = 1;

            if (cameraExtents.width > extents.width)
            {
                orthographicScale = Mathf.Min(orthographicScale, extents.width / cameraExtents.width);
            }

            if (cameraExtents.height > extents.height)
            {
                orthographicScale = Mathf.Min(orthographicScale, extents.height / cameraExtents.height);
            }

            Camera.main.orthographicSize *= orthographicScale;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Rect GetExtents()
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();

            return new Rect(
                transform.position.x - (transform.localScale.x * collider.size.x) / 2,
                transform.position.y - (transform.localScale.y * collider.size.y) / 2,
                transform.localScale.x * collider.size.x,
                transform.localScale.y * collider.size.y
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnGUI()
        {
            if (ShouldDrawTitle)
            {
                DrawTitle();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawTitle()
        {
            Vector2 textBoxSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.1f);
            Vector2 textBoxPosition = new Vector2(Screen.width * 0.5f - textBoxSize.x * 0.5f, Screen.height * 0.25f - textBoxSize.y * 0.5f);
            Rect textBox = new Rect(textBoxPosition.x, textBoxPosition.y, textBoxSize.x, textBoxSize.y);

            Texture2D speechBackground = new Texture2D(1, 1);
            speechBackground.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
            speechBackground.Apply();

            GUIStyle speechStyle = GUI.skin.GetStyle("Box");
            speechStyle.padding = new RectOffset(20, 20, 20, 20);
            speechStyle.normal.background = speechBackground;
            speechStyle.alignment = TextAnchor.MiddleCenter;

            GUI.Box(textBox, Title, speechStyle);
        }
    }
}