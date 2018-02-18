using Character;
using Objects.Collectables;
using Traits;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(Equipper))]
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(Collector))]
    public class Inventory : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        protected CharacterController2D Controller;
        protected Player Player;
        protected Collector Collector;
        protected Equipper Equipper;

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 ItemGrid = new Vector2(6, 3);
        protected Vector2 ItemMargin = new Vector2(48, 32);
        protected Vector2 ItemSize = new Vector2(16, 16);
        protected Vector2 ItemSpacing = new Vector2(8, 8);
        protected Vector2 ItemPadding = new Vector2(2, 2);

        /// <summary>
        /// 
        /// </summary>
        protected float Scale;
        protected bool IsOpen;
        protected float OldTimeScale;

        /// <summary>
        /// 
        /// </summary>
        protected Vector2 CursorPosition = Vector2.zero;
        protected float CursorDeadZone = 0.5f;
        protected bool CursorLocked;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Controller = GetComponent<CharacterController2D>();
            Player = GetComponent<Player>();
            Collector = GetComponent<Collector>();
            Equipper = GetComponent<Equipper>();

            Scale = Camera.main.orthographicSize;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            if (Input.GetButtonDown("Select"))
            {
                Toggle();
            }

            if (!IsOpen)
            {
                return;
            }

            MoveCursor(new Vector2(
                Input.GetAxis("Left Stick Horizontal"),
                Input.GetAxis("Left Stick Vertical") * -1
            ));

            if (Input.GetButtonDown("X"))
            {
                Select(CursorPosition, Equipper.ItemSlots.Primary);
            }

            if (Input.GetButtonDown("Y"))
            {
                Select(CursorPosition, Equipper.ItemSlots.Secondary);
            }

            if (Input.GetButtonDown("L"))
            {
                Select(CursorPosition, Equipper.ItemSlots.Tertiary);
            }

            if (Input.GetButtonDown("R"))
            {
                Select(CursorPosition, Equipper.ItemSlots.Quaternary);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Toggle()
        {
            if (!IsOpen)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected bool CanOpen()
        {
            return Controller.State.IsGrounded();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Open()
        {
            if (IsOpen)
            {
                return;
            }

            OldTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            Player.StopListening();

            IsOpen = true;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Close()
        {
            if (!IsOpen)
            {
                return;
            }

            Time.timeScale = OldTimeScale;

            Player.StartListening();

            IsOpen = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnGUI()
        {
            if (!IsOpen)
            {
                return;
            }

            DrawBackground();

            DrawSlots();

            DrawCursor(CursorPosition);

            DrawItems();
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        protected void DrawCursor(Vector2 position)
        {
            GUI.depth = 1;

            Color color = new Color(0.26f, 0.58f, 0.66f, 0.5f);

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            GUI.skin.box.normal.background = texture;

            Vector2 positionOnCamera = PositionToCameraSpace(position);

            GUI.Box(new Rect((positionOnCamera.x - ItemPadding.x) * Scale, (positionOnCamera.y - ItemPadding.y) * Scale, (ItemSize.x + (ItemPadding.x * 2)) * Scale, (ItemSize.y + (ItemPadding.y * 2)) * Scale), GUIContent.none);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawSlots()
        {
            for (int y = 0; y < ItemGrid.y; y++)
            {
                for (int x = 0; x < ItemGrid.x; x++)
                {
                    DrawSlot(new Vector2(x, y));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawItems()
        {
            for (int y = 0; y < ItemGrid.y; y++)
            {
                for (int x = 0; x < ItemGrid.x; x++)
                {
                    int index = y * (int) ItemGrid.x + x;

                    if (Collector.Items.Count > index)
                    {
                        DrawItem(Collector.Items[index], new Vector2(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        protected void DrawSlot(Vector2 position)
        {
            if (position == CursorPosition)
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

            GUI.Box(new Rect(
                (positionOnCamera.x - ItemPadding.x) * Scale,
                (positionOnCamera.y - ItemPadding.y) * Scale,
                (ItemSize.x + ItemPadding.x * 2) * Scale,
                (ItemSize.y + ItemPadding.y * 2) * Scale
            ), GUIContent.none);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="position"></param>
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

            GUI.DrawTextureWithTexCoords(new Rect(positionOnCamera.x * Scale, positionOnCamera.y * Scale, ItemSize.x * Scale, ItemSize.y * Scale), texture, coordinates);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected Vector2 PositionToCameraSpace(Vector2 position)
        {
            return new Vector2(
                ItemMargin.x + position.x * (ItemSize.x + ItemSpacing.x),
                ItemMargin.y + position.y * (ItemSize.y + ItemSpacing.y)
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        protected void MoveCursor(Vector2 input)
        {
            input = new Vector2(
                Mathf.Clamp(Mathf.Round(input.x), -1, 1),
                Mathf.Clamp(Mathf.Round(input.y), -1, 1)
            );

            if (input.magnitude == 0)
            {
                CursorLocked = false;
                return;
            }

            if (CursorLocked)
            {
                return;
            }

            CursorPosition = new Vector2(
                Mathf.Repeat(CursorPosition.x + input.x, ItemGrid.x),
                Mathf.Repeat(CursorPosition.y + input.y, ItemGrid.y)
            );

            CursorLocked = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="slot"></param>
        protected void Select(Vector2 position, Equipper.ItemSlots slot)
        {
            int index = (int) (position.y * ItemGrid.x + position.x);

            if (Collector.Items.Count > index)
            {
                Item item = Collector.Items[index];
                Equipper.Equip(slot, item);
            }
        }
    }
}