using UnityEngine;

namespace Shoguneko
{

    public enum CharacterType { Player, NPC, All }
    public enum Facing {Up, Right, Down, Left}

    public abstract class Character : MonoBehaviour
    {

        // The type of character this is.
        public CharacterType characterType;
        // The GameObject that represents the character entity.
        public GameObject characterEntity;

        public bool CanMove = true;
        public float AlterSpeed = 1f;
        public bool Interactable = false;

        // The Character Animator.
        public Animator CharacterAnimator;

        // Is there a Action Key Dialogue currently running.
        public bool isActionKeyDialogueRunning = false;
        // The focus target for the Action Key Dialogue.
        public GameObject actionKeyFocusTarget;

        // Options for player interactions.

        //
        //		// The characters base movement speed.
        //		public float DefaultMoveSpeed = 1f;
        //	//	[ReadOnlyAttribute]
        //	//	public float BaseMoveSpeed;
        //		[ReadOnlyAttribute]
        //		public float CurrentMoveSpeed;

        void OnEnable()
        {
            // Add this to our List.
            CharacterManager.Register(this);
        }

        void OnDisable()
        {
            // Remove from our List.
            CharacterManager.Unregister(this);
        }
    }
}
