using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shoguneko
{
    public class NPCManager : Character, IInteractable
    {
        //[Inject(InjectFrom.Anywhere)]
        //public PlayerManager _playerManager;

        public string DialogueDirectory;
        public Facing InitialFacing;

        private ActionKeyDialog akd;

        private string dialogPath;

        void Awake()
        {
            // Assign the Animator Component.
            CharacterAnimator = characterEntity.GetComponent<Animator>();

            // Adjust NPC facing
            Turn(InitialFacing);

            // Set the dialogue system
            akd = GetComponentInChildren<ActionKeyDialog>();
            string[] arr = { Application.dataPath, "Text", Grid.optionsManager.lang, SceneManager.GetActiveScene().name, DialogueDirectory };
            dialogPath = string.Join("/", arr);
            akd.getDialogueFiles(dialogPath);
        }

        /*
        private IEnumerator NoCharacterControl()
        {
            // Make the character not be able to be controlled while the knockback is happening.
            CanMove = false;
            // Wait for 'noControlTime' time before being able to control the character again.
            yield return new WaitForSeconds(HitAnimationTime);
            // Stop the knockback.
            characterEntity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // We can now move the character.
            CanMove = true;
        }

        private IEnumerator HitAnimation()
        {
            CharacterAnimator.SetBool("IsHit", true);
            yield return new WaitForSeconds(HitAnimationTime);
            CharacterAnimator.SetBool("IsHit", false);
        }
        */

        public void Interact()
        {
            // Make the NPC face the player
            Vector2 playerPos = Grid.setup.player.characterEntity.transform.position;
            Vector2 NPCPos = characterEntity.transform.position;
            Vector2 posDiff = playerPos - NPCPos;
            float hor = Mathf.Abs(posDiff.x) < Mathf.Abs(posDiff.y) ? 0f : (playerPos.x > NPCPos.x ? 1f : -1f);
            float vert = Mathf.Abs(posDiff.x) > Mathf.Abs(posDiff.y) ? 0f : (playerPos.y > NPCPos.y ? 1f : -1f);
            CharacterAnimator.SetFloat("FaceX", hor);
            CharacterAnimator.SetFloat("FaceY", vert);

            // Record the interaction (since Interact executed also to close the dialogue, only record the first time)
            if (!akd.DialoguePlaying())
            {
                Grid.recorder.Interacted(DialogueDirectory);
            }
            // Create dialog
            akd.CreateDialogue();
        }

        private void Turn(Facing facing)
        {
            switch (facing)
            {
                case Facing.Up:
                    CharacterAnimator.SetFloat("FaceX", 0f);
                    CharacterAnimator.SetFloat("FaceY", 1f);
                    break;
                case Facing.Right:
                    CharacterAnimator.SetFloat("FaceX", 1f);
                    CharacterAnimator.SetFloat("FaceY", 0f);
                    break;
                case Facing.Down:
                    CharacterAnimator.SetFloat("FaceX", 0f);
                    CharacterAnimator.SetFloat("FaceY", -1f);
                    break;
                case Facing.Left:
                    CharacterAnimator.SetFloat("FaceX", -1f);
                    CharacterAnimator.SetFloat("FaceY", 0f);
                    break;
            }
        }
    }
}
