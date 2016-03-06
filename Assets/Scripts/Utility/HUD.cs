using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(Collector))]
[RequireComponent(typeof(Damagable))]

public class HUD : MonoBehaviour
{
    private Player player;
    private Collector collector;
    private Damagable damagable;

    public float scale = 1f;
    public int margin;

    public Font font;

    public Sprite fullHealthUnitSprite;
    public Sprite emptyHealthUnitSprite;
    public Sprite coinUnitSprite;
    public Sprite keyUnitSprite;

    public Sprite aButtonSprite;
    public Sprite bButtonSprite;
    public Sprite xButtonSprite;
    public Sprite yButtonSprite;
    public Sprite lButtonSprite;
    public Sprite rButtonSprite;
    public Color buttonColor = new Color(1f, 1f, 1f, 0.5f);

    private GUIStyle labelLeftStyle;
    private GUIStyle labelCenterStyle;
    private GUIStyle labelRightStyle;

    public Color labelOutlineColor = new Color(0f, 0f, 0f, 0.25f);
    public float labelOutlineThickness = 1f;

    public Vector2 spacing = new Vector2(8, 8);
    public Vector2 padding = new Vector2(0, 0);

    private Texture2D fullHealthUnitTexture;
    private Rect fullHealthUnitRectangle;
    private Rect fullHealthUnitCoordinates;

    private Texture2D emptyHealthUnitTexture;
    private Rect emptyHealthUnitRectangle;
    private Rect emptyHealthUnitCoordinates;

    private Texture2D coinUnitTexture;
    private Rect coinUnitRectangle;
    private Rect coinUnitCoordinates;

    private Texture2D keyUnitTexture;
    private Rect keyUnitRectangle;
    private Rect keyUnitCoordinates;

    private Texture2D aButtonTexture;
    private Rect aButtonRectangle;
    private Rect aButtonCoordinates;

    private Texture2D bButtonTexture;
    private Rect bButtonRectangle;
    private Rect bButtonCoordinates;

    private Texture2D xButtonTexture;
    private Rect xButtonRectangle;
    private Rect xButtonCoordinates;

    private Texture2D yButtonTexture;
    private Rect yButtonRectangle;
    private Rect yButtonCoordinates;

    private Texture2D lButtonTexture;
    private Rect lButtonRectangle;
    private Rect lButtonCoordinates;

    private Texture2D rButtonTexture;
    private Rect rButtonRectangle;
    private Rect rButtonCoordinates;

    /**
     * 
     */
    public void Start()
    {
        scale = scale * Camera.main.orthographicSize;

        player = GetComponent<Player>();
        collector = GetComponent<Collector>();
        damagable = GetComponent<Damagable>();

        labelLeftStyle = new GUIStyle();
        labelLeftStyle.alignment = TextAnchor.MiddleLeft;
        labelLeftStyle.normal.textColor = Color.white;

        labelCenterStyle = new GUIStyle();
        labelCenterStyle.alignment = TextAnchor.MiddleCenter;
        labelCenterStyle.normal.textColor = Color.white;

        labelRightStyle = new GUIStyle();
        labelRightStyle.alignment = TextAnchor.MiddleRight;
        labelRightStyle.normal.textColor = Color.white;

        if (fullHealthUnitSprite)
        {
            fullHealthUnitTexture = fullHealthUnitSprite.texture;
            fullHealthUnitRectangle = fullHealthUnitSprite.textureRect;
            fullHealthUnitCoordinates = new Rect(
                fullHealthUnitRectangle.x / fullHealthUnitTexture.width,
                fullHealthUnitRectangle.y / fullHealthUnitTexture.height,
                fullHealthUnitRectangle.width / fullHealthUnitTexture.width,
                fullHealthUnitRectangle.height / fullHealthUnitTexture.height
            );
        }

        if (emptyHealthUnitSprite)
        {
            emptyHealthUnitTexture = emptyHealthUnitSprite.texture;
            emptyHealthUnitRectangle = emptyHealthUnitSprite.textureRect;
            emptyHealthUnitCoordinates = new Rect(
                emptyHealthUnitRectangle.x / emptyHealthUnitTexture.width,
                emptyHealthUnitRectangle.y / emptyHealthUnitTexture.height,
                emptyHealthUnitRectangle.width / emptyHealthUnitTexture.width,
                emptyHealthUnitRectangle.height / emptyHealthUnitTexture.height
            );
        }

        if (coinUnitSprite)
        {
            coinUnitTexture = coinUnitSprite.texture;
            coinUnitRectangle = coinUnitSprite.textureRect;
            coinUnitCoordinates = new Rect(
                coinUnitRectangle.x / coinUnitTexture.width,
                coinUnitRectangle.y / coinUnitTexture.height,
                coinUnitRectangle.width / coinUnitTexture.width,
                coinUnitRectangle.height / coinUnitTexture.height
            );
        }

        if (keyUnitSprite)
        {
            keyUnitTexture = keyUnitSprite.texture;
            keyUnitRectangle = keyUnitSprite.textureRect;
            keyUnitCoordinates = new Rect(
                keyUnitRectangle.x / keyUnitTexture.width,
                keyUnitRectangle.y / keyUnitTexture.height,
                keyUnitRectangle.width / keyUnitTexture.width,
                keyUnitRectangle.height / keyUnitTexture.height
            );
        }

        if (aButtonSprite)
        {
            aButtonTexture = aButtonSprite.texture;
            aButtonRectangle = aButtonSprite.textureRect;
            aButtonCoordinates = new Rect(
                aButtonRectangle.x / aButtonTexture.width,
                aButtonRectangle.y / aButtonTexture.height,
                aButtonRectangle.width / aButtonTexture.width,
                aButtonRectangle.height / aButtonTexture.height
            );
        }

        if (bButtonSprite)
        {
            bButtonTexture = bButtonSprite.texture;
            bButtonRectangle = bButtonSprite.textureRect;
            bButtonCoordinates = new Rect(
                bButtonRectangle.x / bButtonTexture.width,
                bButtonRectangle.y / bButtonTexture.height,
                bButtonRectangle.width / bButtonTexture.width,
                bButtonRectangle.height / bButtonTexture.height
            );
        }

        if (xButtonSprite)
        {
            xButtonTexture = xButtonSprite.texture;
            xButtonRectangle = xButtonSprite.textureRect;
            xButtonCoordinates = new Rect(
                xButtonRectangle.x / xButtonTexture.width,
                xButtonRectangle.y / xButtonTexture.height,
                xButtonRectangle.width / xButtonTexture.width,
                xButtonRectangle.height / xButtonTexture.height
            );
        }

        if (yButtonSprite)
        {
            yButtonTexture = yButtonSprite.texture;
            yButtonRectangle = yButtonSprite.textureRect;
            yButtonCoordinates = new Rect(
                yButtonRectangle.x / yButtonTexture.width,
                yButtonRectangle.y / yButtonTexture.height,
                yButtonRectangle.width / yButtonTexture.width,
                yButtonRectangle.height / yButtonTexture.height
            );
        }

        if (lButtonSprite)
        {
            lButtonTexture = lButtonSprite.texture;
            lButtonRectangle = lButtonSprite.textureRect;
            lButtonCoordinates = new Rect(
                lButtonRectangle.x / lButtonTexture.width,
                lButtonRectangle.y / lButtonTexture.height,
                lButtonRectangle.width / lButtonTexture.width,
                lButtonRectangle.height / lButtonTexture.height
            );
        }

        if (rButtonSprite)
        {
            rButtonTexture = rButtonSprite.texture;
            rButtonRectangle = rButtonSprite.textureRect;
            rButtonCoordinates = new Rect(
                rButtonRectangle.x / rButtonTexture.width,
                rButtonRectangle.y / rButtonTexture.height,
                rButtonRectangle.width / rButtonTexture.width,
                rButtonRectangle.height / rButtonTexture.height
            );
        }
    }

    /**
     * 
     */
    public void OnGUI()
    {
        GUI.skin.font = font;

        labelLeftStyle.fontSize = Mathf.RoundToInt(6f * scale);
        labelCenterStyle.fontSize = Mathf.RoundToInt(6f * scale);
        labelRightStyle.fontSize = Mathf.RoundToInt(6f * scale);

        DrawHealth();

        DrawMana();

        DrawCoins();

        DrawKeys();

        DrawButtons();
    }

    /**
     * 
     */
    protected void DrawHealth()
    {
        if (!fullHealthUnitTexture || !emptyHealthUnitTexture)
        {
            return;
        }

        GUI.depth = 0;

        for (int i = 0; i < damagable.maximumHealth; i++)
        {
            var position = new Vector2(
                margin + spacing.x * (i % 10) * scale,
                margin + spacing.y * Mathf.Ceil(i / 10) * scale
            );

            var size = new Vector2(
                16 * scale,
                16 * scale
            );

            if (i < damagable.currentHealth)
            {
                GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, size.x, size.y), fullHealthUnitTexture, fullHealthUnitCoordinates);
            }
            else
            {
                GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, size.x, size.y), emptyHealthUnitTexture, emptyHealthUnitCoordinates);
            }
        }
    }

    /**
     * 
     */
    protected void DrawMana()
    {
        GUI.depth = 0;

        var container = new Rect(
            0.5f * margin * scale, // TODO: This offset should just be 1 * margin.
            2.25f * margin * scale, // TODO: This offset should be determined by the height of the health bar.
            2 * scale * collector.maximumMana,
            6 * scale
        );

        DrawRectangle(container, Color.white);

        var background = new Rect(
            container.x + 1 * scale,
            container.y + 1 * scale,
            container.width - 2 * scale,
            container.height - 2 * scale
        );

        DrawRectangle(background, new Color(0.15f, 0.15f, 0.15f));

        var mana = new Rect(
            background.x,
            background.y,
            background.width * ((float) collector.currentMana / collector.maximumMana),
            background.height
        );

        DrawRectangle(mana, new Color(0.22f, 0.71f, 0.29f));
    }

    /**
     * 
     */
    protected void DrawCoins()
    {
        if (!coinUnitTexture)
        {
            return;
        }

        GUI.depth = 0;

        var size = new Vector2(
            16 * scale,
            16 * scale
        );

        var iconPosition = new Vector2(
            margin,
            Camera.main.pixelHeight - margin - (size.y)
        );

        GUI.DrawTextureWithTexCoords(new Rect(iconPosition.x, iconPosition.y, size.x, size.y), coinUnitTexture, coinUnitCoordinates);

        var textPosition = new Rect(
            iconPosition.x + size.x + (spacing.x / 4),
            iconPosition.y - 2, // arbitrary offset, because number rendered too low
            16 * scale,
            16 * scale
        );

        OutlinedLabel(textPosition, collector.coins.ToString(), labelLeftStyle);
    }

    /**
     * 
     */
    protected void DrawKeys()
    {
        if (!keyUnitTexture)
        {
            return;
        }

        GUI.depth = 0;

        var size = new Vector2(
            16 * scale,
            16 * scale
        );

        var iconPosition = new Vector2(
            Camera.main.pixelWidth - margin - size.x,
            Camera.main.pixelHeight - margin - size.y
        );

        GUI.DrawTextureWithTexCoords(new Rect(iconPosition.x, iconPosition.y, size.x, size.y), keyUnitTexture, keyUnitCoordinates);

        var textPosition = new Rect(
            iconPosition.x - size.x - (spacing.x / 4),
            iconPosition.y - 2, // arbitrary offset, because number rendered too low
            16 * scale,
            16 * scale
        );

        OutlinedLabel(textPosition, collector.keys.ToString(), labelRightStyle);
    }

    /**
     * 
     */
    protected void DrawButtons()
    {
        GUI.depth = 0;

        Vector2 size = new Vector2(
            16 * scale,
            16 * scale
        );

        DrawAButton(size);
        DrawBButton(size);
        DrawXButton(size);
        DrawYButton(size);
        DrawLButton(size);
        DrawRButton(size);
    }

    /**
     * 
     */
    protected void DrawAButton(Vector2 size)
    {
        Vector2 aIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 2.25f),
            margin + (size.y * 2)
        );

        if (aButtonTexture)
        {
            Color oldColor = GUI.color;
            GUI.color = buttonColor;
            GUI.DrawTextureWithTexCoords(new Rect(aIconPosition.x, aIconPosition.y, size.x, size.y), aButtonTexture, aButtonCoordinates);
            GUI.color = oldColor;
        }

        if (player.aButtonLabel != null)
        {
            var aLabelPosition = new Rect(
                aIconPosition.x,
                aIconPosition.y,
                16 * scale,
                16 * scale
            );

            OutlinedLabel(aLabelPosition, player.aButtonLabel, labelCenterStyle);
        }
    }

    /**
     * 
     */
    protected void DrawBButton(Vector2 size)
    {
        Vector2 bIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 1.5f),
            margin + (size.y * 1.25f)
        );

        if (bButtonTexture)
        {
            Color oldColor = GUI.color;
            GUI.color = buttonColor;
            GUI.DrawTextureWithTexCoords(new Rect(bIconPosition.x, bIconPosition.y, size.x, size.y), bButtonTexture, bButtonCoordinates);
            GUI.color = oldColor;
        }

        if (player.bButtonLabel != null)
        {
            var bLabelPosition = new Rect(
                bIconPosition.x,
                bIconPosition.y,
                16 * scale,
                16 * scale
            );

            OutlinedLabel(bLabelPosition, player.bButtonLabel, labelCenterStyle);
        }
    }

    /**
     * 
     */
    protected void DrawXButton(Vector2 size)
    {
        Vector2 xIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 3f),
            margin + (size.y * 1.25f)
        );

        if (xButtonTexture)
        {
            Color oldColor = GUI.color;
            GUI.color = buttonColor;
            GUI.DrawTextureWithTexCoords(new Rect(xIconPosition.x, xIconPosition.y, size.x, size.y), xButtonTexture, xButtonCoordinates);
            GUI.color = oldColor;
        }

        if (player.xButtonLabel != null)
        {
            var xLabelPosition = new Rect(
                xIconPosition.x,
                xIconPosition.y,
                16 * scale,
                16 * scale
            );

            OutlinedLabel(xLabelPosition, player.xButtonLabel, labelCenterStyle);
        }
    }

    /**
     * 
     */
    protected void DrawYButton(Vector2 size)
    {
        Vector2 yIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 2.25f),
            margin + (size.y * 0.5f)
        );

        if (yButtonTexture)
        {
            Color oldColor = GUI.color;
            GUI.color = buttonColor;
            GUI.DrawTextureWithTexCoords(new Rect(yIconPosition.x, yIconPosition.y, size.x, size.y), yButtonTexture, yButtonCoordinates);
            GUI.color = oldColor;
        }

        if (player.tertiaryItem)
        {
            var yButtonActionSprite = player.tertiaryItem.GetComponent<SpriteRenderer>().sprite;

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

                GUI.DrawTextureWithTexCoords(new Rect(yIconPosition.x + padding.x * scale, yIconPosition.y + padding.y * scale, size.x - 2 * padding.x * scale, size.y - 2 * padding.y * scale), yButtonActionTexture, yButtonActionCoordinates);
            }

            if (player.tertiaryItem.RequiresAmmo())
            {
                var yLabelPosition = new Rect(
                    yIconPosition.x + (4 * scale),
                    yIconPosition.y + (4 * scale),
                    16 * scale,
                    16 * scale
                );

                OutlinedLabel(yLabelPosition, player.tertiaryItem.GetAmmo().ToString(), labelCenterStyle);
            }
        }
    }

    /**
     * 
     */
    protected void DrawLButton(Vector2 size)
    {
        Vector2 lIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 3.5f),
            margin
        );

        if (lButtonTexture)
        {
            Color oldColor = GUI.color;
            GUI.color = buttonColor;
            GUI.DrawTextureWithTexCoords(new Rect(lIconPosition.x, lIconPosition.y, size.x, size.y), lButtonTexture, lButtonCoordinates);
            GUI.color = oldColor;
        }

        if (player.secondaryItem)
        {
            var lButtonActionSprite = player.secondaryItem.GetComponent<SpriteRenderer>().sprite;

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

                GUI.DrawTextureWithTexCoords(new Rect(lIconPosition.x + padding.x * scale, lIconPosition.y + padding.y * scale, size.x - 2 * padding.x * scale, size.y - 2 * padding.y * scale), lButtonActionTexture, lButtonActionCoordinates);
            }

            if (player.secondaryItem.RequiresAmmo())
            {
                var lLabelPosition = new Rect(
                    lIconPosition.x + (4 * scale),
                    lIconPosition.y + (4 * scale),
                    16 * scale,
                    16 * scale
                );

                OutlinedLabel(lLabelPosition, player.secondaryItem.GetAmmo().ToString(), labelCenterStyle);
            }
        }
    }

    /**
     * 
     */
    protected void DrawRButton(Vector2 size)
    {
        Vector2 rIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - size.x,
            margin
        );

        if (rButtonTexture)
        {
            Color oldColor = GUI.color;
            GUI.color = buttonColor;
            GUI.DrawTextureWithTexCoords(new Rect(rIconPosition.x, rIconPosition.y, size.x, size.y), rButtonTexture, rButtonCoordinates);
            GUI.color = oldColor;
        }

        if (player.primaryItem)
        {
            var rButtonActionSprite = player.primaryItem.GetComponent<SpriteRenderer>().sprite;

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

                GUI.DrawTextureWithTexCoords(new Rect(rIconPosition.x + padding.x * scale, rIconPosition.y + padding.y * scale, size.x - 2 * padding.x * scale, size.y - 2 * padding.y * scale), rButtonActionTexture, rButtonActionCoordinates);
            }

            if (player.primaryItem.RequiresAmmo())
            {
                var rLabelPosition = new Rect(
                    rIconPosition.x + (4 * scale),
                    rIconPosition.y + (4 * scale),
                    16 * scale,
                    16 * scale
                );

                OutlinedLabel(rLabelPosition, player.primaryItem.GetAmmo().ToString(), labelCenterStyle);
            }
        }
    }

    /**
     * 
     */
    protected void DrawRectangle(Rect rectangle, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        GUIStyle rectangleStyle = new GUIStyle();
        rectangleStyle.normal.background = texture;

        GUI.Box(rectangle, GUIContent.none, rectangleStyle);
    }

    /**
     * 
     */
    protected void OutlinedLabel(Rect rect, string text, GUIStyle style)
    {
        OutlinedLabel(rect, text, style, labelOutlineColor);
    }

    /**
     * 
     */
    protected void OutlinedLabel(Rect rect, string text, GUIStyle style, Color outlineColor)
    {
        OutlinedLabel(rect, text, style, outlineColor, labelOutlineThickness * scale);
    }

    /**
     * 
     */
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
