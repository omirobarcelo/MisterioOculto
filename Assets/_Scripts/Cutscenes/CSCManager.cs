using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shoguneko
{
    public class CSCManager : MonoBehaviour
    {
        // The GameObject that represents the character entity.
        public GameObject characterEntity;

        public bool CanMove = true;
        public float AlterSpeed = 2f;

        // The Character Animator.
        public Animator CharacterAnimator;

        [Tooltip("The directory for the character's dialogue.")]
        public string DialogueDir;

        private ActionKeyDialog akd;

        private void Awake()
        {
            // Assign the Animator Component.
            CharacterAnimator = characterEntity.GetComponent<Animator>();

            akd = GetComponentInChildren<ActionKeyDialog>();
            string[] arr = { Application.dataPath, "Text", Grid.optionsManager.lang, SceneManager.GetActiveScene().name, DialogueDir };
            string dialogPath = string.Join("/", arr);
            akd.getDialogueFiles(dialogPath);
            // TODO update dialogue with player's name
        }

        //// Use this for initialization
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}

        public Transform GetCharaTransform()
        {
            return characterEntity.transform;
        }

        public void Speak()
        {
            akd.CreateDialogue();
        }

        public bool Talking()
        {
            return akd.DialoguePlaying();
        }

        public void TurnUp()
        {
            CharacterAnimator.SetFloat("FaceX", 0f);
            CharacterAnimator.SetFloat("FaceY", 1f);
        }

        public void TurnRight()
        {
            CharacterAnimator.SetFloat("FaceX", 1f);
            CharacterAnimator.SetFloat("FaceY", 0f);
        }

        public void TurnDown()
        {
            CharacterAnimator.SetFloat("FaceX", 0f);
            CharacterAnimator.SetFloat("FaceY", -1f);
        }

        public void TurnLeft()
        {
            CharacterAnimator.SetFloat("FaceX", -1f);
            CharacterAnimator.SetFloat("FaceY", 0f);
        }
    }
}
