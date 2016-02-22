using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//Tiles must have a box collider component
//[RequireComponent(typeof(BoxCollider2D))]
public class Tile2D : MonoBehaviour {
	public Texture tileTexture;
	public string tileName;
	public string tileType;
	//Used in cases where tiles are not using the standard 1 Unity unit scale
	public float tileSize = 1f;

	public bool passive;
	//public bool disableColliderOnAwake;

	public ParticleSystem particles;
	
	//In case a sprite needs custom gameobjects containing for example extra colliders, animations or other components.
	public Tile2DAttachment[] attachments;
	public Tile2DAttachment currentAttachment;

	//Contains the side name of this tile ("N" is north tile for example)
	//Used to identify tiles when adding attachments
	[HideInInspector]
	public string tileSprite;

	//Tile sprites
	//These are the most common tiles used in platformers
	public Sprite[] tileA;
	public Sprite[] tileC;
	public Sprite[] tileCE;
	public Sprite[] tileCN;
	public Sprite[] tileCS;
	public Sprite[] tileCW;
	public Sprite[] tileE;
	public Sprite[] tileN;
	public Sprite[] tileNE;
	public Sprite[] tileNS;
	public Sprite[] tileNW;
	public Sprite[] tileS;
	public Sprite[] tileSE;
	public Sprite[] tileSW;
	public Sprite[] tileW;
	public Sprite[] tileWE;

	//Corner tile sprites (optinal sprites)
	public Sprite[] tileCNW;
	public Sprite[] tileCNE;
	public Sprite[] tileCSE;
	public Sprite[] tileCSW;
	public Sprite[] tileCWE;
	public Sprite[] tileCNS;
	public Sprite[] tileCSN;
	public Sprite[] tileCEW;

	//Advanced tile sprites (optional sprites)
	public Sprite[] tileCWNN;
	public Sprite[] tileCNEE;
	public Sprite[] tileCSWW;
	public Sprite[] tileCESS;
	public Sprite[] tileCNSS;
	public Sprite[] tileCENN;
	public Sprite[] tileCWSS;
	public Sprite[] tileCSEE;
	public Sprite[] tileSENW;
	public Sprite[] tileSWNE;
	public Sprite[] tileNESW;
	public Sprite[] tileNWSE;
	public Sprite[] tileCNWE;
	public Sprite[] tileCENS;
	public Sprite[] tileCSWE;
	public Sprite[] tileCWNS;
	public Sprite[] tileCENSW;
	public Sprite[] tileCSWEN;
	public Sprite[] tileCNWES;
	public Sprite[] tileCWNSE;
	public Sprite[] tileCNWSE;
	public Sprite[] tileCNESW;
	public Sprite[] tileCNSWE;

	//Close by tiles
	//These will determine what sprite to use
	public Tile2D nTile;
	public Tile2D eTile;
	public Tile2D sTile;
	public Tile2D wTile;
	public Tile2D nwTile;
	public Tile2D neTile;
	public Tile2D seTile;
	public Tile2D swTile;

	//Cached components
	public SpriteRenderer cacheRenderer;
	//public BoxCollider2D cacheCollider;
	public int sortingOrder;
	[HideInInspector]	public Collider2D[] tiles;
	[HideInInspector]	public Vector2 savePosition;
	[HideInInspector]	public bool moved;

	//Function to destroy a tile in run time.
	public void DestroyTile() {
		transform.position = new Vector3(9999999.999999f, 9999999.999999f, 0);
		Destroy(gameObject);
		//if (!disableColliderOnAwake) {
			BeautifyCloseByTiles();
			//return;
		//}
		//Debug.LogWarning("Destroying objects with disabled collider has no effect on other tiles");
	}

	public void Awake() {
		CacheComponents();
		//FindTiles();
		savePosition = (Vector2)transform.position;
		//DisableColliderOnAwake();
		if (particles) particles.GetComponent<Renderer>().sortingOrder = cacheRenderer.sortingOrder+1;
	}

	public void CacheComponents() {
		//if (cacheCollider == null) cacheCollider = GetComponent<BoxCollider2D>();
		if (cacheRenderer == null) cacheRenderer = GetComponent<SpriteRenderer>();
	}

	//public void DisableColliderOnAwake() {
	//	if (disableColliderOnAwake) cacheCollider.enabled = false;
	//}

	public void BeautifyCloseByTiles() {
		if (nTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(nTile.gameObject, "TileTool2D: Magic");
#endif
			nTile.FindTiles();
		}

		if (eTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(eTile.gameObject, "TileTool2D: Magic");
#endif
			eTile.FindTiles();
		}

		if (sTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(sTile.gameObject, "TileTool2D: Magic");
#endif
			sTile.FindTiles();
		}

		if (wTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(wTile.gameObject, "TileTool2D: Magic");
#endif
			wTile.FindTiles();
		}

		if (nwTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(nwTile.gameObject, "TileTool2D: Magic");
#endif
			nwTile.FindTiles();
		}

		if (neTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(neTile.gameObject, "TileTool2D: Magic");
#endif
			neTile.FindTiles();
		}

		if (seTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(seTile.gameObject, "TileTool2D: Magic");
#endif
			seTile.FindTiles();
		}

		if (swTile != null) {
#if UNITY_EDITOR
			Undo.RegisterCompleteObjectUndo(swTile.gameObject, "TileTool2D: Magic");
#endif
			swTile.FindTiles();
		}
		//Swap tiles in the nearby tile
		if (nTile != null)	nTile.Beautify();
		if (eTile != null) 	eTile.Beautify();
		if (sTile != null) 	sTile.Beautify();
		if (wTile != null) 	wTile.Beautify();
		if (nwTile != null) nwTile.Beautify();
		if (neTile != null) neTile.Beautify();
		if (seTile != null) seTile.Beautify();
		if (swTile != null) swTile.Beautify();
		//Add attachements to nearby tiles
		if (nTile != null) nTile.AddAttachment();
		if (eTile != null) eTile.AddAttachment();
		if (sTile != null) sTile.AddAttachment();
		if (wTile != null) wTile.AddAttachment();
		if (nwTile != null) nwTile.AddAttachment();
		if (neTile != null) neTile.AddAttachment();
		if (seTile != null) seTile.AddAttachment();
		if (swTile != null) swTile.AddAttachment();
	}

	public void CheckIt() {
		// Check if the position of this tile has been changed
		if (savePosition.x != transform.position.x || savePosition.y != transform.position.y) {		
			if (!moved) {
				BeautifyCloseByTiles();
				moved = true;
				savePosition = (Vector2)transform.position;
			}
				savePosition = (Vector2)transform.position;
				// Find new close by tiles
				FindTiles();
				// Swap sprites based on close by tiles
				Beautify();
				// Find and add attachment
				AddAttachment();
				//Fix new close by tiles if this tile is moved
				BeautifyCloseByTiles();
				moved = false;
		}
	}

	public void FindTiles() {
		nwTile = neTile = seTile = swTile= nTile = eTile = sTile = wTile = null;
	//// Previous version of neighbour tiles used physics check
	//	float overLapSize = 2.0f *tileSize;
	//	tiles = Physics2D.OverlapAreaAll(new Vector2(transform.position.x - overLapSize, transform.position.y - overLapSize), new Vector2(transform.position.x + overLapSize, transform.position.y + overLapSize));
	//	for (int i = 0; i < tiles.Length; i++) {
	//		Debug.Log(tiles[i].name);
	//		Tile2D t = tiles[i].transform.GetComponent<Tile2D>();
	//		if (t != this) FindTile(t);
	//	}
		for (int i = 0; i < transform.parent.childCount; i++) {
			Tile2D t = transform.parent.GetChild(i).GetComponent<Tile2D>();
			if (t != this) FindTile(t);
		}
	}

	public void CheckOverlap() {
		//Only useable in Editor
#if UNITY_EDITOR
		for (int i = 0; i < transform.parent.childCount; i++) {
			Tile2D t = transform.parent.GetChild(i).GetComponent<Tile2D>();
			Transform tt = t.transform;
			if (t != this && gameObject != null && tt.position.x == transform.position.x && tt.position.y == transform.position.y) {
				tt.position = new Vector3(9999999.999999f, 9999999.999999f, 0);
				Undo.DestroyObjectImmediate(t.gameObject);
			}
		}
#endif
	}

	public void FindTile(Tile2D t) {
		Transform tt = t.transform;
		if (tt.parent != transform.parent)
			return;

		// Check if tile is directly above
		if (tt.position.y == Round(transform.position.y + tileSize, tileSize) && tt.position.x == transform.position.x) {

			if (!t.passive) nTile = t;
#if UNITY_EDITOR
			if (nTile != null) EditorUtility.SetDirty(nTile);
#endif
			return;
		}

		// Check if tile is directly below
		if (tt.position.y == Round(transform.position.y - tileSize, tileSize) && tt.position.x == transform.position.x) {
			if (!t.passive) sTile = t;
#if UNITY_EDITOR
			if (sTile != null) EditorUtility.SetDirty(sTile);
#endif
			return;
		}

		// Check if tile is directly to the right
		if (tt.position.x == Round(transform.position.x + tileSize, tileSize) && tt.position.y == transform.position.y) {
			if (!t.passive) eTile = t;
#if UNITY_EDITOR
			if (eTile != null) EditorUtility.SetDirty(eTile);
#endif
			return;
		}

		// Check if tile is directly to the left
		if (tt.position.x == Round(transform.position.x - tileSize, tileSize) && tt.position.y == transform.position.y) {
			if (!t.passive) wTile = t;
#if UNITY_EDITOR
			if (wTile != null) EditorUtility.SetDirty(wTile);
#endif
			return;
		}

		// Check if tile is north west
		if (tt.position.x == Round(transform.position.x - tileSize, tileSize) && tt.position.y == Round(transform.position.y + tileSize, tileSize)) {
			if (!t.passive) nwTile = t;
#if UNITY_EDITOR
			if (nwTile != null) EditorUtility.SetDirty(nwTile);
#endif
			return;
		}

		// Check if tile is north east
		if (tt.position.x == Round(transform.position.x + tileSize, tileSize) && tt.position.y == Round(transform.position.y + tileSize, tileSize)) {
			if (!t.passive) neTile = t;
#if UNITY_EDITOR
			if (neTile != null) EditorUtility.SetDirty(neTile);
#endif
			return;
		}

		// Check if tile is south east
		if (tt.position.x == Round(transform.position.x + tileSize, tileSize) && tt.position.y == Round(transform.position.y - tileSize, tileSize)) {
			if (!t.passive) seTile = t;
#if UNITY_EDITOR
			if (seTile != null) EditorUtility.SetDirty(seTile);
#endif
			return;
		}

		// Check if tile is south west
		if (tt.position.x == Round(transform.position.x - tileSize, tileSize) && tt.position.y == Round(transform.position.y - tileSize, tileSize)) {
			if (!t.passive) swTile = t;
#if UNITY_EDITOR
			if (swTile != null) EditorUtility.SetDirty(swTile);
#endif
			return;
		}

		// Check if tile is overlapping (EDITOR ONLY)
#if UNITY_EDITOR
		if (gameObject != null && tt.position.x == transform.position.x && tt.position.y == transform.position.y) {
			tt.position = new Vector3(9999999.999999f, 9999999.999999f, 0);
			Undo.DestroyObjectImmediate(t.gameObject);
			//DestroyImmediate(t.gameObject);
			return;
		}
#endif
		// Check if tile is overlapping
		if (gameObject != null && tt.position.x == transform.position.x && tt.position.y == transform.position.y) {
			tt.position = new Vector3(9999999.999999f, 9999999.999999f, 0);
			Destroy(t.gameObject);
			return;
		}
	}

	public int FindCloseByHighestSortOrder() {
		int s = 0;
		if (nTile != null) s = nTile.sortingOrder;
		if ((eTile != null) && eTile.sortingOrder > s) s = eTile.sortingOrder;
		if ((sTile != null) && sTile.sortingOrder > s) s = sTile.sortingOrder;
		if ((wTile != null) && wTile.sortingOrder > s) s = wTile.sortingOrder;
		return s + 1;
	}

	public void FixSortingLayer(int layerIndex) {
		CacheComponents();
		cacheRenderer.sortingOrder = layerIndex + Mathf.Clamp(sortingOrder, -1999, 1999);
		if (currentAttachment != null)
			SortAttachment();
#if UNITY_EDITOR
		EditorUtility.SetDirty(cacheRenderer);
#endif
	}

	public int FindCloseByLowestSortOrder() {
		int s = 1000000;
		if (nTile != null) s = nTile.sortingOrder;
		if ((eTile != null) && eTile.sortingOrder < s) s = eTile.sortingOrder;
		if ((sTile != null) && sTile.sortingOrder < s) s = sTile.sortingOrder;
		if ((wTile != null) && wTile.sortingOrder < s) s = wTile.sortingOrder;
		if (s == 1000000) return 0;
		return s - 1;
	}

	public void AddAttachment() {
		GameObject g = FindAttachment();
		if (g == null) {
			if (currentAttachment) {
				cacheRenderer.enabled = true;
				DestroyImmediate(currentAttachment.gameObject);
			}
			return;
		}
		if (currentAttachment) DestroyImmediate(currentAttachment.gameObject);
		GameObject ng = null;

#if UNITY_EDITOR
		ng = (GameObject)PrefabUtility.InstantiatePrefab(g);
#endif
		if (Application.isPlaying)
			ng = (GameObject)Instantiate(g, transform.position, transform.rotation);

		ng.transform.parent = transform;
		ng.transform.localPosition = Vector3.zero;
		currentAttachment = ng.GetComponent<Tile2DAttachment>();
		if (currentAttachment.replaceSprite) cacheRenderer.enabled = false;
		SortAttachment();
	}

	public void SortAttachment() {
		SpriteRenderer[] s = (SpriteRenderer[])currentAttachment.transform.GetComponents<SpriteRenderer>();
		for (int i = 0; i < s.Length; i++) {
			s[i].sortingOrder = cacheRenderer.sortingOrder;
			s[i].sortingLayerName = cacheRenderer.sortingLayerName;
			s[i].sortingLayerID = cacheRenderer.sortingLayerID;
		}
	}

	public GameObject FindAttachment() {
		GameObject g = null;
		for(int i = 0; i < attachments.Length; i++) {
			if(attachments[i].replaceTile == tileSprite) {
				g = attachments[i].gameObject;
				break;
			}
		}
		return g;
	}

	public float Round(float input, float size) {
		float snappedValue = 0.0f;
		snappedValue = size * Mathf.Round((input / size));
		return (snappedValue);
	}

	public void Beautify() {
		Tile2D nTileX = null;
		Tile2D eTileX = null;
		Tile2D sTileX = null;
		Tile2D wTileX = null;
		Tile2D neTileX = null;
		Tile2D nwTileX = null;
		Tile2D seTileX = null;
		Tile2D swTileX = null;
		if ((nTile != null) && nTile.tileType == tileType) nTileX = nTile;
		if ((eTile != null) && eTile.tileType == tileType) eTileX = eTile;
		if ((sTile != null) && sTile.tileType == tileType) sTileX = sTile;
		if ((wTile != null) && wTile.tileType == tileType) wTileX = wTile;
		if ((nwTile != null) && nwTile.tileType == tileType) nwTileX = nwTile;
		if ((neTile != null) && neTile.tileType == tileType) neTileX = neTile;
		if ((seTile != null) && seTile.tileType == tileType) seTileX = seTile;
		if ((swTile != null) && swTile.tileType == tileType) swTileX = swTile;

		if (passive || (nTileX == null) && (eTileX == null) && (sTileX == null) && (wTileX == null) && tileA.Length > 0) cacheRenderer.sprite = tileA[Random.Range(0, tileA.Length)];

		//Has a tile at all NESW sides
		if ((nTileX != null) && (eTileX != null) && (sTileX != null) && (wTileX != null)) {

			if (swTileX == null && seTileX == null && nwTileX == null && neTileX == null && tileCNSWE.Length > 0) {
				cacheRenderer.sprite = tileCNSWE[Random.Range(0, tileCNSWE.Length)];
				tileSprite = "CNSWE";
				return;
			}

			if (nwTileX == null && neTileX == null && seTileX == null && tileCENSW.Length > 0) {
				cacheRenderer.sprite = tileCENSW[Random.Range(0, tileCENSW.Length)];
				tileSprite = "CENSW";
				return;
			}

			if (nwTileX == null && neTileX == null && swTileX == null && tileCNWES.Length > 0) {
				cacheRenderer.sprite = tileCNWES[Random.Range(0, tileCNWES.Length)];
				tileSprite = "CNWES";
				return;
			}

			if(neTileX == null && seTileX == null && swTileX == null && tileCSWEN.Length > 0) {
				cacheRenderer.sprite = tileCSWEN[Random.Range(0, tileCSWEN.Length)];
				tileSprite = "CSWEN";
				return;
			}

			if (nwTileX == null && seTileX == null && swTileX == null && tileCWNSE.Length > 0) {
				cacheRenderer.sprite = tileCWNSE[Random.Range(0, tileCWNSE.Length)];
				tileSprite = "CWNSE";
				return;
			}

			if (nwTileX == null && seTileX == null && tileCNWSE.Length > 0) {
				cacheRenderer.sprite = tileCNWSE[Random.Range(0, tileCNWSE.Length)];
				tileSprite = "CNWSE";
				return;
			}

			if (neTileX == null && swTileX == null && tileCNESW.Length > 0) {
				cacheRenderer.sprite = tileCNESW[Random.Range(0, tileCNESW.Length)];
				tileSprite = "CNESW";
				return;
			}

			if (nwTileX == null && neTileX == null && tileCNWE.Length > 0) {
				cacheRenderer.sprite = tileCNWE[Random.Range(0, tileCNWE.Length)];
				tileSprite = "CNWE";
				return;
			}

			if (seTileX == null && neTileX == null && tileCENS.Length > 0) {
				cacheRenderer.sprite = tileCENS[Random.Range(0, tileCENS.Length)];
				tileSprite = "CENS";
				return;
			}

			if (seTileX == null && swTileX == null && tileCSWE.Length > 0) {
				cacheRenderer.sprite = tileCSWE[Random.Range(0, tileCSWE.Length)];
				tileSprite = "CSWE";
				return;
			}

			if (nwTileX == null && swTileX == null && tileCWNS.Length > 0) {
				cacheRenderer.sprite = tileCWNS[Random.Range(0, tileCWNS.Length)];
				tileSprite = "CWNS";
				return;
			}

			if (nwTileX == null && tileCNW.Length > 0) {
				cacheRenderer.sprite = tileCNW[Random.Range(0, tileCNW.Length)];
				tileSprite = "CNW";
				return;
			}

			if (neTileX == null && tileCNE.Length > 0) {
				cacheRenderer.sprite = tileCNE[Random.Range(0, tileCNE.Length)];
				tileSprite = "CNE";
				return;
			}

			if (seTileX == null && tileCSE.Length > 0) {
				cacheRenderer.sprite = tileCSE[Random.Range(0, tileCSE.Length)];
				tileSprite = "CSE";
				return;
			}

			if (swTileX == null && tileCSW.Length > 0) {
				cacheRenderer.sprite = tileCSW[Random.Range(0, tileCSW.Length)];
				tileSprite = "CSW";
				return;
			}

			if (tileC.Length > 0) {
				cacheRenderer.sprite = tileC[Random.Range(0, tileC.Length)];
				tileSprite = "C";
				return;
			}
		}

		if ((nTileX == null) && (eTileX != null) && (sTileX != null) && (wTileX != null)) {

			if (seTileX == null && swTileX == null && tileCNS.Length > 0) {
				cacheRenderer.sprite = tileCNS[Random.Range(0, tileCNS.Length)];
				tileSprite = "CNS";
				return;
			}

			if (seTileX == null && tileCNEE.Length > 0) {
				cacheRenderer.sprite = tileCNEE[Random.Range(0, tileCNEE.Length)];
				tileSprite = "CNEE";
				return;
			}

			if (swTileX == null && tileCNSS.Length > 0) {
				cacheRenderer.sprite = tileCNSS[Random.Range(0, tileCNSS.Length)];
				tileSprite = "CNSS";
				return;
			}

			if (tileCN.Length > 0) {
				cacheRenderer.sprite = tileCN[Random.Range(0, tileCN.Length)];
				tileSprite = "CN";
				return;
			}
		}

		if ((nTileX != null) && (eTileX != null) && (sTileX == null) && (wTileX != null)) {

			if (neTileX == null && nwTileX == null && tileCSN.Length > 0) {
				cacheRenderer.sprite = tileCSN[Random.Range(0, tileCSN.Length)];
				tileSprite = "CSN";
				return;
			}

			if (nwTileX == null && tileCSWW.Length > 0) {
				cacheRenderer.sprite = tileCSWW[Random.Range(0, tileCSWW.Length)];
				tileSprite = "CSWW";
				return;
			}

			if (neTileX == null && tileCSEE.Length > 0) {
				cacheRenderer.sprite = tileCSEE[Random.Range(0, tileCSEE.Length)];
				tileSprite = "CSEE";
				return;
			}

			if (tileCS.Length > 0) {
				cacheRenderer.sprite = tileCS[Random.Range(0, tileCS.Length)];
				tileSprite = "CS";
				return;
			}
		}

		if ((nTileX != null) && (eTileX != null) && (sTileX != null) && (wTileX == null)) {

			if (neTileX == null && seTileX == null && tileCWE.Length > 0) {
				cacheRenderer.sprite = tileCWE[Random.Range(0, tileCWE.Length)];
				tileSprite = "CWE";
				return;
			}

			if (neTileX == null && tileCWNN.Length > 0) {
				cacheRenderer.sprite = tileCWNN[Random.Range(0, tileCWNN.Length)];
				tileSprite = "CWNN";
				return;
			}

			if (seTileX == null && tileCWSS.Length > 0) {
				cacheRenderer.sprite = tileCWSS[Random.Range(0, tileCWSS.Length)];
				tileSprite = "CWSS";
				return;
			}

			if (tileCW.Length > 0) {
				cacheRenderer.sprite = tileCW[Random.Range(0, tileCW.Length)];
				tileSprite = "CW";
				return;
			}
		}

		if ((nTileX != null) && (eTileX == null) && (sTileX != null) && (wTileX != null)) {

			if (nwTileX == null && swTileX == null && tileCEW.Length > 0) {
				cacheRenderer.sprite = tileCEW[Random.Range(0, tileCEW.Length)];
				tileSprite = "CEW";
				return;
			}

			if (swTileX == null && tileCESS.Length > 0) {
				cacheRenderer.sprite = tileCESS[Random.Range(0, tileCESS.Length)];
				tileSprite = "CESS";
				return;
			}

			if (nwTileX == null && tileCENN.Length > 0) {
				cacheRenderer.sprite = tileCENN[Random.Range(0, tileCENN.Length)];
				tileSprite = "CENN";
				return;
			}

			if (tileCE.Length > 0) {
				cacheRenderer.sprite = tileCE[Random.Range(0, tileCE.Length)];
				tileSprite = "CE";
				return;
			}
		}

		// Check if this is NW tile
		if ((nTileX == null) && (eTileX != null) && (sTileX != null) && (wTileX == null)) {

			if (seTileX == null && tileNWSE.Length > 0) {
				cacheRenderer.sprite = tileNWSE[Random.Range(0, tileNWSE.Length)];
				tileSprite = "NWSE";
				return;
			}

			if (tileNW.Length > 0) {
				cacheRenderer.sprite = tileNW[Random.Range(0, tileNW.Length)];
				tileSprite = "NW";
				return;
			}
		}

		// Check if this is NE tile
		if ((nTileX == null) && (eTileX == null) && (sTileX != null) && (wTileX != null)) {

			if (swTileX == null && tileNESW.Length > 0) {
				cacheRenderer.sprite = tileNESW[Random.Range(0, tileNESW.Length)];
				tileSprite = "NESW";
				return;
			}

			if (tileNE.Length > 0) {
				cacheRenderer.sprite = tileNE[Random.Range(0, tileNE.Length)];
				tileSprite = "NE";
				return;
			}
		}

		// Check if this is SW tile
		if ((nTileX != null) && (eTileX != null) && (sTileX == null) && (wTileX == null)) {

			if (neTileX == null && tileSWNE.Length > 0) {
				cacheRenderer.sprite = tileSWNE[Random.Range(0, tileSWNE.Length)];
				tileSprite = "SWNE";
				return;
			}

			if (tileSW.Length > 0) {
				cacheRenderer.sprite = tileSW[Random.Range(0, tileSW.Length)];
				tileSprite = "SW";
				return;
			}
		}

		// Check if this is SE tile
		if ((nTileX != null) && (eTileX == null) && (sTileX == null) && (wTileX != null)) {

			if (nwTileX == null && tileSENW.Length > 0) {
				cacheRenderer.sprite = tileSENW[Random.Range(0, tileSENW.Length)];
				tileSprite = "SENW";
				return;
			}

			if (tileSE.Length > 0) {
				cacheRenderer.sprite = tileSE[Random.Range(0, tileSE.Length)];
				tileSprite = "SE";
				return;
			}
		}

		// Check if this is N tile
		if ((nTileX == null) && (eTileX == null) && (sTileX != null) && (wTileX == null)) {
			if (tileN.Length > 0) cacheRenderer.sprite = tileN[Random.Range(0, tileN.Length)];
			tileSprite = "N";
			return;
		}
		// Check if this is E tile
		if ((nTileX == null) && (eTileX == null) && (sTileX == null) && (wTileX != null)) {
			if (tileE.Length > 0) cacheRenderer.sprite = tileE[Random.Range(0, tileE.Length)];
			tileSprite = "E";
			return;
		}
		// Check if this is S tile
		if ((nTileX != null) && (eTileX == null) && (sTileX == null) && (wTileX == null)) {
			if (tileS.Length > 0) cacheRenderer.sprite = tileS[Random.Range(0, tileS.Length)];
			tileSprite = "S";
			return;
		}
		// Check if this is W tile
		if ((nTileX == null) && (eTileX != null) && (sTileX == null) && (wTileX == null)) {
			if (tileW.Length > 0) cacheRenderer.sprite = tileW[Random.Range(0, tileW.Length)];
			tileSprite = "W";
			return;
		}

		// Check if this is NS tile
		if ((nTileX == null) && (eTileX != null) && (sTileX == null) && (wTileX != null)) {
			if (tileNS.Length > 0) cacheRenderer.sprite = tileNS[Random.Range(0, tileNS.Length)];
			tileSprite = "NS";
			return;
		}
		// Check if this is WE tile
		if ((nTileX != null) && (eTileX == null) && (sTileX != null) && (wTileX == null)) {
			if (tileWE.Length > 0) cacheRenderer.sprite = tileWE[Random.Range(0, tileWE.Length)];
			tileSprite = "WE";
			return;
		}
		// Check if this is A tile
		if (tileA.Length > 0) {
			cacheRenderer.sprite = tileA[Random.Range(0, tileA.Length)];
			tileSprite = "A";
			return;
		}
	}
}