/****************************************
TileTool2D v1.0
Copyright 2015 Unluck Software	
www.chemicalbliss.com
*****************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityEditorInternal;
using System.Reflection;


[Serializable]
public class TileTool2D : EditorWindow {
	//Window
	public static EditorWindow win;

	//GUI Textures
	[SerializeField]
	public Texture select;
	public static Texture brush;
	public static Texture brushActive;
	public static Texture eraser;
	public static Texture eraserActive;
	public static Texture magicWand;
	public static Texture magicWandActive;
	public static Texture snapT;
	public static Texture snapTActive;

	//Drawing
	[SerializeField]
	public GameObject drawField;
	[SerializeField]
	public Vector2 currentDrawPos;
	[SerializeField]
	public GameObject drawTile;
	public static bool mouseDown;
	[SerializeField]
	public bool draw = true;

	//Erasing
	public static bool erase;
	[SerializeField]
	public bool eraseLock;
	[SerializeField]
	public float eraseSize = 0.1f;

	//Fixing
	public static bool magic;
	[SerializeField]
	public bool magicLock;

	//Layers
	public static GameObject layerHolder;
	public static Vector2 layerScroll;
	[SerializeField]
	public int currentLayer;
	[SerializeField]
	public bool onlyVisible = false;

	//Preview
	[SerializeField]
	public GameObject[] tiles;
	[SerializeField]
	public Vector2 scrollPos;
	[SerializeField]
	public int spriteSize = 64;

	//Snapping
	public Vector3 prevPosition;
	public bool doSnap = false;
	public float snapValue = 1.0f;
	public Transform[] _selection;

	//GUI Style
	public static GUIStyle invisibleButton;
	public int invisibleButtonMargin = 2;
	public GUIStyle smallButton;
	static public bool alt;
	[SerializeField]
	public float adjust = 0.32f;
	[SerializeField]
	public string[] sets;
	[SerializeField]
	public string currentSet;
	[SerializeField]
	public int _index = 0;
	[SerializeField]
	public int layerCount;

	//Unity sorting layers
	[SerializeField]
	public String[] sortingLayers;

	private bool warningSort;

	[MenuItem("Window/TileTool2D/TileTool2D")]
	public static void ShowWindow() {
		win = EditorWindow.GetWindow(typeof(TileTool2D));
		win.titleContent = new GUIContent(" TileTool2D", Resources.Load<Texture>("GUITextures/TileTool2DIcon"));
		win.minSize = new Vector2(415.0f, 400.0f);
	}

	public void OnEnable() {
		CreateDrawField();
		CreateLayerHolder();
		EnableSceneGUI(true);
		sortingLayers = GetSortingLayerNames();
		LoadGUITextures();
		LoadSet();
		if (drawTile == null) drawTile = tiles[0].gameObject;
	}

	public void LoadGUITextures() {
		if (select == null) select = Resources.Load<Texture>("GUITextures/TT2DSelected");
		if (brush == null) brush = Resources.Load<Texture>("GUITextures/TT2DBrush");
		if (brushActive == null) brushActive = Resources.Load<Texture>("GUITextures/TT2DBrushActive");
		if (eraser == null) eraser = Resources.Load<Texture>("GUITextures/TT2DEraser");
		if (eraserActive == null) eraserActive = Resources.Load<Texture>("GUITextures/TT2DEraserActive");
		if (magicWand == null) magicWand = Resources.Load<Texture>("GUITextures/TT2DMagicWand");
		if (magicWandActive == null) magicWandActive = Resources.Load<Texture>("GUITextures/TT2DMagicWandActive");
		if (snapT == null) snapT = Resources.Load<Texture>("GUITextures/TT2Dsnap");
		if (snapTActive == null) snapTActive = Resources.Load<Texture>("GUITextures/TT2DsnapActive");
	}

	public void OnDisable() {
		RemoveDrawField();
		EnableSceneGUI(false);
	}

	public void EnableSceneGUI(bool enabled) {
		SceneView.onSceneGUIDelegate -= OnSceneGUI;
		if (enabled) SceneView.onSceneGUIDelegate += OnSceneGUI;
	}

	// Adjusts scroll windows based on how many layers there are
	// Saves amount of layers then checks if that number has changed >> Sorts all sprite sorting orders if amount of layers has changed
	public void OnFocus() {
		adjust = AdjustLayerScrollSize();
		if (layerCount == 0) {
			layerCount = layerHolder.transform.childCount;
			return;
		}
		if (layerCount != layerHolder.transform.childCount) {
			SortAll();
			layerCount = layerHolder.transform.childCount;
		}
	}

	// Returns string based on draw mode 
	public string ModeText() {
		if (Selection.activeGameObject) return ("Off     ");
		if (erase) return ("Erase   ");	
		if (magic) return ("Fix     ");	
		if (draw) return ("Draw    ");		
		return ("Off");
	}

	// Handles everything that happens in scene view, updates as long as window is open
	public void OnSceneGUI(SceneView sceneview) {
		if (Selection.activeGameObject) {           // Disable if something is selected
			return;
		}
		if (Event.current.type == EventType.layout) HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		Event e = Event.current;
		if (e.shift && e.control) alt = true; else alt = false;
		if (!alt && (e.shift || eraseLock)) erase = true; else erase = false;
		if (!alt && (e.control || magicLock)) magic = true; else magic = false;
		if (draw || erase || magic) {
			Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
			RaycastHit hit = new RaycastHit();
			Tile2D t = (Tile2D)drawTile.GetComponent(typeof(Tile2D));
			if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) {
				if (currentDrawPos.x != Round(hit.point.x, t.tileSize) || currentDrawPos.y != Round(hit.point.y, t.tileSize)) {
					if (e.button == 0 && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown)) {
						if (layerHolder.transform.childCount == 0) CreateLayer();
						if (currentLayer > layerHolder.transform.childCount - 1) currentLayer = 0;
						if (!layerHolder.transform.GetChild(currentLayer).gameObject.activeInHierarchy) return;
						Draw(hit.point);
						Erase(hit.point);
						Magic(hit.point);
					}
				}
			}

			if (erase || magic) {
				if (erase) Handles.color = new Color32((byte)255, (byte)0, (byte)0, (byte)100);
				if (magic) Handles.color = new Color32((byte)0, (byte)255, (byte)0, (byte)100);
				float eSize = Mathf.Max(1.0f, eraseSize*2);
				Vector3 pos = new Vector3(hit.point.x, hit.point.y, 0.0f);
				if (eSize == 1) pos = new Vector3(Mathf.FloorToInt(hit.point.x) + .5f, Mathf.FloorToInt(hit.point.y) + .5f, 0.0f);
				Handles.CubeCap(1, pos, Quaternion.identity, eSize);
				return;
			}
			Handles.color = new Color32((byte)155, (byte)155, (byte)255, (byte)100);
			Vector2 d;
			d.x = Round(hit.point.x, t.tileSize);
			d.y = Round(hit.point.y, t.tileSize);
			Handles.CubeCap(1, new Vector3(d.x + t.tileSize*0.5f, d.y + t.tileSize*0.5f, 0.0f), Quaternion.identity, t.tileSize);
		}
		if (layerHolder && layerHolder.name != "TileTool2DLayers") {
			Debug.LogWarning("TileTool2D: Can't rename TileTool2DLayers");
			layerHolder.name = "TileTool2DLayers";
		}
	}

	public void Draw(Vector3 hit) {
		Tile2D t = (Tile2D)drawTile.GetComponent(typeof(Tile2D));
		currentDrawPos.x = Round(hit.x, t.tileSize);
		currentDrawPos.y = Round(hit.y, t.tileSize);
		if (draw && !erase && !magic) {
			GameObject dupeTarget = (GameObject)PrefabUtility.InstantiatePrefab(drawTile);
			t = (Tile2D)dupeTarget.transform.GetComponent(typeof(Tile2D));
			dupeTarget.transform.parent = layerHolder.transform.GetChild(currentLayer);
			dupeTarget.transform.position = new Vector3(currentDrawPos.x, currentDrawPos.y, 0);
			t.CacheComponents();
			if (layerHolder.transform.GetChild(currentLayer).childCount > 0) t.cacheRenderer.sortingLayerName = layerHolder.transform.GetChild(currentLayer).GetChild(0).GetComponent<Renderer>().sortingLayerName;
			t.CheckOverlap();
			t.CheckIt();
			if (!alt) t.sortingOrder = t.FindCloseByHighestSortOrder();
			else t.sortingOrder = t.FindCloseByLowestSortOrder();
			if (layerHolder.transform.GetChild(currentLayer).GetChild(0).GetComponent<Renderer>().sortingLayerID == 0) t.FixSortingLayer((layerHolder.transform.childCount - currentLayer - 1) * 2000 - 30000);
			EditorUtility.SetDirty(dupeTarget);
			Undo.RegisterCreatedObjectUndo(dupeTarget, "TileTool2D: Draw");
		}
	}

	public void Erase(Vector3 hit) {
		if (erase) {
			Tile2D t = null;
			float overLapSize = eraseSize;
			Collider2D[] tiles = Physics2D.OverlapAreaAll(new Vector2(hit.x +overLapSize, hit.y +overLapSize), new Vector2(hit.x -overLapSize, hit.y -overLapSize));
			for (int i = 0; i < tiles.Length; i++) {
				if (tiles[i] != null && tiles[i].transform.parent == layerHolder.transform.GetChild(currentLayer)) {			
					tiles[i].transform.position = new Vector3(9999999.999999f, 9999999.999999f, 0);
					t = tiles[i].GetComponent<Tile2D>();
					t.BeautifyCloseByTiles();
					Undo.DestroyObjectImmediate(tiles[i].gameObject);
				}
			}
		}
	}

	public void Magic(Vector3 hit) {
		if (magic || magicLock) {
			Tile2D t = null;
			float overLapSize = eraseSize;
			Collider2D[] tiles = Physics2D.OverlapAreaAll(new Vector2(hit.x +overLapSize, hit.y +overLapSize), new Vector2(hit.x -overLapSize, hit.y -overLapSize));
			for (int i = 0; i < tiles.Length; i++) {
				t = (Tile2D)tiles[i].transform.GetComponent(typeof(Tile2D));
				if (t.transform.parent == layerHolder.transform.GetChild(currentLayer)) {
					Undo.RegisterCompleteObjectUndo(t.gameObject, "TileTool2D: Magic");
					t.savePosition = new Vector2(0.1f, 0.1f);
					t.CheckIt();
				}
			}
		}
	}

	public void Fix(Transform tr) {
		for (int i = 0; i < tr.childCount; i++) {
			float pp = (float)i;
			if (EditorUtility.DisplayCancelableProgressBar(
				"Fix Seams",
				"Fixing Seams between tiles",
				pp / tr.childCount)) {
				EditorUtility.ClearProgressBar();
				return;
			} else {
				Tile2D t = (Tile2D)tr.GetChild(i).GetComponent(typeof(Tile2D));
				Undo.ClearUndo(t.gameObject);
				Undo.RegisterCompleteObjectUndo(t.gameObject, "TileTool2D: Magic");
				t.savePosition = new Vector2(0.1f, 0.1f);
				t.FindTiles();
				t.Beautify();
				t.AddAttachment();
			}
		}
		EditorUtility.ClearProgressBar();
	}

	public void LoadSet() {
		//Debug.Log("LoadSet");
		string[] dir = Directory.GetDirectories("Assets/TileTool2D/Resources/Tiles/");
		sets = new string[dir.Length];
		for (int i = 0; i < dir.Length; i++) {
			string[] split = dir[i].Split("/"[0]);
			sets[i] = split[split.Length - 1];
		}
		//	sets[sets.Length - 1] = "All";
		//	if (currentSet == "All") currentSet = "";
		currentSet = sets[_index];
        GameObject[] _resources = Resources.LoadAll<GameObject>("Tiles/" + currentSet);
        List<GameObject> _tiles = new List<GameObject>();
        foreach (GameObject resource in _resources)
        {
            if (resource.GetComponent<Tile2D>())
            {
                _tiles.Add(resource);
            }
        }
        tiles = _tiles.ToArray();
	//	if (_index == -1) _index = sets.Length - 1;
	}

	public void OnGUI() {
		GUILayout.Space(4.0f);
		EditorGUILayout.BeginHorizontal();
		_index = EditorGUILayout.Popup(_index, sets, GUILayout.Width(Mathf.Min(position.width * 0.48f, 250)));
		spriteSize = (int)GUILayout.HorizontalSlider((float)spriteSize, 16.0f, 128.0f);
		GUILayout.EndHorizontal();	
		if (GUI.changed) LoadSet();
		//GUI Style elements
		invisibleButton = new GUIStyle();
		invisibleButton.margin = new RectOffset(invisibleButtonMargin, invisibleButtonMargin, invisibleButtonMargin, invisibleButtonMargin);
		smallButton = new GUIStyle(GUI.skin.button);
		smallButton.fixedHeight = 16.0f;
		smallButton.fontSize = 9;
		//Scroll area for preview images
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height - (115 + (position.height * adjust))));
		EditorGUILayout.BeginHorizontal();
		int row = 0;
		for (int i = 0; i < tiles.Length; i++) {
			int n = (int)((position.width/(spriteSize+invisibleButtonMargin))-.2f);
			if (GUILayout.Button(AssetPreview.GetAssetPreview(tiles[i]), invisibleButton, GUILayout.Width((float)spriteSize), GUILayout.Height((float)spriteSize))) {
				if (drawTile == tiles[i]) Selection.activeObject = tiles[i];
				drawTile = tiles[i];
			}
			if (tiles[i] == drawTile) GUI.DrawTexture(new Rect((float)((spriteSize + invisibleButtonMargin) * (i % n) + invisibleButtonMargin),
															   (float)((spriteSize + invisibleButtonMargin) * row + invisibleButtonMargin),
															   (float)spriteSize, (float)spriteSize), select);
			if (i % n == n - 1) {
				row++;
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();

		//Button and slider area
		EditorGUILayout.BeginHorizontal();
		Texture b = brush;
		if (draw) b = brushActive;
		if (GUILayout.Button(b, invisibleButton, GUILayout.Width(32.0f), GUILayout.Height(32.0f))) {
			draw = !draw;
			erase = eraseLock = magic = magicLock = false;
			if (draw) EnableSceneGUI(true); else EnableSceneGUI(false);
		}
		b = eraser;
		if (erase || eraseLock) {
			b = eraserActive;
			if (eraseLock) draw = false;
		}
		if (GUILayout.Button(b, invisibleButton, GUILayout.Width(32.0f), GUILayout.Height(32.0f))) {
			eraseLock = !eraseLock;
			magicLock = false;
			draw = false;
			if (eraseLock) EnableSceneGUI(true); else EnableSceneGUI(false);
		}
		b = magicWand;
		if (magic || magicLock) {
			b = magicWandActive;
		}
		if (GUILayout.Button(b, invisibleButton, GUILayout.Width(32.0f), GUILayout.Height(32.0f))) {
			magicLock = !magicLock;
			eraseLock = false;
			draw = false;
			if (magicLock) EnableSceneGUI(true); else EnableSceneGUI(false);
		}
		b = snapT;
		if (doSnap) {
			b = snapTActive;
		}
		if (GUILayout.Button(b, invisibleButton, GUILayout.Width(32.0f), GUILayout.Height(32.0f))) {
			doSnap = !doSnap;
		}
		GUILayout.Space(4.0f);
		EditorGUILayout.BeginVertical();
		GUILayout.Label("" + drawTile.name, GUILayout.Height(16));
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Erase", GUILayout.Width(35));
		eraseSize = GUILayout.HorizontalSlider(eraseSize, 0.1f, 4.0f, GUILayout.Height(14));
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(2.0f);

		//Scroll area for layers
		layerScroll = EditorGUILayout.BeginScrollView(layerScroll, false, false, GUILayout.Width(position.width), GUILayout.Height(position.height * adjust + 25));
		EditorGUILayout.BeginHorizontal();
		if (layerHolder != null) {
			for (int j = 0; j < layerHolder.transform.childCount; j++) {



				Transform child = layerHolder.transform.GetChild(j);


				GUI.color = Color.gray;
				if (currentLayer == j) GUI.color = Color.white;
				//Fix window width when the scrollbar appears
				int fixWidth = 9;
				if (layerHolder.transform.childCount > 4) fixWidth = 24;
				EditorGUILayout.BeginHorizontal("Box", GUILayout.Width(position.width - fixWidth));
				GUI.color = Color.white;

				string hideText = "HIDE";
				if (!child.gameObject.activeInHierarchy) {
					hideText = "SHOW";
					GUI.color = Color.red;
				}
				if (GUILayout.Button(hideText, smallButton, GUILayout.Width(45.0f))) {
					child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
				}
				GUI.color = Color.white;
				if (currentLayer == j) GUI.color = Color.cyan;
				if (GUILayout.Button("SELECT", smallButton, GUILayout.Width(55.0f))) {
					currentLayer = j;
					OnlyShowVisibleLayer(j, child.gameObject);
				}
				GUI.color = Color.white;
				child.name = EditorGUILayout.TextField(child.name, GUILayout.Width(75.0f));

				//Unity Layer Sorting
				if (layerHolder.transform.GetChild(j).childCount == 0)
					GUI.enabled = false;
				else
					GUI.enabled = true;
				int sindex = 0;
				int cindex = 0;

				if (layerHolder.transform.GetChild(j).childCount > 0) {
					cindex = sindex = GetSortIndex(layerHolder.transform.GetChild(j).GetChild(0).GetComponent<Renderer>().sortingLayerName);
				}

				sindex = EditorGUILayout.Popup(sindex, sortingLayers);
				if (cindex != sindex) {
					if (!warningSort) {
						Debug.LogWarning("TileTool2D: Unity Sorting Layers overrides TileTool2D layer sorting, use <b>Sort All</b> if going back to TileTool layers");
						warningSort = true;
					}
						
					SetLayerSortingLayers(sortingLayers[sindex], layerHolder.transform.GetChild(j));
				}
				GUI.enabled = true;
				if (j == 0)
					GUI.enabled = false;
				if (GUILayout.Button("▲", smallButton, GUILayout.Width(25.0f))) {
					LayerMove((layerHolder.transform.childCount - j) * 2000 - 30000, child);
					LayerMove((layerHolder.transform.childCount - j - 1) * 2000 - 30000, layerHolder.transform.GetChild(j - 1));
					child.SetSiblingIndex(j - 1);
					if (currentLayer == j)
						currentLayer = j - 1;
					else if (currentLayer == j - 1)
						currentLayer = j;
				}
				GUI.enabled = true;
				if (j == layerHolder.transform.childCount - 1)
					GUI.enabled = false;
				if (GUILayout.Button("▼", smallButton, GUILayout.Width(25.0f))) {
					LayerMove((layerHolder.transform.childCount - j - 2) * 2000 - 30000, child);
					LayerMove((layerHolder.transform.childCount - j - 1) * 2000 - 30000, layerHolder.transform.GetChild(j + 1));
					child.SetSiblingIndex(j + 1);
					if (currentLayer == j)
						currentLayer = j + 1;
					else if (currentLayer == j + 1)
						currentLayer = j;
				}
				GUI.enabled = true;
				GUILayout.FlexibleSpace();
				GUILayout.Label("" + child.childCount);
				if (child.childCount == 0) GUI.enabled = false;
				if (GUILayout.Button("FIX", smallButton, GUILayout.Width(40.0f)) && (child.childCount == 0 || EditorUtility.DisplayDialog("Fix Tile Seams?", "Fix " + child.name + " containing " + child.childCount + " tiles?\nThis might take several minutes.\n\nIt is recommended to save and reload scene before and after Fix.", "Yes", "No"))) {
					Fix(child);
				}
				GUI.enabled = true;
				if (GUILayout.Button("X", smallButton, GUILayout.Width(20.0f)) && (child.childCount == 0 || EditorUtility.DisplayDialog("Delete Layer?", "Delete " + child.name + " containing " + child.childCount + " tiles?", "Yes", "No"))) {

					Undo.DestroyObjectImmediate(child.gameObject);
					adjust = AdjustLayerScrollSize();
					layerCount = layerHolder.transform.childCount;
					SortAll();
				}
				EditorGUILayout.EndHorizontal();
				if (j < layerHolder.transform.childCount) {
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
				}
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();

		//Area for status text and add layer button
		GUI.color = Color.clear;
		GUILayout.BeginHorizontal("Box", GUILayout.Width(position.width - 9));
		GUI.color = Color.white;
		GUILayout.Label("Mode: " + ModeText());
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("SORT ALL", smallButton, GUILayout.Width(65.0f))) {
			SortAll();
		}
		if (GUILayout.Button("CLEAR SORT", smallButton, GUILayout.Width(80.0f))) {
			ClearSortAll();
		}
		if (GUILayout.Button("NEW LAYER", smallButton, GUILayout.Width(75.0f))) {
			CreateLayer();
		}
		EditorGUILayout.EndHorizontal();
	}

	public void OnlyShowVisibleLayer(int n, GameObject go) {
		if (onlyVisible) {
			for (int i = 0; i < layerHolder.transform.childCount; i++) {
				GameObject l = layerHolder.transform.GetChild(i).gameObject;
				if (i != currentLayer) {
					l.SetActive(false);
				} else l.SetActive(true);
			}
		}
	}

	public void LayerMove(int layerIndex, Transform layer) {
		for (int i = 0; i < layer.childCount; i++) {
			Tile2D t = layer.GetChild(i).GetComponent<Tile2D>();
			t.FixSortingLayer(layerIndex);
		}
	}

	public void SortAll() {
		if (layerHolder != null) {
			for (int i = 0; i < layerHolder.transform.childCount; i++) {
				Undo.RegisterCompleteObjectUndo(layerHolder.transform.GetChild(i).gameObject, "TileTool2D: Sort All");
				LayerMove((layerHolder.transform.childCount - i - 1) * 2000 - 30000, layerHolder.transform.GetChild(i));
			}
		}
	}

	public void ClearSortAll() {
		if (layerHolder != null) {
			Renderer[] renderers = layerHolder.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < renderers.Length; i++) {
				Undo.RegisterCompleteObjectUndo(renderers[i].transform, "TileTool 2D: Sorting Layer Changed");
				renderers[i].sortingOrder = 0;
				EditorUtility.SetDirty(renderers[i]);
			}
		}
	}

	public void OnInspectorUpdate() {
		Repaint();
		EditorUtility.ClearProgressBar();
	}


	public void Update() {
		if (Selection.transforms.Length > 0 && !EditorApplication.isPlaying && doSnap && Selection.transforms[0].position != prevPosition) snap(true);
		CreateDrawField();
		if (EditorApplication.isPlaying)
			RemoveDrawField();
		CreateLayerHolder();
	}

	public void RemoveDrawField() {
		if (drawField != null)
			DestroyImmediate(drawField);
	}

	public void CreateDrawField() {
		if (drawField == null) {
			drawField = GameObject.Find("TileTool2D");
			if (drawField == null) {
				drawField = new GameObject();
				BoxCollider b = drawField.AddComponent<BoxCollider>();
				b.size = new Vector3(9999999.0f, 9999999.0f, 0.0f);
				drawField.hideFlags = HideFlags.DontSaveInBuild;
				drawField.hideFlags = HideFlags.DontSaveInEditor;
				drawField.hideFlags = HideFlags.HideAndDontSave;
				drawField.name = "TileTool2D";
			}
		}
	}

	public void CreateLayerHolder() {
		if (layerHolder == null) {
			layerHolder = GameObject.Find("TileTool2DLayers");
			if (layerHolder == null) {
				layerHolder = new GameObject();
				layerHolder.name = "TileTool2DLayers";
				CreateLayer();
			}
		}
	}

	public void CreateLayer() {
		if (layerHolder.transform.childCount > 30) {
			Debug.LogError("TileTool2D: Max 30 layers");
			return;
		}
		if (layerHolder == null) return;
		GameObject newLayer = new GameObject();
		newLayer.name = "New Layer";
		newLayer.transform.parent = layerHolder.transform;
		newLayer.transform.SetSiblingIndex(0);
		adjust = AdjustLayerScrollSize();
		layerCount = layerHolder.transform.childCount;
		Undo.RegisterCreatedObjectUndo(newLayer, "TileTool2D: New Layer");
	}

	public float AdjustLayerScrollSize() {
		if (layerHolder.transform.childCount <= 1) return 0.02f;
		if (layerHolder.transform.childCount == 2) return 0.085f;
		if (layerHolder.transform.childCount == 3) return 0.15f;
		if (layerHolder.transform.childCount == 4) return 0.215f;
		return 0.24f;
	}

	public void snap(bool onlyTiles) {
		_selection = Selection.transforms;
		try {
			for (int i = 0; i < Selection.transforms.Length; i++) {
				Tile2D TTT = (Tile2D)_selection[i].GetComponent(typeof(Tile2D));
				if (onlyTiles && (TTT != null) || !onlyTiles) {
					if (!onlyTiles) {
						Undo.RecordObjects(_selection, "TileTool: Snapping");
					}
					Vector3 t = Selection.transforms[i].transform.position;
					t.x = Round(t.x, snapValue);
					t.y = Round(t.y, snapValue);
					t.z = Round(t.z, snapValue);
					Selection.transforms[i].transform.position = t;
				}
			}
			prevPosition = Selection.transforms[0].position;
		} catch (System.Exception e) {
			Debug.LogError("Nothing to move.  " + e);
		}
	}

	public float Round(float input, float size) {
		float snappedValue = 0.0f;
		snappedValue = size * Mathf.Floor((input / size));
		return (snappedValue);
	}

	public String[] GetSortingLayerNames() {
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (String[])sortingLayersProperty.GetValue(null, new System.Object[0]);
	}

	public int GetSortIndex(string layerName) {
		int n = 0;
		for(int i = 0; i < sortingLayers.Length; i++) {
			if(sortingLayers[i] == layerName) {
				return i;
			}
		}
		return n;
	}

	public void SetLayerSortingLayers(string layerName, Transform layer) {
		Renderer[] renderers = layer.GetComponentsInChildren<Renderer>();
		for (int i=0; i < renderers.Length; i++) {
			Undo.RegisterCompleteObjectUndo(renderers[i].transform, "TileTool 2D: Sorting Layer Changed");
			renderers[i].sortingLayerName = layerName;
		}
	}
}