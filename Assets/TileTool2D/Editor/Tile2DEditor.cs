using UnityEngine;
using System.Collections;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(Tile2D))]
public class Tile2DEditor : Editor {
	public override void OnInspectorGUI() {
		DrawCustomInspector();
		DrawDefaultInspector();
	}

	public void DrawCustomInspector() {
		if (PrefabUtility.GetPrefabParent(Selection.activeObject) != null) return;
		//EditorGUILayout.BeginHorizontal("Box");
		//GUIStyle lab = new GUIStyle();
		//lab.richText = true;
		//GUILayout.Label("Correct sprite naming is required to auto-fill arrays\n\n<b>NAME_SEGMENT TYPE_NUMBER</b>\n\nExample:\nGRASS_C_4", lab);
		//EditorGUILayout.EndHorizontal();
		if (GUILayout.Button("Find and apply sprite properties")) {
			Fill();
		}
	}

	public Sprite[] FindTiles(ArrayList sprites, string nam, string templateNum) {
		ArrayList tileList = new ArrayList();
		for (int i = 0; i < sprites.Count; i++) {
			Sprite s = (Sprite)sprites[i];
			string[] splitString = s.name.Split("_"[0]);
			if (splitString[1].ToUpper() == nam.ToUpper()) {
				tileList.Add(s);
			}
			if (splitString[1].ToUpper() == templateNum.ToUpper()) {
				tileList.Add(s);
			}
		}
		return (Sprite[])tileList.ToArray(typeof(Sprite));
	}

	public void Fill() {
		Tile2D myTarget = (Tile2D)target;
		myTarget.cacheRenderer = myTarget.GetComponent<SpriteRenderer>();
		string tileName = myTarget.tileName;
		if (myTarget.tileTexture == null) {
			Debug.LogError("TileTool2D: Assign Texture containing multiple Sprites to [Tile Texture] property. (Texture must be in Resources folder)");
			return;
		}
		if (tileName == "") {
			Debug.LogWarning("TileTool2D: Assign Sprite Name to look for in the Tile Name property\nTrying to use texture name: "+ myTarget.tileTexture.name);
			tileName = myTarget.tileName = myTarget.tileTexture.name;
		}
		Texture tex = myTarget.tileTexture;
		Sprite[] sprites = Resources.LoadAll<Sprite>(tex.name);
		ArrayList fooList = new ArrayList();
		for (int i = 0; i < sprites.Length; i++) {
			string[] splitString = sprites[i].name.Split("_"[0]);
			if (splitString[0].ToUpper() == tileName.ToUpper()) {
				fooList.Add(sprites[i]);
			}
		}
		if (myTarget.tileType == "") myTarget.tileType = myTarget.tileName;
		string log = "TileTool2D: Couldn't find MAIN tile types :";
		string olog = "TileTool2D: Couldn't find OPTIONAL tile types :";

		myTarget.tileA = FindTiles(fooList, "A", "0");
		if (myTarget.tileA.Length == 0) log = log + " A";
		myTarget.tileC = FindTiles(fooList, "C", "4");
		if (myTarget.tileC.Length == 0) log = log + " - C";
		myTarget.tileCE = FindTiles(fooList, "CE", "15");
		if (myTarget.tileCE.Length == 0) log = log + " - CE";
		myTarget.tileCN = FindTiles(fooList, "CN", "5");
		if (myTarget.tileCN.Length == 0) log = log + " - CN";
		myTarget.tileCS = FindTiles(fooList, "CS", "6");
		if (myTarget.tileCS.Length == 0) log = log + " - CS";
		myTarget.tileCW = FindTiles(fooList, "CW", "14");
		if (myTarget.tileCW.Length == 0) log = log + " - CW";
		myTarget.tileE = FindTiles(fooList, "E", "3");
		if (myTarget.tileE.Length == 0) log = log + " - E";
		myTarget.tileN = FindTiles(fooList, "N", "9");
		if (myTarget.tileN.Length == 0) log = log + " - N";
		myTarget.tileNE = FindTiles(fooList, "NE", "8");
		if (myTarget.tileNE.Length == 0) log = log + " - NE";
		myTarget.tileNS = FindTiles(fooList, "NS", "2");
		if (myTarget.tileNS.Length == 0) log = log + " - NS";
		myTarget.tileNW = FindTiles(fooList, "NW", "7");
		if (myTarget.tileNW.Length == 0) log = log + " - NW";
		myTarget.tileS = FindTiles(fooList, "S", "23");
		if (myTarget.tileS.Length == 0) log = log + " - S";
		myTarget.tileSE = FindTiles(fooList, "SE", "22");
		if (myTarget.tileSE.Length == 0) log = log + " - SE";
		myTarget.tileSW = FindTiles(fooList, "SW", "21");
		if (myTarget.tileSW.Length == 0) log = log + " - SW";
		myTarget.tileW = FindTiles(fooList, "W", "1");
		if (myTarget.tileW.Length == 0) log = log + " - W";
		myTarget.tileWE = FindTiles(fooList, "WE", "16");
		if (myTarget.tileWE.Length == 0) log = log + " - WE";

		myTarget.tileCNW = FindTiles(fooList, "CNW", "26");
		if (myTarget.tileCNW.Length == 0) olog = olog + " CNW";

		myTarget.tileCNE = FindTiles(fooList, "CNE", "24");
		if (myTarget.tileCNE.Length == 0) olog = olog + " - CNE";

		myTarget.tileCSW = FindTiles(fooList, "CSW", "12");
		if (myTarget.tileCSW.Length == 0) olog = olog + " - CSW";

		myTarget.tileCSE = FindTiles(fooList, "CSE", "10");
		if (myTarget.tileCSE.Length == 0) olog = olog + " - CSE";

		myTarget.tileCNWE = FindTiles(fooList, "CNWE", "25");
		if (myTarget.tileCNWE.Length == 0) olog = olog + " - CNWE";

		myTarget.tileCENS = FindTiles(fooList, "CENS", "17");
		if (myTarget.tileCENS.Length == 0) olog = olog + " - CENS";

		myTarget.tileCSWE = FindTiles(fooList, "CSWE", "11");
		if (myTarget.tileCSWE.Length == 0) olog = olog + " - CSWE";

		myTarget.tileCWNS = FindTiles(fooList, "CWNS", "19");
		if (myTarget.tileCWNS.Length == 0) olog = olog + " - CWNS";

		myTarget.tileCNSWE = FindTiles(fooList, "CNSWE", "18");
		if (myTarget.tileCNSWE.Length == 0) olog = olog + " - CNSWE";

		myTarget.tileCWE = FindTiles(fooList, "CWE", "13");
		if (myTarget.tileCWE.Length == 0) olog = olog + " - CWE";

		myTarget.tileCNS = FindTiles(fooList, "CNS", "27");
		if (myTarget.tileCNS.Length == 0) olog = olog + " - CNS";

		myTarget.tileCSN = FindTiles(fooList, "CSN", "34");
		if (myTarget.tileCSN.Length == 0) olog = olog + " - CSN";

		myTarget.tileCEW = FindTiles(fooList, "CEW", "20");
		if (myTarget.tileCEW.Length == 0) olog = olog + " - CEW";




		myTarget.tileCWNN = FindTiles(fooList, "CWNN", "37");
		if (myTarget.tileCWNN.Length == 0) olog = olog + " - CWNN";

		myTarget.tileCNEE = FindTiles(fooList, "CNEE", "32");
		if (myTarget.tileCNEE.Length == 0) olog = olog + " - CNEE";

		myTarget.tileCSWW = FindTiles(fooList, "CSWW", "40");
		if (myTarget.tileCSWW.Length == 0) olog = olog + " - CSWW";

		myTarget.tileCESS = FindTiles(fooList, "CESS", "31");
		if (myTarget.tileCESS.Length == 0) olog = olog + " - CESS";

		myTarget.tileCNSS = FindTiles(fooList, "CNSS", "33");
		if (myTarget.tileCNSS.Length == 0) olog = olog + " - CNSS";

		myTarget.tileCENN = FindTiles(fooList, "CENN", "38");
		if (myTarget.tileCENN.Length == 0) olog = olog + " - CENN";

		myTarget.tileCWSS = FindTiles(fooList, "CWSS", "30");
		if (myTarget.tileCWSS.Length == 0) olog = olog + " - CWSS";

		myTarget.tileCSEE = FindTiles(fooList, "CSEE", "39");
		if (myTarget.tileCSEE.Length == 0) olog = olog + " - CSEE";

		myTarget.tileSENW = FindTiles(fooList, "SENW", "36");
		if (myTarget.tileSENW.Length == 0) olog = olog + " - SENW";

		myTarget.tileSWNE = FindTiles(fooList, "SWNE", "35");
		if (myTarget.tileSWNE.Length == 0) olog = olog + " - SWNE";

		myTarget.tileNESW = FindTiles(fooList, "NESW", "29");
		if (myTarget.tileNESW.Length == 0) olog = olog + " - NESW";

		myTarget.tileNWSE = FindTiles(fooList, "NWSE", "28");
		if (myTarget.tileNWSE.Length == 0) olog = olog + " - NWSE";

		myTarget.tileCENSW = FindTiles(fooList, "CENSW", "43");
		if (myTarget.tileCENSW.Length == 0) olog = olog + " - CENSW";

		myTarget.tileCNWES = FindTiles(fooList, "CNWES", "44");
		if (myTarget.tileCNWES.Length == 0) olog = olog + " - CNWES";

		myTarget.tileCSWEN = FindTiles(fooList, "CSWEN", "45");
		if (myTarget.tileCSWEN.Length == 0) olog = olog + " - CSWEN";

		myTarget.tileCWNSE = FindTiles(fooList, "CWNSE", "46");
		if (myTarget.tileCWNSE.Length == 0) olog = olog + " - CWNSE";



		myTarget.tileCNWSE = FindTiles(fooList, "CNWSE", "41");
		if (myTarget.tileCNWSE.Length == 0) olog = olog + " - CNWSE";

		myTarget.tileCNESW = FindTiles(fooList, "CNESW", "42");
		if (myTarget.tileCNESW.Length == 0) olog = olog + " - CNESW";


		if (log != "TileTool2D: Couldn't find MAIN tile types :") Debug.LogWarning(log + "(textures must be in the resource folder while auto detecting)");
		if (olog != "TileTool2D: Couldn't find OPTIONAL tile types :") Debug.LogWarning(olog);
		if (myTarget.tileA.Length > 0) myTarget.cacheRenderer.sprite = myTarget.tileA[0];
		else if (myTarget.tileCN.Length > 0) myTarget.cacheRenderer.sprite = myTarget.tileCN[0];
		else if (myTarget.tileCE.Length > 0) myTarget.cacheRenderer.sprite = myTarget.tileCE[0];
		else if (myTarget.tileCS.Length > 0) myTarget.cacheRenderer.sprite = myTarget.tileCS[0];		
		else if (myTarget.tileCW.Length > 0) myTarget.cacheRenderer.sprite = myTarget.tileCW[0];
		else if (myTarget.tileC.Length > 0) myTarget.cacheRenderer.sprite = myTarget.tileC[0];
		else Debug.LogWarning("Please assign default sprite to tile manually");
		EditorUtility.SetDirty(myTarget);
		string spritePath = AssetDatabase.GetAssetPath(myTarget);
		AssetDatabase.ImportAsset(spritePath, ImportAssetOptions.ForceUpdate);
	}
}