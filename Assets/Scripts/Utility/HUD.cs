using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{

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

    private GUIStyle labelLeftStyle;
    private GUIStyle labelCenterStyle;
    private GUIStyle labelRightStyle;

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

    void Start()
    {
        scale = scale * Camera.main.orthographicSize;

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

    void OnGUI()
    {
        GUI.skin.font = font;

        labelLeftStyle.fontSize = Mathf.RoundToInt(6 * scale);
        labelCenterStyle.fontSize = Mathf.RoundToInt(6 * scale);
        labelRightStyle.fontSize = Mathf.RoundToInt(6 * scale);

        DrawHealth();

        DrawCoins();

        DrawKeys();

        DrawButtons();

    }

    void DrawHealth()
    {

        if (!damagable || !fullHealthUnitTexture || !emptyHealthUnitTexture)
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
            else {
                GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, size.x, size.y), emptyHealthUnitTexture, emptyHealthUnitCoordinates);
            }

        }

    }

    void DrawCoins()
    {

        if (!collector || !coinUnitTexture)
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

        GUI.Label(textPosition, collector.coins.ToString(), labelLeftStyle);

    }

    void DrawKeys()
    {

        if (!collector || !keyUnitTexture)
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

        GUI.Label(textPosition, collector.keys.ToString(), labelRightStyle);

    }

    void DrawButtons()
    {
        if (!collector || !aButtonTexture || !bButtonTexture || !xButtonTexture || !yButtonTexture || !lButtonTexture || !rButtonTexture)
        {
            return;
        }

        GUI.depth = 0;

        var size = new Vector2(
            16 * scale,
            16 * scale
        );

        var aIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 2.25f),
            margin + (size.y * 2)
        );

        var bIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 1.5f),
            margin + (size.y * 1.25f)
        );

        var xIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 3f),
            margin + (size.y * 1.25f)
        );

        var yIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 2.25f),
            margin + (size.y * 0.5f)
        );

        var lIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - (size.x * 3.5f),
            margin
        );

        var rIconPosition = new Vector2(
            Camera.main.pixelWidth - margin - size.x,
            margin
        );

        GUI.DrawTextureWithTexCoords(new Rect(aIconPosition.x, aIconPosition.y, size.x, size.y), aButtonTexture, aButtonCoordinates);
        GUI.DrawTextureWithTexCoords(new Rect(bIconPosition.x, bIconPosition.y, size.x, size.y), bButtonTexture, bButtonCoordinates);
        GUI.DrawTextureWithTexCoords(new Rect(xIconPosition.x, xIconPosition.y, size.x, size.y), xButtonTexture, xButtonCoordinates);
        GUI.DrawTextureWithTexCoords(new Rect(yIconPosition.x, yIconPosition.y, size.x, size.y), yButtonTexture, yButtonCoordinates);
        GUI.DrawTextureWithTexCoords(new Rect(lIconPosition.x, lIconPosition.y, size.x, size.y), lButtonTexture, lButtonCoordinates);
        GUI.DrawTextureWithTexCoords(new Rect(rIconPosition.x, rIconPosition.y, size.x, size.y), rButtonTexture, rButtonCoordinates);

        Player player = collector.GetComponent<Player>();

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
        }

        if (player.aButtonLabel != null)
        {
            var aLabelPosition = new Rect(
                aIconPosition.x,
                aIconPosition.y,
                16 * scale,
                16 * scale
            );

            GUI.Label(aLabelPosition, player.aButtonLabel, labelCenterStyle);
        }

        if (player.bButtonLabel != null)
        {
            var bLabelPosition = new Rect(
                bIconPosition.x,
                bIconPosition.y,
                16 * scale,
                16 * scale
            );

            GUI.Label(bLabelPosition, player.bButtonLabel, labelCenterStyle);
        }

        if (player.xButtonLabel != null)
        {
            var xLabelPosition = new Rect(
                xIconPosition.x,
                xIconPosition.y,
                16 * scale,
                16 * scale
            );

            GUI.Label(xLabelPosition, player.xButtonLabel, labelCenterStyle);
        }
    }
}
