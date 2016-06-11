using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]

/**
 * 
 */
public class Zone : MonoBehaviour
{
    public string title;
    public List<Zone> titleTriggers;
    public AudioClip theme;

    private bool drawTitle = false;

    /**
     * 
     */
    public void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player)
        {
            ZoneManager.SetCurrentZone(this);
        }
    }

    /**
     * 
     */
    public void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player && this.IsCurrentZone())
        {
            ZoneManager.RestorePreviousZone();
        }
    }

    /**
     * 
     */
    public bool IsCurrentZone()
    {
        return this == ZoneManager.GetCurrentZone();
    }

    /**
     * 
     */
    public virtual void OnFirstEnter()
    {
        this.ShowTitle();

        this.PlayTheme();

        this.ShrinkCamera();
    }

    /**
     * 
     */
    public virtual void OnEnter(Zone previousZone)
    {
        if (this.titleTriggers.Contains(previousZone))
        {
            this.ShowTitle();
        }

        this.PlayTheme();

        this.ShrinkCamera();
    }

    /**
     * 
     */
    public virtual void OnLeave(Zone nextZone)
    {
        var pixelPerfectCamera = Camera.main.GetComponent<PixelArtCamera>();

        if (pixelPerfectCamera)
        {
            pixelPerfectCamera.SetOrthographicSize();
        }
    }


    /**
     * 
     */
    protected void ShowTitle()
    {
        if (string.IsNullOrEmpty(this.title))
        {
            return;
        }

        this.drawTitle = true;

        StartCoroutine(this.HideTitle());
    }

    /**
     * 
     */
    protected IEnumerator HideTitle()
    {
        yield return new WaitForSeconds(3f);

        this.drawTitle = false;
    }

    /**
     * 
     */
    protected void PlayTheme()
    {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();

        if (audioSource)
        {
            if (!theme)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            else if (!audioSource.clip || audioSource.clip.name != theme.name)
            {
                audioSource.Stop();
                audioSource.clip = theme;
                audioSource.PlayDelayed(0.5f);
            }
        }
    }

    /**
     * 
     */
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

        Rect extents = this.GetExtents();

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

    /**
     * 
     */
    public Rect GetExtents()
    {
        var collider = GetComponent<BoxCollider2D>();

        return new Rect(
            transform.position.x - (transform.localScale.x * collider.size.x) / 2,
            transform.position.y - (transform.localScale.y * collider.size.y) / 2,
            transform.localScale.x * collider.size.x,
            transform.localScale.y * collider.size.y
        );
    }

    /**
     * 
     */
    public void OnGUI()
    {
        if (this.drawTitle)
        {
            this.DrawTitle();
        }
    }

    /**
     * 
     */
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

        GUI.Box(
            textBox,
            this.title,
            speechStyle
        );
    }
}