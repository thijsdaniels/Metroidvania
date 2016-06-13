using UnityEngine;
using System.Collections;
using Objects.Collectables;

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

    protected Vector2 cursorPosition = Vector2.zero;
    protected float cursorDeadZone = 0.5f;
    protected bool cursorLocked;

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

        if (!isOpen)
        {
            return;
        }

        MoveCursor(new Vector2(
            Input.GetAxis("Horizontal Primary"),
            Input.GetAxis("Vertical Primary") * -1
        ));

        if (Input.GetButtonDown("Item Primary"))
        {
            Select(cursorPosition, Player.ItemSlot.Primary);
        }

        if (Input.GetButtonDown("Item Secondary"))
        {
            Select(cursorPosition, Player.ItemSlot.Secondary);
        }

        if (Input.GetButtonDown("Item Tertiary"))
        {
            Select(cursorPosition, Player.ItemSlot.Tertiary);
        }

        if (Input.GetButtonDown("Item Quaternary"))
        {
            Select(cursorPosition, Player.ItemSlot.Quaternary);
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

        DrawSlots();

        DrawCursor(cursorPosition);

        DrawItems();
    }

    protected void DrawBackground()
    {
        GUI.depth = 4;

        Rect position = new Rect(0f, 0f, Camera.main.pixelWidth, Camera.main.pixelHeight);
        Color color = new Color(0f, 0f, 0f, 0.5f);

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        GUI.skin.box.normal.background = texture;

        GUI.Box(position, GUIContent.none);
    }

    protected void DrawCursor(Vector2 position)
    {
        GUI.depth = 1;

        Color color = new Color(0.26f, 0.58f, 0.66f, 0.5f);

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        GUI.skin.box.normal.background = texture;

        Vector2 positionOnCamera = PositionToCameraSpace(position);

        GUI.Box(new Rect((positionOnCamera.x - itemPadding.x) * scale, (positionOnCamera.y - itemPadding.y) * scale, (itemSize.x + (itemPadding.x * 2)) * scale, (itemSize.y + (itemPadding.y * 2)) * scale), GUIContent.none);
    }

    protected void DrawSlots()
    {
        for (int y = 0; y < itemGrid.y; y++)
        {
            for (int x = 0; x < itemGrid.x; x++)
            {
                DrawSlot(new Vector2(x, y));
            }
        }
    }

    protected void DrawItems()
    {
        for (int y = 0; y < itemGrid.y; y++)
        {
            for (int x = 0; x < itemGrid.x; x++)
            {
                int index = y * (int) itemGrid.x + x;

                if (collector.items.Count > index)
                {
                    DrawItem(collector.items[index], new Vector2(x, y));
                }
            }
        }
    }

    protected void DrawSlot(Vector2 position)
    {
        if (position == cursorPosition)
        {
            return;
        }

        GUI.depth = 2;

        Color color = new Color(0f, 0f, 0f, 0.5f);

        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        GUI.skin.box.normal.background = texture;

        Vector2 positionOnCamera = PositionToCameraSpace(position);

        GUI.Box(new Rect((positionOnCamera.x - itemPadding.x) * scale, (positionOnCamera.y - itemPadding.y) * scale, (itemSize.x + (itemPadding.x * 2)) * scale, (itemSize.y + (itemPadding.y * 2)) * scale), GUIContent.none);
    }

    protected void DrawItem(Item item, Vector2 position)
    {
        GUI.depth = 0;

        SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();

        if (!spriteRenderer || !spriteRenderer.sprite)
        {
            Debug.LogError("Cannot render " + item + " at position (" + position.x + "," + position.y + "), because its sprite is not set.");
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

        Vector2 positionOnCamera = PositionToCameraSpace(position);

        GUI.DrawTextureWithTexCoords(new Rect(positionOnCamera.x * scale, positionOnCamera.y * scale, itemSize.x * scale, itemSize.y * scale), texture, coordinates);
    }

    protected Vector2 PositionToCameraSpace(Vector2 position)
    {
        return new Vector2(
            itemMargin.x + position.x * (itemSize.x + itemSpacing.x),
            itemMargin.y + position.y * (itemSize.y + itemSpacing.y)
        );
    }

    protected void MoveCursor(Vector2 input)
    {
        input = new Vector2(
            Mathf.Clamp(Mathf.Round(input.x), -1, 1),
            Mathf.Clamp(Mathf.Round(input.y), -1, 1)
        );

        if (input.magnitude == 0)
        {
            cursorLocked = false;
            return;
        }

        if (cursorLocked)
        {
            return;
        }

        cursorPosition = new Vector2(
            Mathf.Repeat(cursorPosition.x + input.x, itemGrid.x),
            Mathf.Repeat(cursorPosition.y + input.y, itemGrid.y)
        );

        cursorLocked = true;
    }

    protected void Select(Vector2 position, Player.ItemSlot slot)
    {
        int index = (int) (position.y * itemGrid.x + position.x);

        if (collector.items.Count > index)
        {
            Item item = collector.items[index];
            player.Equip(slot, item);
        }
    }
}
