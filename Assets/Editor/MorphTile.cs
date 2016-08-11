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
    public abstract class MorphTile : Tile
    {
        public const int NONE = 0;
        public const int SOUTH = 1;
        public const int SOUTHEAST = 2;
        public const int EAST = 4;
        public const int NORTHEAST = 8;
        public const int NORTH = 16;
        public const int NORTHWEST = 32;
        public const int WEST = 64;
        public const int SOUTHWEST = 128;
        public const int ALL = 255;

        [SerializeField]
        public Sprite[] sprites;

        [SerializeField]
        public GameObject[] gameobjects;

        [SerializeField]
        public MorphTile[] mergers;

        /**
         *
         */
        public override void RefreshTile(Vector3Int location, ITileMap tileMap)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    Vector3Int position = new Vector3Int(location.x + x, location.y + y, location.z);

                    if (TileValue(tileMap, position))
                    {
                        tileMap.RefreshTile(position);
                    }
                }
            }
        }

        /**
         *
         */
        public override bool GetTileData(Vector3Int location, ITileMap tileMap, ref TileData tileData)
        {
            base.GetTileData(location, tileMap, ref tileData);

            int mask = GetMask(location, tileMap, ref tileData);
            int index = GetIndex(mask);

            tileData.sprite = GetTileSprite(index);
            tileData.gameobject = GetTileGameObject(index);

            return true;
        }

        /**
         *
         */
        private Sprite GetTileSprite(int index)
        {
            if (sprite != null)
            {
                return sprite;
            }

            if (index >= 0 && index < sprites.Length)
            {
                return sprites[index];
            }

            return null;
        }

        /**
         *
         */
        private GameObject GetTileGameObject(int index)
        {
            if (gameobject != null)
            {
                return gameobject;
            }

            if (index >= 0 && index < gameobjects.Length)
            {
                return gameobjects[index];
            }

            return null;
        }

        /**
         *
         */
        private int GetMask(Vector3Int location, ITileMap tileMap, ref TileData tileData)
        {
            int mask = TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? SOUTH : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? SOUTHEAST : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? EAST : 0;
            mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? NORTHEAST : 0;
            mask += TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? NORTH : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? NORTHWEST : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? WEST : 0;
            mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? SOUTHWEST : 0;

            byte original = (byte)mask;

            if ((original | 254) < ALL) { mask = mask & 125; }
            if ((original | 251) < ALL) { mask = mask & 245; }
            if ((original | 239) < ALL) { mask = mask & 215; }
            if ((original | 191) < ALL) { mask = mask & 95; }

            return mask;
        }

        /**
         *
         */
        private bool TileValue(ITileMap tileMap, Vector3Int position)
        {
            BaseTile tile = tileMap.GetTile(position);

            return (tile != null && ShouldMerge(tile));
        }

        /**
         *
         */
        private bool ShouldMerge(BaseTile tile)
        {
            if (tile == this)
            {
                return true;
            }
            else if (tile is MorphTile)
            {
                return this.mergers.Contains(tile);
            }
            else
            {
                return false;
            }
        }

        /**
         *
         */
        abstract public int GetIndex(int mask);
    }
}
