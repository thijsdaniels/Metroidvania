using Character;
using Traits;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(Collector))]
    [RequireComponent(typeof(Equipper))]
    [RequireComponent(typeof(Damagable))]
    public class HUD : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        private Player Player;
        private Collector Collector;
        private Equipper Equipper;
        private Damagable Damagable;

        /// <summary>
        /// 
        /// </summary>
        public float Scale = 1f;
        public int Margin;

        /// <summary>
        /// 
        /// </summary>
        public Font Font;

        /// <summary>
        /// 
        /// </summary>
        public Sprite FullHealthUnitSprite;
        public Sprite EmptyHealthUnitSprite;
        public Sprite CoinUnitSprite;
        public Sprite KeyUnitSprite;

        /// <summary>
        /// 
        /// </summary>
        public Sprite AButtonSprite;
        public Sprite BButtonSprite;
        public Sprite XButtonSprite;
        public Sprite YButtonSprite;
        public Sprite LButtonSprite;
        public Sprite RButtonSprite;
        public Color ButtonColor = new Color(1f, 1f, 1f, 0.5f);

        /// <summary>
        /// 
        /// </summary>
        private GUIStyle LabelLeftStyle;
        private GUIStyle LabelCenterStyle;
        private GUIStyle LabelRightStyle;

        /// <summary>
        /// 
        /// </summary>
        public Color LabelOutlineColor = new Color(0f, 0f, 0f, 0.25f);
        public float LabelOutlineThickness = 1f;

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Spacing = new Vector2(8, 8);
        public Vector2 Padding = new Vector2(0, 0);

        /// <summary>
        /// 
        /// </summary>
        private Texture2D FullHealthUnitTexture;
        private Rect FullHealthUnitRectangle;
        private Rect FullHealthUnitCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D EmptyHealthUnitTexture;
        private Rect EmptyHealthUnitRectangle;
        private Rect EmptyHealthUnitCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D CoinUnitTexture;
        private Rect CoinUnitRectangle;
        private Rect CoinUnitCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D KeyUnitTexture;
        private Rect KeyUnitRectangle;
        private Rect KeyUnitCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D AButtonTexture;
        private Rect AButtonRectangle;
        private Rect AButtonCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D BButtonTexture;
        private Rect BButtonRectangle;
        private Rect BButtonCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D XButtonTexture;
        private Rect XButtonRectangle;
        private Rect XButtonCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D YButtonTexture;
        private Rect YButtonRectangle;
        private Rect YButtonCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D LButtonTexture;
        private Rect LButtonRectangle;
        private Rect LButtonCoordinates;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D RButtonTexture;
        private Rect RButtonRectangle;
        private Rect RButtonCoordinates;

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Scale = Scale * Camera.main.orthographicSize;

            Player = GetComponent<Player>();
            Collector = GetComponent<Collector>();
            Equipper = GetComponent<Equipper>();
            Damagable = GetComponent<Damagable>();

            LabelLeftStyle = new GUIStyle();
            LabelLeftStyle.alignment = TextAnchor.MiddleLeft;
            LabelLeftStyle.normal.textColor = Color.white;

            LabelCenterStyle = new GUIStyle();
            LabelCenterStyle.alignment = TextAnchor.MiddleCenter;
            LabelCenterStyle.normal.textColor = Color.white;

            LabelRightStyle = new GUIStyle();
            LabelRightStyle.alignment = TextAnchor.MiddleRight;
            LabelRightStyle.normal.textColor = Color.white;

            if (FullHealthUnitSprite)
            {
                FullHealthUnitTexture = FullHealthUnitSprite.texture;
                FullHealthUnitRectangle = FullHealthUnitSprite.textureRect;
                FullHealthUnitCoordinates = new Rect(
                    FullHealthUnitRectangle.x / FullHealthUnitTexture.width,
                    FullHealthUnitRectangle.y / FullHealthUnitTexture.height,
                    FullHealthUnitRectangle.width / FullHealthUnitTexture.width,
                    FullHealthUnitRectangle.height / FullHealthUnitTexture.height
                );
            }

            if (EmptyHealthUnitSprite)
            {
                EmptyHealthUnitTexture = EmptyHealthUnitSprite.texture;
                EmptyHealthUnitRectangle = EmptyHealthUnitSprite.textureRect;
                EmptyHealthUnitCoordinates = new Rect(
                    EmptyHealthUnitRectangle.x / EmptyHealthUnitTexture.width,
                    EmptyHealthUnitRectangle.y / EmptyHealthUnitTexture.height,
                    EmptyHealthUnitRectangle.width / EmptyHealthUnitTexture.width,
                    EmptyHealthUnitRectangle.height / EmptyHealthUnitTexture.height
                );
            }

            if (CoinUnitSprite)
            {
                CoinUnitTexture = CoinUnitSprite.texture;
                CoinUnitRectangle = CoinUnitSprite.textureRect;
                CoinUnitCoordinates = new Rect(
                    CoinUnitRectangle.x / CoinUnitTexture.width,
                    CoinUnitRectangle.y / CoinUnitTexture.height,
                    CoinUnitRectangle.width / CoinUnitTexture.width,
                    CoinUnitRectangle.height / CoinUnitTexture.height
                );
            }

            if (KeyUnitSprite)
            {
                KeyUnitTexture = KeyUnitSprite.texture;
                KeyUnitRectangle = KeyUnitSprite.textureRect;
                KeyUnitCoordinates = new Rect(
                    KeyUnitRectangle.x / KeyUnitTexture.width,
                    KeyUnitRectangle.y / KeyUnitTexture.height,
                    KeyUnitRectangle.width / KeyUnitTexture.width,
                    KeyUnitRectangle.height / KeyUnitTexture.height
                );
            }

            if (AButtonSprite)
            {
                AButtonTexture = AButtonSprite.texture;
                AButtonRectangle = AButtonSprite.textureRect;
                AButtonCoordinates = new Rect(
                    AButtonRectangle.x / AButtonTexture.width,
                    AButtonRectangle.y / AButtonTexture.height,
                    AButtonRectangle.width / AButtonTexture.width,
                    AButtonRectangle.height / AButtonTexture.height
                );
            }

            if (BButtonSprite)
            {
                BButtonTexture = BButtonSprite.texture;
                BButtonRectangle = BButtonSprite.textureRect;
                BButtonCoordinates = new Rect(
                    BButtonRectangle.x / BButtonTexture.width,
                    BButtonRectangle.y / BButtonTexture.height,
                    BButtonRectangle.width / BButtonTexture.width,
                    BButtonRectangle.height / BButtonTexture.height
                );
            }

            if (XButtonSprite)
            {
                XButtonTexture = XButtonSprite.texture;
                XButtonRectangle = XButtonSprite.textureRect;
                XButtonCoordinates = new Rect(
                    XButtonRectangle.x / XButtonTexture.width,
                    XButtonRectangle.y / XButtonTexture.height,
                    XButtonRectangle.width / XButtonTexture.width,
                    XButtonRectangle.height / XButtonTexture.height
                );
            }

            if (YButtonSprite)
            {
                YButtonTexture = YButtonSprite.texture;
                YButtonRectangle = YButtonSprite.textureRect;
                YButtonCoordinates = new Rect(
                    YButtonRectangle.x / YButtonTexture.width,
                    YButtonRectangle.y / YButtonTexture.height,
                    YButtonRectangle.width / YButtonTexture.width,
                    YButtonRectangle.height / YButtonTexture.height
                );
            }

            if (LButtonSprite)
            {
                LButtonTexture = LButtonSprite.texture;
                LButtonRectangle = LButtonSprite.textureRect;
                LButtonCoordinates = new Rect(
                    LButtonRectangle.x / LButtonTexture.width,
                    LButtonRectangle.y / LButtonTexture.height,
                    LButtonRectangle.width / LButtonTexture.width,
                    LButtonRectangle.height / LButtonTexture.height
                );
            }

            if (RButtonSprite)
            {
                RButtonTexture = RButtonSprite.texture;
                RButtonRectangle = RButtonSprite.textureRect;
                RButtonCoordinates = new Rect(
                    RButtonRectangle.x / RButtonTexture.width,
                    RButtonRectangle.y / RButtonTexture.height,
                    RButtonRectangle.width / RButtonTexture.width,
                    RButtonRectangle.height / RButtonTexture.height
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnGUI()
        {
            GUI.skin.font = Font;

            LabelLeftStyle.fontSize = Mathf.RoundToInt(6f * Scale);
            LabelCenterStyle.fontSize = Mathf.RoundToInt(6f * Scale);
            LabelRightStyle.fontSize = Mathf.RoundToInt(6f * Scale);

            DrawHealth();

            DrawMana();

            DrawCoins();

            DrawKeys();

            DrawButtons();
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawHealth()
        {
            if (!FullHealthUnitTexture || !EmptyHealthUnitTexture)
            {
                return;
            }

            GUI.depth = 0;

            for (int i = 0; i < Damagable.MaximumHealth; i++)
            {
                var position = new Vector2(
                    Margin + Spacing.x * (i % 10) * Scale,
                    Margin + Spacing.y * Mathf.Ceil((float) i / 10) * Scale
                );

                var size = new Vector2(
                    16 * Scale,
                    16 * Scale
                );

                if (i < Damagable.CurrentHealth)
                {
                    GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, size.x, size.y), FullHealthUnitTexture, FullHealthUnitCoordinates);
                }
                else
                {
                    GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, size.x, size.y), EmptyHealthUnitTexture, EmptyHealthUnitCoordinates);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawMana()
        {
            GUI.depth = 0;

            var container = new Rect(
                0.5f * Margin * Scale, // TODO: This offset should just be 1 * margin.
                2.25f * Margin * Scale, // TODO: This offset should be determined by the height of the health bar.
                2 * Scale * Collector.Mana.Maximum,
                6 * Scale
            );

            DrawRectangle(container, Color.white);

            var background = new Rect(
                container.x + 1 * Scale,
                container.y + 1 * Scale,
                container.width - 2 * Scale,
                container.height - 2 * Scale
            );

            DrawRectangle(background, new Color(0.15f, 0.15f, 0.15f));

            var mana = new Rect(
                background.x,
                background.y,
                background.width * Collector.Mana.Ratio(),
                background.height
            );

            DrawRectangle(mana, new Color(0.22f, 0.71f, 0.29f));
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawCoins()
        {
            if (!CoinUnitTexture)
            {
                return;
            }

            GUI.depth = 0;

            var size = new Vector2(
                16 * Scale,
                16 * Scale
            );

            var iconPosition = new Vector2(
                Margin,
                Camera.main.pixelHeight - Margin - (size.y)
            );

            GUI.DrawTextureWithTexCoords(new Rect(iconPosition.x, iconPosition.y, size.x, size.y), CoinUnitTexture, CoinUnitCoordinates);

            var textPosition = new Rect(
                iconPosition.x + size.x + (Spacing.x / 4),
                iconPosition.y - 2, // arbitrary offset, because number rendered too low
                16 * Scale,
                16 * Scale
            );

            OutlinedLabel(textPosition, Collector.Coins.ToString(), LabelLeftStyle);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawKeys()
        {
            if (!KeyUnitTexture)
            {
                return;
            }

            GUI.depth = 0;

            var size = new Vector2(
                16 * Scale,
                16 * Scale
            );

            var iconPosition = new Vector2(
                Camera.main.pixelWidth - Margin - size.x,
                Camera.main.pixelHeight - Margin - size.y
            );

            GUI.DrawTextureWithTexCoords(new Rect(iconPosition.x, iconPosition.y, size.x, size.y), KeyUnitTexture, KeyUnitCoordinates);

            var textPosition = new Rect(
                iconPosition.x - size.x - (Spacing.x / 4),
                iconPosition.y - 2, // arbitrary offset, because number rendered too low
                16 * Scale,
                16 * Scale
            );

            OutlinedLabel(textPosition, Collector.Keys.ToString(), LabelRightStyle);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DrawButtons()
        {
            GUI.depth = 0;

            Vector2 size = new Vector2(
                16 * Scale,
                16 * Scale
            );

            DrawAButton(size);
            DrawBButton(size);
            DrawXButton(size);
            DrawYButton(size);
            DrawLButton(size);
            DrawRButton(size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        protected void DrawAButton(Vector2 size)
        {
            Vector2 aIconPosition = new Vector2(
                Camera.main.pixelWidth - Margin - (size.x * 2.25f),
                Margin + (size.y * 2)
            );

            if (AButtonTexture)
            {
                Color oldColor = GUI.color;
                GUI.color = ButtonColor;
                GUI.DrawTextureWithTexCoords(new Rect(aIconPosition.x, aIconPosition.y, size.x, size.y), AButtonTexture, AButtonCoordinates);
                GUI.color = oldColor;
            }

            CharacterController2D controller = Player.GetComponent<CharacterController2D>();
            Jumper jumper = Player.GetComponent<Jumper>();

            string label = null;

            if (controller && controller.State.IsSwimming())
            {
                label = "Swim";
            }
            else if (jumper && jumper.CanJump())
            {
                label = "Jump";
            }
            
            if (label != null)
            {
                var aLabelPosition = new Rect(
                    aIconPosition.x,
                    aIconPosition.y,
                    16 * Scale,
                    16 * Scale
                );

                OutlinedLabel(aLabelPosition, label, LabelCenterStyle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        protected void DrawBButton(Vector2 size)
        {
            Vector2 bIconPosition = new Vector2(
                Camera.main.pixelWidth - Margin - (size.x * 1.5f),
                Margin + (size.y * 1.25f)
            );

            if (BButtonTexture)
            {
                Color oldColor = GUI.color;
                GUI.color = ButtonColor;
                GUI.DrawTextureWithTexCoords(new Rect(bIconPosition.x, bIconPosition.y, size.x, size.y), BButtonTexture, BButtonCoordinates);
                GUI.color = oldColor;
            }
            
            Roller roller = Player.GetComponent<Roller>();
            Interactor interactor = Player.GetComponent<Interactor>();

            string label = null;

            if (interactor && interactor.CanInteract())
            {
                label = interactor.Interactable.Action;
            }
            else if (roller && roller.CanRoll())
            {
                label = "Roll";
            }

            if (label != null)
            {
                var bLabelPosition = new Rect(
                    bIconPosition.x,
                    bIconPosition.y,
                    16 * Scale,
                    16 * Scale
                );

                OutlinedLabel(bLabelPosition, label, LabelCenterStyle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        protected void DrawXButton(Vector2 size)
        {
            Vector2 xIconPosition = new Vector2(
                Camera.main.pixelWidth - Margin - (size.x * 3f),
                Margin + (size.y * 1.25f)
            );

            if (XButtonTexture)
            {
                Color oldColor = GUI.color;
                GUI.color = ButtonColor;
                GUI.DrawTextureWithTexCoords(new Rect(xIconPosition.x, xIconPosition.y, size.x, size.y), XButtonTexture, XButtonCoordinates);
                GUI.color = oldColor;
            }

            if (Equipper.PrimaryItem)
            {
                var xButtonActionSprite = Equipper.PrimaryItem.GetComponent<SpriteRenderer>().sprite;

                if (xButtonActionSprite)
                {
                    Texture2D xButtonActionTexture = xButtonActionSprite.texture;
                    Rect xButtonActionRectangle = xButtonActionSprite.textureRect;
                    Rect xButtonActionCoordinates = new Rect(
                        xButtonActionRectangle.x / xButtonActionTexture.width,
                        xButtonActionRectangle.y / xButtonActionTexture.height,
                        xButtonActionRectangle.width / xButtonActionTexture.width,
                        xButtonActionRectangle.height / xButtonActionTexture.height
                    );

                    GUI.DrawTextureWithTexCoords(new Rect(xIconPosition.x + Padding.x * Scale, xIconPosition.y + Padding.y * Scale, size.x - 2 * Padding.x * Scale, size.y - 2 * Padding.y * Scale), xButtonActionTexture, xButtonActionCoordinates);
                }

                if (Equipper.PrimaryItem.GetAmmo() != null)
                {
                    var yLabelPosition = new Rect(
                        xIconPosition.x + (4 * Scale),
                        xIconPosition.y + (4 * Scale),
                        16 * Scale,
                        16 * Scale
                    );

                    OutlinedLabel(yLabelPosition, Equipper.PrimaryItem.GetAmmo().ToString(), LabelCenterStyle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        protected void DrawYButton(Vector2 size)
        {
            Vector2 yIconPosition = new Vector2(
                Camera.main.pixelWidth - Margin - (size.x * 2.25f),
                Margin + (size.y * 0.5f)
            );

            if (YButtonTexture)
            {
                Color oldColor = GUI.color;
                GUI.color = ButtonColor;
                GUI.DrawTextureWithTexCoords(new Rect(yIconPosition.x, yIconPosition.y, size.x, size.y), YButtonTexture, YButtonCoordinates);
                GUI.color = oldColor;
            }

            if (Equipper.SecondaryItem)
            {
                var yButtonActionSprite = Equipper.SecondaryItem.GetComponent<SpriteRenderer>().sprite;

                if (yButtonActionSprite)
                {
                    Texture2D yButtonActionTexture = yButtonActionSprite.texture;
                    Rect yButtonActionRectangle = yButtonActionSprite.textureRect;
                    Rect yButtonActionCoordinates = new Rect(
                        yButtonActionRectangle.x / yButtonActionTexture.width,
                        yButtonActionRectangle.y / yButtonActionTexture.height,
                        yButtonActionRectangle.width / yButtonActionTexture.width,
                        yButtonActionRectangle.height / yButtonActionTexture.height
                    );

                    GUI.DrawTextureWithTexCoords(new Rect(yIconPosition.x + Padding.x * Scale, yIconPosition.y + Padding.y * Scale, size.x - 2 * Padding.x * Scale, size.y - 2 * Padding.y * Scale), yButtonActionTexture, yButtonActionCoordinates);
                }

                if (Equipper.SecondaryItem.GetAmmo() != null)
                {
                    var yLabelPosition = new Rect(
                        yIconPosition.x + (4 * Scale),
                        yIconPosition.y + (4 * Scale),
                        16 * Scale,
                        16 * Scale
                    );

                    OutlinedLabel(yLabelPosition, Equipper.SecondaryItem.GetAmmo().ToString(), LabelCenterStyle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        protected void DrawLButton(Vector2 size)
        {
            Vector2 lIconPosition = new Vector2(
                Camera.main.pixelWidth - Margin - (size.x * 3.5f),
                Margin
            );

            if (LButtonTexture)
            {
                Color oldColor = GUI.color;
                GUI.color = ButtonColor;
                GUI.DrawTextureWithTexCoords(new Rect(lIconPosition.x, lIconPosition.y, size.x, size.y), LButtonTexture, LButtonCoordinates);
                GUI.color = oldColor;
            }

            if (Equipper.TertiaryItem)
            {
                var lButtonActionSprite = Equipper.TertiaryItem.GetComponent<SpriteRenderer>().sprite;

                if (lButtonActionSprite)
                {
                    Texture2D lButtonActionTexture = lButtonActionSprite.texture;
                    Rect lButtonActionRectangle = lButtonActionSprite.textureRect;
                    Rect lButtonActionCoordinates = new Rect(
                        lButtonActionRectangle.x / lButtonActionTexture.width,
                        lButtonActionRectangle.y / lButtonActionTexture.height,
                        lButtonActionRectangle.width / lButtonActionTexture.width,
                        lButtonActionRectangle.height / lButtonActionTexture.height
                    );

                    GUI.DrawTextureWithTexCoords(new Rect(lIconPosition.x + Padding.x * Scale, lIconPosition.y + Padding.y * Scale, size.x - 2 * Padding.x * Scale, size.y - 2 * Padding.y * Scale), lButtonActionTexture, lButtonActionCoordinates);
                }

                if (Equipper.TertiaryItem.GetAmmo() != null)
                {
                    var lLabelPosition = new Rect(
                        lIconPosition.x + (4 * Scale),
                        lIconPosition.y + (4 * Scale),
                        16 * Scale,
                        16 * Scale
                    );

                    OutlinedLabel(lLabelPosition, Equipper.TertiaryItem.GetAmmo().ToString(), LabelCenterStyle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        protected void DrawRButton(Vector2 size)
        {
            Vector2 rIconPosition = new Vector2(
                Camera.main.pixelWidth - Margin - size.x,
                Margin
            );

            if (RButtonTexture)
            {
                Color oldColor = GUI.color;
                GUI.color = ButtonColor;
                GUI.DrawTextureWithTexCoords(new Rect(rIconPosition.x, rIconPosition.y, size.x, size.y), RButtonTexture, RButtonCoordinates);
                GUI.color = oldColor;
            }

            if (Equipper.QuaternaryItem)
            {
                var rButtonActionSprite = Equipper.QuaternaryItem.GetComponent<SpriteRenderer>().sprite;

                if (rButtonActionSprite)
                {
                    Texture2D rButtonActionTexture = rButtonActionSprite.texture;
                    Rect rButtonActionRectangle = rButtonActionSprite.textureRect;
                    Rect rButtonActionCoordinates = new Rect(
                        rButtonActionRectangle.x / rButtonActionTexture.width,
                        rButtonActionRectangle.y / rButtonActionTexture.height,
                        rButtonActionRectangle.width / rButtonActionTexture.width,
                        rButtonActionRectangle.height / rButtonActionTexture.height
                    );

                    GUI.DrawTextureWithTexCoords(new Rect(rIconPosition.x + Padding.x * Scale, rIconPosition.y + Padding.y * Scale, size.x - 2 * Padding.x * Scale, size.y - 2 * Padding.y * Scale), rButtonActionTexture, rButtonActionCoordinates);
                }

                if (Equipper.QuaternaryItem.GetAmmo() != null)
                {
                    var rLabelPosition = new Rect(
                        rIconPosition.x + (4 * Scale),
                        rIconPosition.y + (4 * Scale),
                        16 * Scale,
                        16 * Scale
                    );

                    OutlinedLabel(rLabelPosition, Equipper.QuaternaryItem.GetAmmo().ToString(), LabelCenterStyle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="color"></param>
        protected void DrawRectangle(Rect rectangle, Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();

            GUIStyle rectangleStyle = new GUIStyle();
            rectangleStyle.normal.background = texture;

            GUI.Box(rectangle, GUIContent.none, rectangleStyle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        protected void OutlinedLabel(Rect rect, string text, GUIStyle style)
        {
            OutlinedLabel(rect, text, style, LabelOutlineColor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <param name="outlineColor"></param>
        protected void OutlinedLabel(Rect rect, string text, GUIStyle style, Color outlineColor)
        {
            OutlinedLabel(rect, text, style, outlineColor, LabelOutlineThickness * Scale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <param name="outlineColor"></param>
        /// <param name="thickness"></param>
        protected void OutlinedLabel(Rect rect, string text, GUIStyle style, Color outlineColor, float thickness)
        {
            Color textColor = style.normal.textColor;
            style.normal.textColor = outlineColor;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Rect outlineRect = new Rect(
                        rect.x - x * thickness,
                        rect.y - y * thickness,
                        rect.width,
                        rect.height
                    );

                    GUI.Label(outlineRect, text, style);
                }
            }

            style.normal.textColor = textColor;
            GUI.Label(rect, text, style);
        }
    }
}