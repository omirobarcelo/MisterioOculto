﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Shoguneko
{

    public class Inventory : MonoBehaviour
    {
        [Tooltip("The amount of default inventory slots. You will want this number to be the LOWEST number of slots the player can have.")]
        public int defaultSlotAmount = 10;
        [Tooltip("The panel that holds all of the inventory container elements.")]
        public GameObject inventoryPanel;
        [Tooltip("The panel that holds the container for the inventory slots.")]
        public GameObject slotPanel;
        [Tooltip("The GameObject that represents each individual inventory slot.")]
        public GameObject inventorySlot;
        [Tooltip("The GameObject that represents Item GameObject that is in each Inventory Slot.")]
        public GameObject inventoryItem;
        [Tooltip("The Key that is used to open or close your inventory.")]
        public KeyCode inventoryKey;
        [Tooltip("Optional - The sound that is played when the inventory opens.")]
        public AudioClip inventoryOpenSound;
        [Tooltip("Optional - The sound that is played when the inventory closes.")]
        public AudioClip inventoryCloseSound;

        public List<Item> items = new List<Item>();
        [HideInInspector]
        public List<GameObject> slots = new List<GameObject>();

        [Inject(InjectFrom.Anywhere)]
        public PlayerManager _player;

        public bool blocked;

        // Saved variable to let us know if we had any extensions
        private int extraInventorySlots = 0;

        // Selected slot
        private int selectedSlot = 0;

        private void Awake()
        {
            CreateSlots();
        }

        void Update()
        {
            if (blocked)
            {
                return;
            }
            // IF the inventory key is pushed and our Canvas is active (meaning we only care about opening the inventory when the player is active).
            if (Input.GetKeyDown(inventoryKey) && Grid.setup.UICanvas.activeInHierarchy && _player.CanOpenInventory)
            {
                // Open or Close the inventory.
                OpenCloseInventory();
            }
            if (inventoryPanel.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    // Unselect current slot
                    slots[selectedSlot].GetComponent<ItemSlot>().Unselect();
                    selectedSlot = (selectedSlot + 1) % (defaultSlotAmount + extraInventorySlots);
                    // TODO move scroll
                    // Select new slot
                    slots[selectedSlot].GetComponent<ItemSlot>().Select();
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    // Unselect current slot
                    slots[selectedSlot].GetComponent<ItemSlot>().Unselect();
                    // Make sure selected slot doesn't go to negatives
                    selectedSlot = (selectedSlot - 1) < 0 ? defaultSlotAmount + extraInventorySlots - 1 : selectedSlot - 1;
                    selectedSlot = selectedSlot % (defaultSlotAmount + extraInventorySlots);
                    // TODO move scroll
                    // Select new slot
                    slots[selectedSlot].GetComponent<ItemSlot>().Select();
                }
                else if (Input.GetKeyDown(Grid.setup.GetInteractionKey()))
                {
                    // if selected slot holds an item
                    if (items[selectedSlot].ID != -1)
                    {
                        slots[selectedSlot].GetComponentInChildren<ItemData>().Use();
                    }
                }
            }
        }

        public void AddItem(int id, int amount)
        {
            // Get the item based on the id.
            Item itemToAdd = Grid.itemDataBase.FetchItemByID(id);

            // IF our items list contains itemToAdd AND this item is stackable.
            if (items.Contains(itemToAdd) && itemToAdd.Stackable)
            {
                // Loop though the items list.
                for (int i = 0; i < items.Count; i++)
                {
                    // IF we already have this item in our inventory.
                    if (items[i].ID == id)
                    {
                        // Get the Item_Data component.
                        ItemData data = slots[i].GetComponentInChildren<ItemData>();
                        // Add the amount we picked up to what we already have.
                        data.amount += amount;
                        // Set the text to show our new amount we have.
                        data.GetComponentInChildren<Text>().text = data.amount.ToString();
                        // GTFO
                        return;
                    }
                }
            }

            // How ever many times we need to add this item.
            for (int j = 0; j < amount; j++)
            {
                // Loop through all the items.
                for (int i = 0; i < items.Count; i++)
                {
                    // IF this item slot is empty.
                    if (items[i].ID == -1)
                    {
                        // Set the item we have been given to this spot in the items list.
                        items[i] = itemToAdd;
                        // Create the GameObject.
                        GameObject itemObj = Instantiate(inventoryItem);
                        // Get the Item_Data Component.
                        ItemData idComp = itemObj.GetComponent<ItemData>();
                        // Set the item in the idComp.
                        idComp.item = itemToAdd;
                        // Set the slot number of this item.
                        idComp.slotNumber = i;
                        // Set the transform of itemObj.
                        itemObj.transform.SetParent(slots[i].transform);
                        itemObj.transform.localScale = Vector2.one;
                        itemObj.transform.localPosition = Vector2.zero;
                        // Offset text so it doesn't take the whole slot space
                        itemObj.GetComponent<RectTransform>().offsetMin = new Vector2(20, 10);
                        itemObj.GetComponent<RectTransform>().offsetMax = new Vector2(-20, -10);
                        // Adjust the sprite and color of the Image.
                        itemObj.GetComponent<Text>().text = itemToAdd.Name_en;
                        //itemObj.GetComponent<Image>().sprite = itemToAdd.SpriteImage;
                        //itemObj.GetComponent<Image>().color = new Color(itemToAdd.R, itemToAdd.G, itemToAdd.B, itemToAdd.A);
                        // Change the name of the itemObj GameObject to the name of the item.
                        itemObj.name = "Item Slot " + i + " - " + itemToAdd.Name_en;
                        // IF this item is stackable.
                        if (itemToAdd.Stackable)
                        {
                            // Set the amount for the idComp.
                            idComp.amount = amount;
                            // Set the text to show our new amount we have.
                            itemObj.GetComponentInChildren<Text>().text = idComp.amount.ToString();
                            return;
                        }
                        // If we come here then we have an item that isn't stackable so we just set the amount to 1.
                        idComp.amount = 1;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the amount of free spots in your inventory.
        /// </summary>
        /// <returns>The free spots.</returns>
        public int GetFreeSpots()
        {
            // New counter.
            int count = 0;
            // Loop through all the items.
            for (int i = 0; i < items.Count; i++)
            {
                // IF this item slot is empty.
                if (items[i].ID == -1)
                {
                    count++;
                }
            }
            return count;
        }

        public void OpenCloseInventory()
        {
            // IF the bag is open then play the close sound,
            // ELSE play the open sound.
            if (inventoryPanel.activeInHierarchy)
            {
                // Play the closed sound.
                Grid.soundManager.PlaySound(inventoryCloseSound);
                // Unselect selected slot
                ItemSlot slot = slots[selectedSlot].GetComponent<ItemSlot>();
                slot.Unselect();
                // Allow player to move and interact -> Separated because other scripts might change these values
                Grid.setup.StartPlayer();
            }
            else
            {
                // Play the open sound.
                Grid.soundManager.PlaySound(inventoryOpenSound);
                // Select first slot
                selectedSlot = 0;
                ItemSlot slot = slots[selectedSlot].GetComponent<ItemSlot>();
                slot.Select();
                // Stop player from moving and interacting -> Separated because other scripts might change these values
                Grid.setup.StopPlayer();
            }
            // Turn the inventory panel to the opposite activeness.
            inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        }

        //public void Save()
        //{
        //    // Create a new Player_Data.
        //    Saved_Inventory data = new Saved_Inventory();
        //    // Save the data.
        //    for (int i = 0; i < defaultSlotAmount + extraInventorySlots; i++)
        //    {
        //        // Assign in order the slot id.
        //        data.slotID.Add(items[i].ID);
        //        // IF in the items List we have an ID that isnt -1 so that means we have an actual item.
        //        // ELSE we have no item.
        //        if (items[i].ID != -1)
        //        {
        //            // Store the amount of this item.
        //            data.slotAmounts.Add(slots[i].GetComponentInChildren<Item_Data>().amount);
        //        }
        //        else
        //        {
        //            // There isn't an item here so add 0.
        //            data.slotAmounts.Add(0);
        //        }
        //    }
        //    // Save the extra inventory slots.
        //    data.extraInventorySlots = extraInventorySlots;
        //    // Turn the Saved_Inventory data to Json data.
        //    string inventoryToJson = JsonUtility.ToJson(data);
        //    // Save the information.
        //    PlayerPrefs.SetString("Inventory", inventoryToJson);
        //}

        //private void Load()
        //{
        //    // Grab the information on what is in the inventory.
        //    string inventoryJson = PlayerPrefs.GetString("Inventory");
        //    // IF there is nothing in this string.
        //    if (String.IsNullOrEmpty(inventoryJson))
        //    {
        //        // Create the slots on a default level.
        //        CreateSlots();
        //        // GTFO of here we done son!
        //        return;
        //    }
        //    // Turn the json data to the data to represent Saved_Inventory.
        //    Saved_Inventory data = JsonUtility.FromJson<Saved_Inventory>(inventoryJson);

        //    // Set our extra inventory slots before we create our inventory.
        //    extraInventorySlots = data.extraInventorySlots;
        //    // Create the default slots.
        //    CreateSlots();

        //    // Loop through however many inventory slots we have.
        //    for (int i = 0; i < defaultSlotAmount + extraInventorySlots; i++)
        //    {
        //        // Fetch the item based on the saved ID for this slot.
        //        Item fetchedItem = Grid.itemDataBase.FetchItemByID(data.slotID[i]);
        //        // IF we have an actual item.
        //        if (fetchedItem != null)
        //        {
        //            // Assign the item to the slot.
        //            items[i] = fetchedItem;
        //            // Create the item.
        //            GameObject itemObj = Instantiate(inventoryItem);
        //            // Get the Item_Data component.
        //            Item_Data iData = itemObj.GetComponentInChildren<Item_Data>();
        //            // Set the Item_Data item and slot number.
        //            iData.item = fetchedItem;
        //            iData.slotNumber = i;
        //            // Set the transform.
        //            itemObj.transform.SetParent(slots[i].transform);
        //            itemObj.transform.localScale = Vector2.one;
        //            itemObj.transform.localPosition = Vector2.zero;
        //            // Set the itemObj sprite, sprite color and name of the image.
        //            itemObj.GetComponent<Image>().sprite = fetchedItem.SpriteImage;
        //            itemObj.GetComponent<Image>().color = new Color(fetchedItem.R, fetchedItem.G, fetchedItem.B, fetchedItem.A);
        //            itemObj.name = "Item Slot " + i + " - " + fetchedItem.Title;
        //            // IF this fetched item is stackable.
        //            if (fetchedItem.Stackable)
        //            {
        //                // Set the Item_Data amount to what we had saved.
        //                iData.amount += data.slotAmounts[i];
        //                // Set the text to display how much we have.
        //                iData.GetComponentInChildren<Text>().text = iData.amount.ToString();
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// This will create the amount of GameObject slots.
        /// </summary>
        private void CreateSlots()
        {
            // Loop the amount of slots.
            for (int i = 0; i < defaultSlotAmount + extraInventorySlots; i++)
            {
                // Add a -1 Item.
                items.Add(new Item());
                // Create the Slot.
                slots.Add(Instantiate(inventorySlot));
                // Assign the Slot a slot number.
                slots[i].GetComponent<ItemSlot>().slotNumber = i;
                // Set the Slot parent to the Slot Panel.
                slots[i].transform.SetParent(slotPanel.transform);
                // Set the scaling to 1.
                slots[i].transform.localScale = Vector2.one;
            }
        }

        /// <summary>
        /// The amount of inventory slots to add or remove.  The parameter 'amount' will alter the extraInventorySlots variable but it wont set it to the amount number.  
        /// Example if you have 5 extra inventory slots and you use this method with the parameter of 3, you will then have 8 extra inventory slots.
        /// </summary>
        /// <param name="amount">Amount.</param>
        public void AddExtraSlots(int amount)
        {
            // Loop the amount of times we want to add a slot.
            for (int i = 0; i < amount; i++)
            {
                // Add a -1 Item.
                items.Add(new Item());
                // Create the Slot.
                slots.Add(Instantiate(inventorySlot));
                // Assign the Slot a slot number by taking the default and adding how many extra we have.
                slots[defaultSlotAmount + extraInventorySlots].GetComponent<ItemSlot>().slotNumber = defaultSlotAmount + extraInventorySlots;
                // Set the Slot parent to the Slot Panel.
                slots[defaultSlotAmount + extraInventorySlots].transform.SetParent(slotPanel.transform);
                // Set the scaling to 1.
                slots[defaultSlotAmount + extraInventorySlots].transform.localScale = Vector2.one;
                // Increase the extraInventorySlots.
                extraInventorySlots++;
            }
        }

        //public void LoadInventory()
        //{
        //    // So lets remove all the stuff in the inventory and make it a fresh start.
        //    DestroyInventory();
        //    // Time to load it up.
        //    Load();
        //}

        private void DestroyInventory()
        {
            // Destroy each slot.
            Grid.helper.DestroyGameObjectsByParent(slotPanel);
            // Make our slot and inventory Lists resort to their default.
            items = new List<Item>();
            slots = new List<GameObject>();
        }

        public void ClearInventory()
        {
            // loop through the amount of slots.
            for (int i = 0; i < slots.Count; i++)
            {
                // Destroy each child in the slots.
                Grid.helper.DestroyGameObjectsByParent(slots[i]);
                // Set each slot to an empty item.
                items[i] = new Item();
            }
        }

        public void PrintInventoryItems()
        {
            // Loop and print each title in the item.
            for (int i = 0; i < slots.Count; i++)
            {
                Debug.Log("Item " + i + " = " + items[i].Name_en);
            }
        }

        public bool CheckIfItemInInventory(int itemID, int amount = 1)
        {
            int slotNum = -1;
            for (int i = 0; i < items.Count; i++)
            {
                // If we have the item in the inventory, save the slot number
                if (items[i].ID == itemID)
                {
                    slotNum = i;
                    break;
                }
            }
            // If the item is in the inventory (slotNum != -1), check if we have the amount
            if (slotNum != -1 && slots[slotNum].GetComponentInChildren<ItemData>().amount >= amount)
            {
                return true;
            }
            return false;
        }
    }

    [Serializable]
    class Saved_Inventory
    {
        public List<int> slotID = new List<int>();
        public List<int> slotAmounts = new List<int>();
        public int extraInventorySlots = 0;
    }
}
