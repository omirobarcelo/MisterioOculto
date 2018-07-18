using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class PlayerManager : Character
    {

        public KeyCode InteractionKey;
        public bool CanInteract;

        public bool CanOpenInventory;

        // The invert movement for the X.
        public int PlayerInvertX = 1;
        // The invert movement for the Y.
        public int PlayerInvertY = 1;

        // The direction the player is facing
        private Vector2 facing = Vector2.up;

        // Maximum distance to interact
        private float interactionDist = 1f;


        private void Awake()
        {
            //characterEntity = GameObject.FindGameObjectWithTag("Player");
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log(facing);
            }
            Vector3 offset = characterEntity.GetComponent<Collider2D>().offset;
            Debug.DrawRay(characterEntity.GetComponent<Collider2D>().transform.position + offset, new Vector3(facing.x, facing.y, 0));

            if (Input.GetKeyDown(InteractionKey) && CanInteract)
            {
                RaycastHit2D hit = Physics2D.Raycast(characterEntity.transform.position + offset,
                                             new Vector3(facing.x, facing.y, 0),
                                             interactionDist, LayerMask.GetMask("Interactable"));
                if (hit.collider != null)
                {
                    IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                    if (interactable == null)
                    {
                        Debug.Log("No interaction possible or error");
                    }
                    else
                    {
                        interactable.Interact();
                    }
                }

            }
        }

        public void SetFacing(Vector2 facing)
        {
            this.facing = facing;
        }

        public Vector2 GetFacing()
        {
            return facing;
        }

        public float GetPlayerY()
        {
            return characterEntity.transform.position.y;
        }
    }
}
