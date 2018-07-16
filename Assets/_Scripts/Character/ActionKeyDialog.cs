using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Shoguneko
{
    public class ActionKeyDialog : MonoBehaviour
    {

        /// The Dialogue UI GameObject that will be displayed.
        public GameObject dialogueBox;
        /// The color alteration for the 'Dialogue UI'.  Leaving this color 'white' will keep the same look for your 'Dialogue UI' GameObject.
        public Color dialogueColor = Color.white;

        /// Set to 'true' if you want dialogue UI transitions to happen after each dialogue string in the 'Dialogue' array.
        public bool multipleTransitions = false;
        /// Set to 'true' if you want the dialogue UI transition to appear/disappear instantly.
        public bool isInstantDialogue = true;
        /// Set to 'true' if you want the dialogue UI transition to fade.
        public bool isFadeDialogue;
        /// The fade time for when a dialogue box fades in and fades out.
        public float fadeTime = 0.5f;
        /// Set to 'true' if you want the dialogue UI transition to grow and shrink.
        public bool isGrowShrinkDialogue;
        /// The grow/shrink time for when a dialogue box grows in and shrinks out.
        public float growShrinkTime = 0.6f;

        /// The color of the dialogue text.
        public Color dialogueTextColor = Color.white;
        /// Set to 'true' if you want the text transition to to appear/disappear instantly.
        public bool instantText;
        /// Set to 'true' if you want the text transition to be faded in and out.
        public bool fadeText;
        /// The time at which the text is faded in and out.
        public float textFadeTime = 0.8f;
        /// Set to 'true' if you want the text transition to be typed out.
        public bool typedText;
        /// The time it takes for the next letter to be displayed. 
        /// Increasing this number slows the typing speed of the dialogue text while decreasing this number speeds up the typing speed of the dialogue.
        public float dialogueTextPause = 0.1f;
        [Tooltip("The text that is displayed in the Dialogue UI.")]
        [Multiline]
        public string[][] dialogue;
        public bool usePortrait;
        private char SEP = '=';
        private int idStatus = 0;
        private int idText = 1;
        [System.Serializable]
        public struct NamedSprite
        {
            public string name;
            public Sprite sprite;
        }
        public NamedSprite[] portraits;
        private Dictionary<string, Sprite> dPortraits;

        // The index in the dialogue.
        private int dialogueIndex = 0;
        private int dialogueFile = 0;
        // The bool to let us know we are currently in a transition.
        private bool isInTransition = false;
        // The dialogue component.
        private Dialog dialogueComponent;
        // The Player State.
        [Inject(InjectFrom.Anywhere)]
        public PlayerManager _playerManager;
        // The Character component on the main object.
        private Character chara;


        void Awake()
        {
            // IF there is a mainObject.
            if (GetComponentInParent<Character>() != null)
            {
                // The Character component.
                chara = GetComponentInParent<Character>();
            }
            // Get the Dialogue Component.
            dialogueComponent = dialogueBox.GetComponent<Dialog>();
            // Check to make sure the user has the scripts working correctly.
            //DebugCheck();
            // Set the color for the dialogue box.
            dialogueComponent.SetDialogueUIColors(dialogueColor, dialogueTextColor);

            dPortraits = new Dictionary<string, Sprite>();
            foreach (NamedSprite elem in portraits)
            {
                dPortraits.Add(elem.name, elem.sprite);
            }
        }

        // Create the Dialogue box.
        public void CreateDialogue()
        {
            // IF we are currently in a dialogue transition.
            if (isInTransition)
            {
                // Do nothing and let the dialogue keep going
                return;
            }
            // IF we are not currently engaged in this Action Key Dialogue.  The dialogue box will be inactive if we are not engaged in it.
            if (!dialogueBox.activeInHierarchy)
            {
                // Freeze the player.
                _playerManager.CanMove = false;
                _playerManager.CanOpenInventory = false;
                // IF a Character script exists.
                if (chara != null)
                {
                    // Make this GameObject not be able to move.
                    chara.CanMove = false;
                    // Let everything know that this GameObject has or has not a running dialogue.
                    chara.isActionKeyDialogueRunning = true;
                    // Let everything know who the focus of this Dialogue is.
                    chara.actionKeyFocusTarget = _playerManager.characterEntity.gameObject;
                }
                // Make the Dialogue Box Active.
                dialogueBox.SetActive(true);
            }
            // Go to the first, middle or last part of the dialogue.
            StartCoroutine(GoToNextDialogue());
        }

        // Go to the next part of the dialogue.
        private IEnumerator GoToNextDialogue()
        {
            // We are now in a Transition.
            isInTransition = true;
            // IF we are on the first message.
            // ELSE IF we are on the last message.
            // ELSE we are in the middle of the entire dialogue.
            if (dialogueIndex == 0)
            {
                // The first transition.
                yield return StartCoroutine(DialogueIn());
            }
            else if (dialogueIndex >= dialogue[dialogueFile].Length)
            {
                // The last transition.
                yield return StartCoroutine(DialogueOut());
                // Reset the Dialogue.
                ResetDialogue();
                // Break out we are done.
                yield break;
            }
            else
            {
                // IF we want multiple dialogue transitions,
                // ELSE we just want the text to be changed.
                if (multipleTransitions)
                {
                    // Take away the dialogue background.
                    yield return StartCoroutine(DialogueOut());
                    // Bring in the dialogue background.
                    yield return StartCoroutine(DialogueIn());
                }
                else
                {
                    // Take away the dialogue text.
                    yield return StartCoroutine(DialogueTextOut());
                    // Bring in the dialogue text.
                    yield return StartCoroutine(DialogueTextIn());
                }
            }
            // We are now not in a Transition.
            isInTransition = false;
            // Increase the dialogueIndex.
            dialogueIndex++;
        }

        private IEnumerator DialogueIn()
        {
            string[] text;
            // Based on the transition, Set the start variables.
            InitDialogue();
            // IF we want to fade in the dialogue,
            // ELSE IF we want to grow the dialogue,
            // ELSE IF we want to instantly show the dialogue.
            if (isFadeDialogue)
            {
                // Fade in image.
                StartCoroutine(GUIHelper.FadeImage(dialogueComponent.dialogueImage, fadeTime, 0f, dialogueComponent.GetInitialDialogueUIAlpha()));
                // IF we have fading text as well.
                if (fadeText)
                {
                    // Switch to the new text.
                    if (usePortrait)
                    {
                        text = dialogue[dialogueFile][dialogueIndex].Split(SEP);
                        dialogueComponent.SwitchPortrait(dPortraits[text[idStatus]]);
                        dialogueComponent.SwitchText(text[idText]);
                    }
                    else
                    {
                        dialogueComponent.SwitchText(dialogue[dialogueFile][dialogueIndex]);
                    }
                    // Fade in text.
                    StartCoroutine(GUIHelper.FadeText(dialogueComponent.dialogueText, textFadeTime, 0f, dialogueComponent.GetInitialDialogueTextAlpha()));
                    // Wait for the longer time of the dialogue fade or the dialogue fade text time.
                    yield return new WaitForSeconds(Mathf.Max(fadeTime, textFadeTime));
                    yield break;
                }
                // Wait for the length of the Dialogue Box fade.
                yield return new WaitForSeconds(fadeTime);
            }
            else if (isGrowShrinkDialogue)
            {
                // Grow the dialogue.
                yield return StartCoroutine(GUIHelper.GrowShrinkImage(dialogueComponent.dialogueImage, growShrinkTime, dialogueComponent.dialogueImage.transform.localScale.x, dialogueComponent.dialogueImage.transform.localScale.y, dialogueComponent.GetInitialDialogueScale().x, dialogueComponent.GetInitialDialogueScale().y));
            }
            else if (isInstantDialogue)
            {
                // No Coroutines needed but leaving this here incase you want to implement something.
            }

            // Switch to the new text.
            //dialogueComponent.SwitchText(dialogue[dialogueFile][dialogueIndex]);
            if (usePortrait)
            {
                text = dialogue[dialogueFile][dialogueIndex].Split(SEP);
                dialogueComponent.SwitchPortrait(dPortraits[text[idStatus]]);
                dialogueComponent.SwitchText(text[idText]);
            }
            else
            {
                dialogueComponent.SwitchText(dialogue[dialogueFile][dialogueIndex]);
            }

            // IF we want to fade the text,
            // ELSE IF we want the text to be typed out,
            // ELSE IF we want the text to be displayed instantly.
            if (fadeText)
            {
                // Start the fade on the text.
                yield return StartCoroutine(GUIHelper.FadeText(dialogueComponent.dialogueText, textFadeTime, 0f, dialogueComponent.GetInitialDialogueTextAlpha()));
            }
            else if (typedText)
            {
                // Dont move forward until the typing of the text is finished.
                yield return StartCoroutine(GUIHelper.TypeText(dialogueComponent.dialogueText, dialogueTextPause, dialogue[dialogueFile][dialogueIndex]));
            }
            else if (instantText)
            {
                // No Coroutine needed but leaving this here incase you want to implement something.
            }
        }

        private IEnumerator DialogueOut()
        {
            // IF we want the dialogue to fade,
            // ELSE IF we want to shrink the dialogue,
            // ELSE IF we want the dialogue to disappear instantly.
            if (isFadeDialogue)
            {
                // Fade out the dialogue.
                StartCoroutine(GUIHelper.FadeImage(dialogueComponent.dialogueImage, fadeTime, dialogueComponent.dialogueImage.color.a, 0f));
                // We fade the text.
                yield return StartCoroutine(GUIHelper.FadeText(dialogueComponent.dialogueText, fadeTime, dialogueComponent.dialogueText.color.a, 0f));
            }
            else if (isGrowShrinkDialogue)
            {
                // Shrink the dialogue.
                yield return StartCoroutine(GUIHelper.GrowShrinkImage(dialogueComponent.dialogueImage, growShrinkTime, dialogueComponent.dialogueImage.transform.localScale.x, dialogueComponent.dialogueImage.transform.localScale.y, 0f, 0f));
            }
            else if (isInstantDialogue)
            {
                // No Coroutine needed but leaving this here incase you want to implement something.
            }
            // Switch to a blank text.
            dialogueComponent.SwitchText("");
        }

        private IEnumerator DialogueTextIn()
        {
            // Switch the dialogue text.
            //dialogueComponent.SwitchText(dialogue[dialogueFile][dialogueIndex]);
            if (usePortrait)
            {
                string[] text = dialogue[dialogueFile][dialogueIndex].Split(SEP);
                dialogueComponent.SwitchPortrait(dPortraits[text[idStatus]]);
                dialogueComponent.SwitchText(text[idText]);
            }
            else
            {
                dialogueComponent.SwitchText(dialogue[dialogueFile][dialogueIndex]);
            }
            // IF we want to fade the text,
            // ELSE IF we want the text to be typed out,
            // ELSE IF we want the text to appear instantly.
            if (fadeText)
            {
                yield return StartCoroutine(GUIHelper.FadeText(dialogueComponent.dialogueText, textFadeTime, 0f, dialogueComponent.GetInitialDialogueTextAlpha()));
            }
            else if (typedText)
            {
                // Dont move forward until the typing of the text is finished.
                yield return StartCoroutine(GUIHelper.TypeText(dialogueComponent.dialogueText, dialogueTextPause, dialogue[dialogueFile][dialogueIndex]));
            }
            else if (instantText)
            {
                // No Coroutine needed but leaving this here incase you want to implement something.
            }
        }

        private IEnumerator DialogueTextOut()
        {
            // IF we want the dialogue to fade,
            // ELSE IF we have the typed text OR instant text to transition out.
            if (fadeText)
            {
                // We fade the text.
                yield return StartCoroutine(GUIHelper.FadeText(dialogueComponent.dialogueText, textFadeTime, dialogueComponent.dialogueText.color.a, 0f));
            }
            else if (typedText)
            {
                // No Coroutine needed but leaving this here incase you want to implement something.
            }
            else if (instantText)
            {
                // No Coroutine needed but leaving this here incase you want to implement something.
            }
            // Switch to a blank.
            dialogueComponent.SwitchText("");
        }

        /// <summary>
        /// Handle the default settings for when the dialogue box is being shown.
        /// </summary>
        private void InitDialogue()
        {
            // IF we have a fading dialogue,
            // ELSE IF we have a grow/shrinking dialogue,
            // ELSE IF we have a instant dialogue.
            if (isFadeDialogue)
            {
                // Set the alpha to 0 since we fade in.
                dialogueComponent.SetDialogueUIAlpha(0f);
                // Set the initial scaling of this Dialogue Box.
                dialogueComponent.SetInitialDialogueScale();
            }
            else if (isGrowShrinkDialogue)
            {
                // Set the alpha to what it was initially.
                dialogueComponent.SetInitialDialogueUIAlpha();
                // Set the scale to 0 as we will be growing the dialogue box.
                dialogueComponent.SetDialogueScale(0f, 0f);
            }
            else if (isInstantDialogue)
            {
                // Set the initial alpha.
                dialogueComponent.SetInitialDialogueUIAlpha();
                // Set the initial scaling of this Dialogue Box.
                dialogueComponent.SetInitialDialogueScale();
            }

            // IF we want the text to fade,
            // ELSE IF we want the text to by typed out,
            // ELSE IF we want hte text to by instantly displayed.
            if (fadeText)
            {
                dialogueComponent.SetDialogueTextAlpha(0f);
            }
            else if (typedText)
            {
                // Set the initial alpha of the text to what the developer had before runtime.
                dialogueComponent.SetInitialDialogueTextAlpha();
            }
            else if (instantText)
            {
                // Set the initial alpha of the text to what the developer had before runtime.
                dialogueComponent.SetInitialDialogueTextAlpha();
            }
        }

        private void ResetDialogue()
        {
            // Update dialogueFile
            dialogueFile = (dialogueFile + 1) % dialogue.Length;
            // Reset the dialogueIndex.
            dialogueIndex = 0;
            // We are now not in a Transition.
            isInTransition = false;
            // IF the DialogueBox is active then deactivate it.
            if (dialogueBox.activeInHierarchy)
            {
                // deactivate the dialogue box.
                dialogueBox.SetActive(false);
            }
            // IF the players closest action key dialogue is this gameobject.
            /*if (_playerManager.ClosestAKD == this)
            {
                // Set the closest action key dialogue to null.
                _playerManager.ClosestAKD = null;
                // Set the bool to show if this is an action key dialogue to false.
                _playerManager.IsActionKeyDialogued = false;
            }*/
            // Unfreeze the player.
            _playerManager.CanMove = true;
            _playerManager.CanOpenInventory = true;
            // IF a Character script exists.
            if (chara != null)
            {
                // Make this GameObject not be able to move.
                chara.CanMove = true;
                // Let everything know that this GameObject has or has not a running dialogue.
                chara.isActionKeyDialogueRunning = false;
                // Let everything know who the focus of this Dialogue is.
                chara.actionKeyFocusTarget = null;
            }
        }



        // Used for displaying collider information on the Scene View.
        private void SceneCircleCollider(CircleCollider2D coll, Color areaColor)
        {
#if UNITY_EDITOR
            // Set the color.
            UnityEditor.Handles.color = areaColor;
            // Get the offset.
            Vector3 offset = coll.offset;
            // Get the position of the collider gameobject.
            Vector3 discCenter = coll.transform.position;
            // Scaling incase the gameobject has been scaled.
            float scale;
            // IF the x scale is larger than the y scale.
            if (transform.lossyScale.x > transform.lossyScale.y)
            {
                // Make scale the size of the x.
                scale = transform.lossyScale.x;
            }
            else
            {
                // Make scale the size of the y.
                scale = transform.lossyScale.y;
            }
            // Draw the Disc on the Scene View.
            UnityEditor.Handles.DrawWireDisc(discCenter + offset, Vector3.back, coll.radius * scale);
#endif
        }

        // Used for displaying collider information on the Scene View.
        private void SceneBoxCollider(BoxCollider2D coll, Color areaColor)
        {
            // Set the color.
            Gizmos.color = areaColor;
            // Get the offset.
            Vector3 offset = coll.offset;
            // Get the position of the collider gameobject.
            Vector3 boxCenter = coll.transform.position;
            // Draw the Box on the Scene View.
            Gizmos.DrawWireCube(boxCenter + offset, new Vector2(coll.size.x * transform.lossyScale.x, coll.size.y * transform.lossyScale.y));
        }

        public void getDialogueFiles(string dialogPath)
        {
            string[] fileEntries = Directory.GetFiles(dialogPath);
            List<string[]> files = new List<string[]>();
            foreach (string file in fileEntries)
            {
                if (file.EndsWith(".txt"))
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        List<string> lines = new List<string>();
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            lines.Add(line);
                        }
                        files.Add(lines.ToArray());
                    }
                }
            }
            dialogue = files.ToArray();
        }

        public bool DialoguePlaying()
        {
            return dialogueBox.activeSelf;
        }
    }
}
