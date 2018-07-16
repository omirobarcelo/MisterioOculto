using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shoguneko
{

    [RequireComponent(typeof(RectTransform))]
    public class Dialog : MonoBehaviour
    {

        public Image dialogueImage;
        public Text dialogueText;
        public Image portrait;

        private Text _dialogueText;
        private Color _dialogueColor;
        private Color _textColor;
        private float _initialDialogueUIAlpha;
        private float _initialDialogueTextAlpha;
        private Vector3 _initialDialogueScale;

        void Awake()
        {
            // Set the txt initially to "".
            dialogueText.text = "";
            // Grab the initial dialogue UI alpha.
            _initialDialogueUIAlpha = dialogueImage.color.a;
            // Grab the initial dialogue text alpha.
            _initialDialogueTextAlpha = dialogueText.color.a;
            // Grab the initial dialogue scale.
            _initialDialogueScale = dialogueImage.transform.localScale;
        }

        void Start()
        {
            // Check to make sure the user has the scripts working correctly.
            //DebugCheck();
        }



        public void SetDialogueUIColors(Color diaColor, Color txtColor)
        {
            // Assign new colors to private variables.
            _dialogueColor = diaColor;
            _textColor = txtColor;

            // Set the dialogue and text colors.
            dialogueImage.color = _dialogueColor;
            dialogueText.color = _textColor;
        }

        public void SetDialogueUIAlpha(float diaAlpha)
        {
            // When we create the dialogue box/text colors we always start with alpha at 0.
            _dialogueColor.a = diaAlpha;
            // Set the dialogue and text colors.
            dialogueImage.color = _dialogueColor;

        }

        public void SetDialogueTextAlpha(float txtAlpha)
        {
            _textColor.a = txtAlpha;
            dialogueText.color = _textColor;
        }

        public void SetInitialDialogueUIAlpha()
        {
            _dialogueColor.a = _initialDialogueUIAlpha;
            dialogueImage.color = _dialogueColor;
        }

        public void SetInitialDialogueTextAlpha()
        {
            _textColor.a = _initialDialogueTextAlpha;
            dialogueText.color = _textColor;
        }

        public void SetInitialDialogueScale()
        {
            gameObject.transform.localScale = _initialDialogueScale;
        }

        public void SetDialogueScale(float diaXScale, float diaYScale)
        {
            gameObject.transform.localScale = new Vector3(diaXScale, diaYScale, 1f);
        }

        public void SwitchText(string newText)
        {
            // Change the dialogueText to newText.
            dialogueText.text = newText;
        }

        public void SwitchPortrait(Sprite img)
        {
            portrait.GetComponent<Image>().sprite = img;
        }

        public float GetInitialDialogueUIAlpha()
        {
            return _initialDialogueUIAlpha;
        }

        public float GetInitialDialogueTextAlpha()
        {
            return _initialDialogueTextAlpha;
        }

        public Vector3 GetInitialDialogueScale()
        {
            return _initialDialogueScale;
        }
    }
}
