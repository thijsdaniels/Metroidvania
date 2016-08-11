using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine
{
    [Serializable]
    public class TerrainTile : MorphTile
    {
        override public int GetIndex(int mask)
        {
            switch (mask)
            {
                case NONE:
                case SOUTHEAST:
                case NORTHEAST:
                case NORTHWEST:
                case SOUTHWEST:
                case SOUTHEAST + NORTHEAST:
                case SOUTHEAST + NORTHWEST:
                case SOUTHEAST + SOUTHWEST:
                case NORTHEAST + NORTHWEST:
                case NORTHEAST + SOUTHWEST:
                case NORTHWEST + SOUTHWEST:
                case SOUTHEAST + NORTHEAST + NORTHWEST:
                case SOUTHEAST + NORTHEAST + SOUTHWEST:
                case SOUTHEAST + NORTHEAST + NORTHWEST + SOUTHWEST:
                    return 0;

                case EAST:
                case EAST + SOUTHEAST:
                case EAST + NORTHEAST:
                case EAST + NORTHWEST:
                case EAST + SOUTHWEST:
                case EAST + SOUTHEAST + NORTHEAST:
                case EAST + SOUTHEAST + NORTHWEST:
                case EAST + SOUTHEAST + SOUTHWEST:
                case EAST + NORTHEAST + NORTHWEST:
                case EAST + NORTHEAST + SOUTHWEST:
                case EAST + NORTHWEST + SOUTHWEST:
                case EAST + SOUTHEAST + NORTHEAST + NORTHWEST:
                case EAST + SOUTHEAST + NORTHEAST + SOUTHWEST:
                case EAST + SOUTHEAST + NORTHEAST + NORTHWEST + SOUTHWEST:
                    return 1;

                case WEST + EAST:
                case WEST + EAST + SOUTHEAST:
                case WEST + EAST + NORTHEAST:
                case WEST + EAST + NORTHWEST:
                case WEST + EAST + SOUTHWEST:
                case WEST + EAST + SOUTHEAST + NORTHEAST:
                case WEST + EAST + SOUTHEAST + NORTHWEST:
                case WEST + EAST + SOUTHEAST + SOUTHWEST:
                case WEST + EAST + NORTHEAST + NORTHWEST:
                case WEST + EAST + NORTHEAST + SOUTHWEST:
                case WEST + EAST + NORTHWEST + SOUTHWEST:
                case WEST + EAST + SOUTHEAST + NORTHEAST + NORTHWEST + SOUTHWEST:
                    return 2;

                case 64:
                case WEST + SOUTHEAST:
                case WEST + NORTHEAST:
                case WEST + NORTHWEST:
                case WEST + SOUTHWEST:
                case WEST + SOUTHEAST + NORTHEAST:
                case WEST + SOUTHEAST + NORTHWEST:
                case WEST + SOUTHEAST + SOUTHWEST:
                case WEST + NORTHEAST + NORTHWEST:
                case WEST + NORTHEAST + SOUTHWEST:
                case WEST + NORTHWEST + SOUTHWEST:
                case WEST + SOUTHEAST + NORTHEAST + NORTHWEST:
                case WEST + SOUTHEAST + NORTHEAST + SOUTHWEST:
                case WEST + SOUTHEAST + NORTHWEST + SOUTHWEST:
                case WEST + NORTHEAST + NORTHWEST + SOUTHWEST:
                case WEST + SOUTHEAST + NORTHEAST + NORTHWEST + SOUTHWEST:
                    return 3;

                case ALL:
                    return 4;

                case SOUTHWEST + SOUTH + SOUTHEAST + EAST + WEST:
                case SOUTHWEST + SOUTH + SOUTHEAST + EAST + WEST + NORTHEAST:
                case SOUTHWEST + SOUTH + SOUTHEAST + EAST + WEST + NORTHWEST:
                case SOUTHWEST + SOUTH + SOUTHEAST + EAST + WEST + NORTHEAST + NORTHWEST:
                    return 5;

                case NORTHWEST + NORTH + NORTHEAST + WEST + EAST:
                case NORTHWEST + NORTH + NORTHEAST + WEST + EAST + SOUTHEAST:
                case NORTHWEST + NORTH + NORTHEAST + WEST + EAST + SOUTHWEST:
                case NORTHWEST + NORTH + NORTHEAST + WEST + EAST + SOUTHEAST + SOUTHWEST:
                    return 6;

                case EAST + SOUTHEAST + SOUTH:
                case EAST + SOUTHEAST + SOUTH + NORTHEAST:
                case EAST + SOUTHEAST + SOUTH + SOUTHWEST:
                case EAST + SOUTHEAST + SOUTH + NORTHEAST + SOUTHWEST:
                case EAST + SOUTHEAST + SOUTH + NORTHWEST:
                case EAST + SOUTHEAST + SOUTH + NORTHEAST + NORTHWEST:
                case EAST + SOUTHEAST + SOUTH + SOUTHWEST + NORTHWEST:
                case EAST + SOUTHEAST + SOUTH + NORTHEAST + SOUTHWEST + NORTHWEST:
                    return 7;

                case SOUTHWEST + WEST + SOUTH:
                case SOUTHWEST + WEST + SOUTH + SOUTHEAST:
                case SOUTHWEST + WEST + SOUTH + NORTHWEST:
                case SOUTHWEST + WEST + SOUTH + SOUTHEAST + NORTHWEST:
                case SOUTHWEST + WEST + SOUTH + NORTHEAST:
                case SOUTHWEST + WEST + SOUTH + SOUTHEAST + NORTHEAST:
                case SOUTHWEST + WEST + SOUTH + NORTHWEST + NORTHEAST:
                case SOUTHWEST + WEST + SOUTH + SOUTHEAST + NORTHWEST + NORTHEAST:
                    return 8;

                case SOUTH:
                case SOUTH + SOUTHEAST:
                case SOUTH + NORTHEAST:
                case SOUTH + NORTHWEST:
                case SOUTH + SOUTHWEST:
                case SOUTH + SOUTHEAST + NORTHEAST:
                case SOUTH + SOUTHEAST + NORTHWEST:
                case SOUTH + SOUTHEAST + SOUTHWEST:
                case SOUTH + NORTHEAST + NORTHWEST:
                case SOUTH + NORTHEAST + SOUTHWEST:
                case SOUTH + NORTHWEST + SOUTHWEST:
                case SOUTH + SOUTHEAST + NORTHEAST + NORTHWEST:
                case SOUTH + SOUTHEAST + NORTHEAST + SOUTHWEST:
                case SOUTH + NORTHEAST + NORTHWEST + SOUTHWEST:
                case SOUTH + SOUTHEAST + NORTHEAST + NORTHWEST + SOUTHWEST:
                    return 9;

                case ALL - SOUTHEAST:
                    return 10;

                case ALL - SOUTHEAST - SOUTHWEST:
                    return 11;

                case ALL - SOUTHWEST:
                    return 12;

                case ALL - WEST - NORTHEAST - SOUTHEAST:
                case ALL - WEST - NORTHEAST - SOUTHEAST - NORTHWEST:
                case ALL - WEST - NORTHEAST - SOUTHEAST - SOUTHWEST:
                case ALL - WEST - NORTHEAST - SOUTHEAST - SOUTHWEST - NORTHWEST:
                    return 13;

                case NORTH + NORTHEAST + EAST + SOUTHEAST + SOUTH:
                case NORTH + NORTHEAST + EAST + SOUTHEAST + SOUTH + NORTHWEST:
                case NORTH + NORTHEAST + EAST + SOUTHEAST + SOUTH + SOUTHWEST:
                case NORTH + NORTHEAST + EAST + SOUTHEAST + SOUTH + NORTHWEST + SOUTHWEST:
                    return 14;

                case NORTHWEST + NORTH + WEST + SOUTHWEST + SOUTH:
                case NORTHWEST + NORTH + WEST + SOUTHWEST + SOUTH + NORTHEAST:
                case NORTHWEST + NORTH + WEST + SOUTHWEST + SOUTH + SOUTHEAST:
                case NORTHWEST + NORTH + WEST + SOUTHWEST + SOUTH + SOUTHEAST + NORTHEAST:
                    return 15;

                case SOUTH + NORTH:
                case SOUTH + NORTH + SOUTHEAST:
                case SOUTH + NORTH + NORTHEAST:
                case SOUTH + NORTH + NORTHWEST:
                case SOUTH + NORTH + SOUTHWEST:
                case SOUTH + NORTH + SOUTHEAST + NORTHEAST:
                case SOUTH + NORTH + SOUTHEAST + NORTHWEST:
                case SOUTH + NORTH + SOUTHEAST + SOUTHWEST:
                case SOUTH + NORTH + NORTHEAST + NORTHWEST:
                case SOUTH + NORTH + NORTHEAST + SOUTHWEST:
                case SOUTH + NORTH + NORTHWEST + SOUTHWEST:
                case SOUTH + NORTH + SOUTHEAST + NORTHEAST + NORTHWEST + SOUTHWEST:
                    return 16;

                case ALL - NORTHEAST - SOUTHEAST:
                    return 17;

                case ALL - NORTHWEST - NORTHEAST - SOUTHEAST - SOUTHWEST:
                    return 18;

                case ALL - NORTHWEST - SOUTHWEST:
                    return 19;

                case ALL - NORTHWEST - SOUTHWEST - EAST:
                case ALL - NORTHWEST - SOUTHWEST - EAST - SOUTHEAST:
                case ALL - NORTHWEST - SOUTHWEST - EAST - NORTHEAST:
                case ALL - NORTHWEST - SOUTHWEST - EAST - SOUTHEAST - NORTHEAST:
                    return 20;

                case NORTH + NORTHEAST + EAST:
                case NORTH + NORTHEAST + EAST + SOUTHEAST:
                case NORTH + NORTHEAST + EAST + NORTHWEST:
                case NORTH + NORTHEAST + EAST + SOUTHEAST + NORTHWEST:
                case NORTH + NORTHEAST + EAST + SOUTHWEST:
                case NORTH + NORTHEAST + EAST + SOUTHEAST + SOUTHWEST:
                case NORTH + NORTHEAST + EAST + NORTHWEST + SOUTHWEST:
                case NORTH + NORTHEAST + EAST + SOUTHEAST + NORTHWEST + SOUTHWEST:
                    return 21;

                case WEST + NORTHWEST + NORTH:
                case WEST + NORTHWEST + NORTH + NORTHEAST:
                case WEST + NORTHWEST + NORTH + SOUTHWEST:
                case WEST + NORTHWEST + NORTH + NORTHEAST + SOUTHWEST:
                case WEST + NORTHWEST + NORTH + SOUTHEAST:
                case WEST + NORTHWEST + NORTH + NORTHEAST + SOUTHEAST:
                case WEST + NORTHWEST + NORTH + SOUTHWEST + SOUTHEAST:
                case WEST + NORTHWEST + NORTH + NORTHEAST + SOUTHWEST + SOUTHEAST:
                    return 22;

                case 16:
                case NORTH + NORTHEAST:
                case NORTH + NORTHWEST:
                case NORTH + NORTHEAST + NORTHWEST:
                case NORTH + SOUTHWEST:
                case NORTH + NORTHEAST + SOUTHWEST:
                case NORTH + NORTHWEST + SOUTHWEST:
                case NORTH + NORTHEAST + NORTHWEST + SOUTHWEST:
                case NORTH + SOUTHEAST:
                case NORTH + NORTHEAST + SOUTHEAST:
                case NORTH + NORTHWEST + SOUTHEAST:
                case NORTH + NORTHEAST + NORTHWEST + SOUTHEAST:
                case NORTH + SOUTHEAST + SOUTHWEST:
                case NORTH + NORTHEAST + SOUTHEAST + SOUTHWEST:
                case NORTH + NORTHWEST + SOUTHEAST + SOUTHWEST:
                case NORTH + NORTHEAST + NORTHWEST + SOUTHEAST + SOUTHWEST:
                    return 23;

                case ALL - NORTHEAST:
                    return 24;

                case ALL - NORTHEAST - NORTHWEST:
                    return 25;

                case ALL - NORTHWEST:
                    return 26;

                case ALL - SOUTHWEST - SOUTHEAST - NORTH:
                case ALL - SOUTHWEST - SOUTHEAST - NORTH - NORTHEAST:
                case ALL - SOUTHWEST - SOUTHEAST - NORTH - NORTHWEST:
                case ALL - SOUTHWEST - SOUTHEAST - NORTH - NORTHEAST - NORTHWEST:
                    return 27;

                case SOUTH + EAST:
                case SOUTH + EAST + NORTHEAST:
                case SOUTH + EAST + NORTHWEST:
                case SOUTH + EAST + SOUTHWEST:
                case SOUTH + EAST + NORTHEAST + NORTHWEST:
                case SOUTH + EAST + NORTHEAST + SOUTHWEST:
                case SOUTH + EAST + NORTHWEST + SOUTHWEST:
                case SOUTH + EAST + NORTHEAST + NORTHWEST + SOUTHWEST:
                    return 28;

                case WEST + SOUTH:
                case WEST + SOUTH + SOUTHEAST:
                case WEST + SOUTH + NORTHEAST:
                case WEST + SOUTH + NORTHWEST:
                case WEST + SOUTH + SOUTHEAST + NORTHEAST:
                case WEST + SOUTH + SOUTHEAST + NORTHWEST:
                case WEST + SOUTH + NORTHEAST + NORTHWEST:
                case WEST + SOUTH + SOUTHEAST + NORTHEAST + NORTHWEST:
                    return 29;

                case ALL - SOUTHEAST - WEST:
                case ALL - SOUTHEAST - WEST - NORTHWEST:
                case ALL - SOUTHEAST - WEST - SOUTHWEST:
                case ALL - SOUTHEAST - WEST - NORTHWEST - SOUTHWEST:
                    return 30;

                case ALL - SOUTHWEST - EAST:
                case ALL - SOUTHWEST - EAST - SOUTHEAST:
                case ALL - SOUTHWEST - EAST - NORTHEAST:
                case ALL - SOUTHWEST - EAST - SOUTHEAST - NORTHEAST:
                    return 31;

                case ALL - SOUTHEAST - NORTH:
                case ALL - SOUTHEAST - NORTH - NORTHEAST:
                case ALL - SOUTHEAST - NORTH - NORTHWEST:
                case ALL - SOUTHEAST - NORTH - NORTHWEST - NORTHEAST:
                    return 32;

                case ALL - SOUTHWEST - NORTH:
                case ALL - SOUTHWEST - NORTH - NORTHEAST:
                case ALL - SOUTHWEST - NORTH - NORTHWEST:
                case ALL - SOUTHWEST - NORTH - NORTHWEST - NORTHEAST:
                    return 33;

                case ALL - NORTHWEST - NORTHEAST - SOUTH:
                case ALL - NORTHWEST - NORTHEAST - SOUTH - SOUTHEAST:
                case ALL - NORTHWEST - NORTHEAST - SOUTH - SOUTHWEST:
                case ALL - NORTHWEST - NORTHEAST - SOUTH - SOUTHEAST - SOUTHWEST:
                    return 34;

                case NORTH + EAST:
                case NORTH + EAST + SOUTHEAST:
                case NORTH + EAST + NORTHWEST:
                case NORTH + EAST + SOUTHWEST:
                case NORTH + EAST + SOUTHEAST + NORTHWEST:
                case NORTH + EAST + SOUTHEAST + SOUTHWEST:
                case NORTH + EAST + NORTHWEST + SOUTHWEST:
                case NORTH + EAST + SOUTHEAST + NORTHWEST + SOUTHWEST:
                    return 35;

                case NORTH + WEST:
                case NORTH + WEST + SOUTHEAST:
                case NORTH + WEST + NORTHEAST:
                case NORTH + WEST + SOUTHWEST:
                case NORTH + WEST + SOUTHEAST + NORTHEAST:
                case NORTH + WEST + SOUTHEAST + SOUTHWEST:
                case NORTH + WEST + NORTHEAST + SOUTHWEST:
                case NORTH + WEST + SOUTHEAST + NORTHEAST + SOUTHWEST:
                    return 36;

                case ALL - NORTHEAST - WEST:
                case ALL - NORTHEAST - WEST - NORTHWEST:
                case ALL - NORTHEAST - WEST - SOUTHWEST:
                case ALL - NORTHEAST - WEST - NORTHWEST - SOUTHWEST:
                    return 37;

                case ALL - NORTHWEST - EAST:
                case ALL - NORTHWEST - EAST - SOUTHEAST:
                case ALL - NORTHWEST - EAST - NORTHEAST:
                case ALL - NORTHWEST - EAST - SOUTHEAST - NORTHEAST:
                    return 38;

                case ALL - NORTHEAST - SOUTH:
                case ALL - NORTHEAST - SOUTH - SOUTHEAST:
                case ALL - NORTHEAST - SOUTH - SOUTHWEST:
                case ALL - NORTHEAST - SOUTH - SOUTHEAST - SOUTHWEST:
                    return 39;

                case ALL - NORTHWEST - SOUTH:
                case ALL - NORTHWEST - SOUTH - SOUTHEAST:
                case ALL - NORTHWEST - SOUTH - SOUTHWEST:
                case ALL - NORTHWEST - SOUTH - SOUTHEAST - SOUTHWEST:
                    return 40;

                case ALL - NORTHWEST - SOUTHEAST:
                    return 41;

                case ALL - NORTHEAST - SOUTHWEST:
                    return 42;

                case ALL - NORTHWEST - NORTHEAST - SOUTHEAST:
                    return 43;

                case ALL - SOUTHWEST - NORTHWEST - NORTHEAST:
                    return 44;

                case ALL - SOUTHWEST - NORTHEAST - SOUTHEAST:
                    return 45;

                case ALL - SOUTHWEST - NORTHWEST - SOUTHEAST:
                    return 46;
            }

            return -1;
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/Scripted Tile/Terrain Tile")]
        public static void CreateTerrainTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Terrain Tile", "New Terrain Tile", "asset", "Save Terrain Tile", "Assets");

            if (path == "")
            {
                return;
            }

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TerrainTile>(), path);
        }

        public override Sprite GetPreview()
        {
            if (sprites != null && sprites.Length > 0)
            {
                return sprites[0];
            }

            return null;
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TerrainTile))]
    public class TerrainTileEditor : Editor
    {
        private TerrainTile tile { get { return (target as TerrainTile); } }
        private int length = 47;

        public void OnEnable()
        {
            if (tile.sprites == null || tile.sprites.Length != length)
            {
                tile.sprites = new Sprite[length];
                EditorUtility.SetDirty(tile);
            }

            if (tile.gameobjects == null || tile.gameobjects.Length != length)
            {
                tile.gameobjects = new GameObject[length];
                EditorUtility.SetDirty(tile);
            }
        }

        private List<string> TileLabels = new List<string>
        {
            "Isolated",
            "West",
            "North South",
            "East",
            "Center",
            "Center North",
            "Center South",
            "North West",
            "North East",
            "North",
            "Center SE",
            "Center SW + SE",
            "Center SW",
            "Center West + Corners",
            "Center West",
            "Center East",
            "West East",
            "Center NE + SE",
            "Center All Corners",
            "Center NW + SW",
            "Center East + Corners",
            "South West",
            "South East",
            "South",
            "Center NE",
            "Center NW + NE",
            "Center NW",
            "Center North + Corners",
            "Northwest + SE",
            "Northeast + SW",
            "Center West + SE",
            "Center East + SW",
            "Center North + SE",
            "Center North + SW",
            "Center South + Corners",
            "South West + NE",
            "South East + NW",
            "Center West + NE",
            "Center East + NW",
            "Center South + NE",
            "Center South + NW",
            "Center NW + SE",
            "Center SW + NE",
            "Center NW + NE + SW",
            "Center NW + NE + SE",
            "Center NE + SW + SE",
            "Center NW + SW + SE",
        };

        /**
        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Place sprites shown based on the contents of the sprite.");
            EditorGUILayout.Space();

            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 210;


            // TODO: Support drag and drop of sprites
            // TODO: Filter an assign sprites by index
            var s = (Sprite)EditorGUILayout.ObjectField("Mass Replace All", tile.sprites[0], typeof(Sprite), false, null);

            EditorGUI.BeginChangeCheck();
            if (s != tile.sprites[0])
            {
                string spriteSheet = AssetDatabase.GetAssetPath(s);
                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();

                for (int i = 0; i < sprites.Length; i++)
                {
                    tile.sprites[i] = sprites[i];
                }
            }

            for (int i = 0; i < length; i++)
            {
                string label = "Unlabelled";
                if (TileLabels.Count > i)
                    label = TileLabels[i];

                tile.sprites[i] = (Sprite)EditorGUILayout.ObjectField(label, tile.sprites[i], typeof(Sprite), false, null);
            }
            if (EditorGUI.EndChangeCheck())
                EditorUtility.SetDirty(tile);

            EditorGUIUtility.labelWidth = oldLabelWidth;
        }
        **/
    }
#endif
}
