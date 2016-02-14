using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(Collector))]

public class Inventory : MonoBehaviour
{
    protected Player player;
    protected CharacterController2D controller;
    protected Collector collector;

    protected Vector2 itemGrid = new Vector2(6, 3);
    protected Vector2 itemMargin = new Vector2(48, 32);
    protected Vector2 itemSize = new Vector2(16, 16);
    protected Vector2 itemSpacing = new Vector2(8, 8);
    protected Vector2 itemPadding = new Vector2(2, 2);

    protected float scale;
    protected bool isOpen = false;
    protected float oldTimeScale;

    public void Start()
    {
        player = GetComponent<Player>();
        controller = GetComponent<CharacterController2D>();
        collector = GetComponent<Collector>();

        scale = Camera.main.orthographicSize;
    }

    public void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            Toggle();
        }
    }

    protected void Toggle()
    {
        if (!isOpen)
        {
            if (CanOpen())
            {
                Open();
            }
        }
        else
        {
            Close();
        }
    }

    protected bool CanOpen()
    {
        return controller.State.IsGrounded();
    }

    protected void Open()
    {
        if (isOpen)
        {
            return;
        }

        oldTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        player.StopListening();

        isOpen = true;
    }

    protected void Close()
    {
        if (!isOpen)
        {
            return;
        }

        Time.timeScale = oldTimeScale;

        player.StartListening();

        isOpen = false;
    }

    public void OnGUI()
    {
        if (!isOpen)
        {
            return;
        }

        DrawBackground();

        DrawItems();
    }

    protected void DrawBackground()
    {
        GUI.depth = 1;

        Rect position = new Rect(0f, 0f, Camera.main.pixelWidth, Camera.main.pixelHeight);
        Color color = new Color(0f, 0f, 0f, 0.5f);

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        GUI.skin.box.normal.background = texture;

        GUI.Box(position, GUIContent.none);
    }

    protected void DrawItems()
    {
        GUI.depth = 0;

        for (int i = 0; i < itemGrid.x * itemGrid.y; i++)
        {
            Vector2 position = new Vector2(
                itemMargin.x + (i % itemGrid.x) * (itemSize.x + itemSpacing.x),
                itemMargin.y + Mathf.Floor(i / itemGrid.x) * (itemSize.y + itemSpacing.y)
            );

            DrawSlot(position);

            if (collector.items.Count > i)
            {
                DrawItem(collector.items[i], position);
            }
        }
    }

    protected void DrawSlot(Vector2 position)
    {
        Color color = new Color(0f, 0f, 0f, 0.5f);

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        GUI.skin.box.normal.background = texture;

        GUI.Box(new Rect((position.x - itemPadding.x) * scale, (position.y - itemPadding.y) * scale, (itemSize.x + (itemPadding.x * 2)) * scale, (itemSize.y + (itemPadding.y * 2)) * scale), GUIContent.none);
    }

    protected void DrawItem(Item item, Vector2 position)
    {
        SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();

        if (!spriteRenderer || !spriteRenderer.sprite)
        {
            return;
        }

        Texture2D texture = spriteRenderer.sprite.texture;
        Rect rectangle = spriteRenderer.sprite.textureRect;
        Rect coordinates = new Rect(
            rectangle.x / texture.width,
            rectangle.y / texture.height,
            rectangle.width / texture.width,
            rectangle.height / texture.height
        );

        GUI.DrawTextureWithTexCoords(new Rect(position.x * scale, position.y * scale, itemSize.x * scale, itemSize.y * scale), texture, coordinates);
    }
}
