using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]

public class Zone : MonoBehaviour
{
    public AudioClip theme;

    public void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player)
        {
            ZoneManager.SetCurrentZone(this);
        }
    }

    public virtual void OnEnter()
    {
        this.PlayTheme();

        this.ShrinkCamera();
    }

    public virtual void OnLeave()
    {
        var pixelPerfectCamera = Camera.main.GetComponent<PixelArtCamera>();

        if (pixelPerfectCamera)
        {
            pixelPerfectCamera.SetOrthographicSize();
        }
    }

    protected void PlayTheme()
    {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();

        if (audioSource && theme && (!audioSource.clip || audioSource.clip.name != theme.name))
        {
            audioSource.clip = theme;
            audioSource.Play();
        }
    }

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
}