using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shoguneko
{
    public class GetItem : MonoBehaviour, IInteractable
    {
        public UnityEvent interacted;

        public bool DisableAfterInteraction;
        private bool disabled = false;

        public int itemID;
        public int obtainedAmount;
        public int possessedAmount;

        public bool sharedItem;
        public GetItem[] sharedWith;

        private ActionKeyDialog akd;
        private readonly string AMOUNT = "<amount>";
        private readonly string NAME = "<name>";

        private void Awake()
        {
            akd = GetComponentInChildren<ActionKeyDialog>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Necessary because akd.CreateDialogue() starts a coroutine and
            // we need to wait until it ends completely because deactivating 
            // the object, or else the player gets frozen
            if (disabled && Grid.setup.PlayerInControl())
            {
                gameObject.SetActive(false);
            }

        }

        public void Interact()
        {
            if (akd.DialoguePlaying())
            {
                akd.CreateDialogue();
                // There should be only one line of dialogue, so we can disable it here
                if (DisableAfterInteraction)
                {
                    disabled = true;
                }
            }
            else
            {
                if (possessedAmount > 0)
                {
                    int amount = possessedAmount >= obtainedAmount ? obtainedAmount : possessedAmount;
                    Grid.inventory.AddItem(itemID, amount);
                    possessedAmount -= amount;

                    if (sharedItem)
                    {
                        // Update the shared interactables
                        foreach (var shared in sharedWith)
                        {
                            shared.possessedAmount -= amount;
                        }
                    }

                    Grid.soundManager.PlaySound(Grid.itemDataBase.FetchItemByID(itemID).PickUpSound);

                    string[] arr = { Application.dataPath, "Text", Grid.optionsManager.lang, "General", "getItem" };
                    string dialogPath = string.Join("/", arr);
                    akd.getDialogueFiles(dialogPath);
                    // Replace text with item information
                    // In theory, only 1 string
                    for (int i = 0; i < akd.dialogue.Length; i++)
                    {
                        for (int j = 0; j < akd.dialogue[i].Length; j++)
                        {
                            akd.dialogue[i][j] = akd.dialogue[i][j].Replace(AMOUNT, amount.ToString());
                            akd.dialogue[i][j] = akd.dialogue[i][j].Replace(NAME, Grid.itemDataBase.FetchItemByID(itemID).Name_en);
                        }
                    }

                    interacted.Invoke();

                    akd.CreateDialogue();
                }
            }
        }
    }
}
