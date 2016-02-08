using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {

	private Collector collector;
	private Damagable damagable;

    public float scale = 1f;
	public int margin;

	public Font font;

	public Sprite fullHealthUnitSprite;
	public Sprite emptyHealthUnitSprite;
	public Sprite coinUnitSprite;
	public Sprite keyUnitSprite;

	private GUIStyle labelLeftStyle;
	private GUIStyle labelRightStyle;

	public Vector2 spacing = new Vector2(
        8,
        8
    );

    private Texture2D fullHealthUnitTexture;
	private Rect fullHealthUnitRectanlge;
	private Rect fullHealthUnitCoordinates;

	private Texture2D emptyHealthUnitTexture;
	private Rect emptyHealthUnitRectanlge;
	private Rect emptyHealthUnitCoordinates;

	private Texture2D coinUnitTexture;
	private Rect coinUnitRectanlge;
	private Rect coinUnitCoordinates;

	private Texture2D keyUnitTexture;
	private Rect keyUnitRectanlge;
	private Rect keyUnitCoordinates;

	void Start() {

        scale = scale * Camera.main.orthographicSize;

		collector = GetComponent<Collector>();

		damagable = GetComponent<Damagable>();

		labelLeftStyle = new GUIStyle();
		labelLeftStyle.alignment = TextAnchor.MiddleLeft;
		labelLeftStyle.normal.textColor = Color.white;

		labelRightStyle = new GUIStyle();
		labelRightStyle.alignment = TextAnchor.MiddleRight;
		labelRightStyle.normal.textColor = Color.white;

		if (fullHealthUnitSprite) {
			fullHealthUnitTexture = fullHealthUnitSprite.texture;
			fullHealthUnitRectanlge = fullHealthUnitSprite.textureRect;
			fullHealthUnitCoordinates = new Rect(
				fullHealthUnitRectanlge.x / fullHealthUnitTexture.width,
				fullHealthUnitRectanlge.y / fullHealthUnitTexture.height,
				fullHealthUnitRectanlge.width / fullHealthUnitTexture.width,
				fullHealthUnitRectanlge.height / fullHealthUnitTexture.height
            );
		}

		if (emptyHealthUnitSprite) {
			emptyHealthUnitTexture = emptyHealthUnitSprite.texture;
			emptyHealthUnitRectanlge = emptyHealthUnitSprite.textureRect;
			emptyHealthUnitCoordinates = new Rect(
				emptyHealthUnitRectanlge.x / emptyHealthUnitTexture.width,
				emptyHealthUnitRectanlge.y / emptyHealthUnitTexture.height,
				emptyHealthUnitRectanlge.width / emptyHealthUnitTexture.width,
				emptyHealthUnitRectanlge.height / emptyHealthUnitTexture.height
            );
		}

		if (coinUnitSprite) {
			coinUnitTexture = coinUnitSprite.texture;
			coinUnitRectanlge = coinUnitSprite.textureRect;
			coinUnitCoordinates = new Rect(
				coinUnitRectanlge.x / coinUnitTexture.width,
				coinUnitRectanlge.y / coinUnitTexture.height,
				coinUnitRectanlge.width / coinUnitTexture.width,
				coinUnitRectanlge.height / coinUnitTexture.height
            );
		}

		if (keyUnitSprite) {
			keyUnitTexture = keyUnitSprite.texture;
			keyUnitRectanlge = keyUnitSprite.textureRect;
			keyUnitCoordinates = new Rect(
				keyUnitRectanlge.x / keyUnitTexture.width,
				keyUnitRectanlge.y / keyUnitTexture.height,
				keyUnitRectanlge.width / keyUnitTexture.width,
				keyUnitRectanlge.height / keyUnitTexture.height
            );
		}

	}

	void OnGUI() {

		GUI.skin.font = font;

        labelLeftStyle.fontSize = Mathf.RoundToInt(6 * scale);
        labelRightStyle.fontSize = Mathf.RoundToInt(6 * scale);

        DrawHealth();

		DrawCoins();

		DrawKeys();

	}

	void DrawHealth() {

		if (!damagable || !fullHealthUnitTexture || !emptyHealthUnitTexture) {
			return;
		}

		for (int i = 0; i < damagable.maximumHealth; i++) {

			var position = new Vector2(
				margin + spacing.x * (i % 10) * scale,
				margin + spacing.y * Mathf.Ceil(i / 10) * scale
			);

			var size = new Vector2(
				16 * scale,
				16 * scale
            );

			if (i < damagable.currentHealth) {
				GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, size.x, size.y), fullHealthUnitTexture, fullHealthUnitCoordinates);
			} else {
				GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y, size.x, size.y), emptyHealthUnitTexture, emptyHealthUnitCoordinates);
			}

		}

	}

	void DrawCoins() {

		if (!collector || !coinUnitTexture) {
			return;
		}

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

	void DrawKeys() {

		if (!collector || !keyUnitTexture) {
			return;
		}

		var size = new Vector2(
			16 * scale,
			16 * scale
        );
		
		var iconPosition = new Vector2(
			Camera.main.pixelWidth - margin - size.x,
			Camera.main.pixelHeight - margin - size.y
        );
		
		GUI.DrawTextureWithTexCoords(new Rect(iconPosition.x, iconPosition.y, size.x, size.y), coinUnitTexture, keyUnitCoordinates);

		var textPosition = new Rect(
			iconPosition.x - size.x - (spacing.x / 4),
			iconPosition.y - 2, // arbitrary offset, because number rendered too low
			16 * scale,
			16 * scale
        );

		GUI.Label(textPosition, collector.keys.ToString(), labelRightStyle);

	}
}
