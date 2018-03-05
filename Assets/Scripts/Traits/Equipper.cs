using System;
using Character;
using Objects.Collectables;
using UnityEngine;

namespace Traits
{
    /// <summary>
    /// 
    /// </summary>
    public class Equipper : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        public enum ItemSlots
        {
            Primary,
            Secondary,
            Tertiary,
            Quaternary
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Item PrimaryItem;
        
        /// <summary>
        /// 
        /// </summary>
        public Item SecondaryItem;
        
        /// <summary>
        /// 
        /// </summary>
        public Item TertiaryItem;
        
        /// <summary>
        /// 
        /// </summary>
        public Item QuaternaryItem;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void OnInput(Player player)
        {
            if (!player.IsListening())
            {
                return;
            }
            
            if (PrimaryItem)
            {
                player.ControllerInput.X.OncePressed(PrimaryItem.OnPress);
                player.ControllerInput.X.OnceHeld(PrimaryItem.OnHold);
                player.ControllerInput.X.OnceReleased(PrimaryItem.OnRelease);
            }

            if (SecondaryItem)
            {
                player.ControllerInput.Y.OncePressed(SecondaryItem.OnPress);
                player.ControllerInput.Y.OnceHeld(SecondaryItem.OnHold);
                player.ControllerInput.Y.OnceReleased(SecondaryItem.OnRelease);
            }

            if (TertiaryItem)
            {
                player.ControllerInput.L.OncePressed(TertiaryItem.OnPress);
                player.ControllerInput.L.OnceHeld(TertiaryItem.OnHold);
                player.ControllerInput.L.OnceReleased(TertiaryItem.OnRelease);
            }

            if (QuaternaryItem)
            {
                player.ControllerInput.R.OncePressed(QuaternaryItem.OnPress);
                player.ControllerInput.R.OnceHeld(QuaternaryItem.OnHold);
                player.ControllerInput.R.OnceReleased(QuaternaryItem.OnRelease);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="item"></param>
        public void Equip(ItemSlots slot, Item item)
        {
            switch (slot)
            {
                case ItemSlots.Primary:
                {
                    EquipPrimary(item);
                    
                    break;
                }
                case ItemSlots.Secondary:
                {
                    EquipSecondary(item);
                    
                    break;
                }
                case ItemSlots.Tertiary:
                {
                    EquipTertiary(item);

                    break;
                }
                case ItemSlots.Quaternary:
                {
                    EquipQuaternary(item);
                    
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, "Invalid slot.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected void EquipPrimary(Item item)
        {
            if (SecondaryItem == item)
            {
                SecondaryItem = PrimaryItem;
            }
            else if (TertiaryItem == item)
            {
                TertiaryItem = PrimaryItem;
            }
            else if (QuaternaryItem == item)
            {
                QuaternaryItem = PrimaryItem;
            }

            PrimaryItem = item;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected void EquipSecondary(Item item)
        {
            if (PrimaryItem == item)
            {
                PrimaryItem = SecondaryItem;
            }
            else if (TertiaryItem == item)
            {
                TertiaryItem = SecondaryItem;
            }
            else if (QuaternaryItem == item)
            {
                QuaternaryItem = SecondaryItem;
            }

            SecondaryItem = item;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected void EquipTertiary(Item item)
        {
            if (PrimaryItem == item)
            {
                PrimaryItem = TertiaryItem;
            }
            else if (SecondaryItem == item)
            {
                SecondaryItem = TertiaryItem;
            }
            else if (QuaternaryItem == item)
            {
                QuaternaryItem = TertiaryItem;
            }

            TertiaryItem = item;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected void EquipQuaternary(Item item)
        {
            if (PrimaryItem == item)
            {
                PrimaryItem = QuaternaryItem;
            }
            else if (SecondaryItem == item)
            {
                SecondaryItem = QuaternaryItem;
            }
            else if (TertiaryItem == item)
            {
                TertiaryItem = QuaternaryItem;
            }

            QuaternaryItem = item;
        }
    }
}