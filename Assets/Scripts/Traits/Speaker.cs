using UnityEngine;
using System.Collections;

public class Speaker : MonoBehaviour
{
	public string[] speech;
	public Vector2 speechOffset;
	public Vector2 speechSize = new Vector2(320, 100);
	public float speechDistance = 5f;
	public bool autoSpeak;

	private int speechIndex = -1;
	private Player listener;

	public void OnTriggerEnter2D(Collider2D collider)
    {
		if (autoSpeak)
        {
			var player = collider.GetComponent<Player>();

			if (player)
            {
				StartSpeaking(player);
			}
		}
	}

	public void OnInteraction(Player player)
    {
		if (!IsSpeaking())
        {
			StartSpeaking(player);
		}
        else if (speechIndex < speech.Length - 1)
        {
			ContinueSpeaking();
		}
        else
        {
			FinishSpeaking();
		}
	}

	private void StartSpeaking(Player player)
    {
		listener = player;
		speechIndex = 0;
	}

	private void ContinueSpeaking()
    {
		speechIndex++;
	}

	private void FinishSpeaking()
    {
		speechIndex = -1;
	}

	public bool IsSpeaking()
    {
		return speechIndex > -1;
	}

	public void OnGUI()
    {
        if (!IsSpeaking())
        {
            return;
        }

		float distance2D = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(listener.transform.position.x, listener.transform.position.y));
		if (distance2D > speechDistance)
        {
			FinishSpeaking();
			return;
		}

		Vector3 positionOnCamera = Camera.main.WorldToScreenPoint(transform.position + new Vector3(speechOffset.x, speechOffset.y, 0));
		Rect textBoxPosition = new Rect(positionOnCamera.x - speechSize.x / 2, Screen.height - positionOnCamera.y - speechSize.y, speechSize.x, speechSize.y);

		Texture2D speechBackground = new Texture2D(1, 1);
		speechBackground.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
		speechBackground.Apply();

		GUIStyle speechStyle = GUI.skin.GetStyle("Box");
        speechStyle.padding = new RectOffset(20, 20, 20, 20);
		speechStyle.normal.background = speechBackground;
		speechStyle.alignment = TextAnchor.MiddleCenter;
		speechStyle.wordWrap = true;

		GUI.Box(
			textBoxPosition,
			speech[speechIndex],
			speechStyle
		);
	}
}
