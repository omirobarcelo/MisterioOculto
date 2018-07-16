using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shoguneko
{
    public class NPCManager : Character, IInteractable
    {
        [Inject(InjectFrom.Anywhere)]
        public PlayerManager _playerManager;

        private ActionKeyDialog akd;

        private string dialogPath;
        private string dialogDir = "trace";

        void Awake()
        {
            // Assign the Animator Component.
            CharacterAnimator = characterEntity.GetComponent<Animator>();

            akd = GetComponentInChildren<ActionKeyDialog>();
            string[] arr = { Application.dataPath, "Text", Grid.optionsManager.lang, SceneManager.GetActiveScene().name, dialogDir };
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
            Debug.Log("It's me, Trace!");

            // Make the NPC face the player
            // TODO switch to setup get player transform
            Vector2 playerPos = _playerManager.characterEntity.transform.position;
            Vector2 NPCPos = characterEntity.transform.position;
            Vector2 posDiff = playerPos - NPCPos;
            float hor = Mathf.Abs(posDiff.x) < Mathf.Abs(posDiff.y) ? 0f : (playerPos.x > NPCPos.x ? 1f : -1f);
            float vert = Mathf.Abs(posDiff.x) > Mathf.Abs(posDiff.y) ? 0f : (playerPos.y > NPCPos.y ? 1f : -1f);
            CharacterAnimator.SetFloat("FaceX", hor);
            CharacterAnimator.SetFloat("FaceY", vert);

            // Create dialog
            akd.CreateDialogue();
        }
    }
}
