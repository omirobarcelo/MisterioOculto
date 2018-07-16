using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {

        // The GameObjects Rigidbody.
        //private Rigidbody2D rb;
        // The Player Manager.
        private PlayerManager _playerManager;
        // Holders for the movements.
        private float moveHorizontal;
        private float moveVertical;
        // For priority of movement press.
        private bool firstKeyPressed;
        private bool horizontalFirst;
        private bool verticalFirst;
        // Movement speed
        [SerializeField]
        private float speed = 0.1f;


        void Awake()
        {
            // Get the Player State.
            _playerManager = GetComponentInParent<PlayerManager>();
            // Get the Rigidbody2D Component.
            //rb = GetComponent<Rigidbody2D>();
            // Set the horizontalFirst to false as nothing is being pressed.
            firstKeyPressed = false;
            horizontalFirst = false;
            verticalFirst = false;
        }

        void Update()
        {
            // IF we are able to move.
            if (_playerManager.CanMove)
            {
                // Get a -1, 0 or 1.
                moveHorizontal = Input.GetAxisRaw("Horizontal");
                moveVertical = Input.GetAxisRaw("Vertical");
            }
        }

        void FixedUpdate()
        {
            // IF we are allowed to move.
            // ELSE IF we are not allowed to move unless we are jolted.
            if (_playerManager.CanMove)
            {

                // Get the First Key and Second Key press setup when moving.
                KeySetup();

                Vector2 movement;

                // IF we are moving horizontal first.
                // ELSE IF we are moving vertical first.
                // ELSE we are not even moving.
                if (horizontalFirst)
                {
                    // IF we are moving vertical.
                    // ELSE we are moving horizontal.
                    if (moveVertical != 0)
                    {
                        // Get the inverted directions.
                        movement = new Vector2(0f, moveVertical * _playerManager.PlayerInvertY);
                        // Play the animation.
                        PlayAnimation(0f, moveVertical);
                        //_playerManager.SetFacing(moveVertical > 0 ? Vector2.up : Vector2.down);
                    }
                    else
                    {
                        // Get the inverted directions.
                        movement = new Vector2(moveHorizontal * _playerManager.PlayerInvertX, 0f);
                        // Play the animation.
                        PlayAnimation(moveHorizontal, 0f);
                        //_playerManager.SetFacing(moveHorizontal > 0 ? Vector2.right : Vector2.left);
                    }

                    // Apply inverted directions with speed.
                    movement *= speed * _playerManager.AlterSpeed;
                    // Apply force.
                    //rb.AddForce(movement);
                    transform.Translate(movement.x, movement.y, 0f);
                    //Debug.Log(movement);
                }
                else if (verticalFirst)
                {
                    // IF we are moving horizontal.
                    // ELSE IF we are moving vertical.
                    if (moveHorizontal != 0)
                    {
                        // Get the inverted directions.
                        movement = new Vector2(moveHorizontal * _playerManager.PlayerInvertX, 0f);
                        // Play the animation.
                        PlayAnimation(moveHorizontal, 0f);
                        //_playerManager.SetFacing(moveHorizontal > 0 ? Vector2.right : Vector2.left);
                    }
                    else
                    {
                        // Get the inverted directions.
                        movement = new Vector2(0f, moveVertical * _playerManager.PlayerInvertY);
                        // Play the animation.
                        PlayAnimation(0f, moveVertical);
                        //_playerManager.SetFacing(moveVertical > 0 ? Vector2.up : Vector2.down);
                    }

                    // Apply inverted directions with speed.
                    movement *= speed * _playerManager.AlterSpeed;
                    // Apply force.
                    //rb.AddForce(movement);
                    transform.Translate(movement.x, movement.y, 0f);
                    //Debug.Log(movement);
                }
                else
                {
                    PlayAnimation(0f, 0f);
                }
            }
        }

        void KeySetup()
        {
            // IF there is not a key being pressed.
            // ELSE we are pressing a key for movement.
            if (!firstKeyPressed)
            {
                // IF we are moving horizontal.
                // ELSE IF we are moving vertical.
                if (moveHorizontal == -1 || moveHorizontal == 1)
                {
                    // First key pressed is moving Horizontal.
                    horizontalFirst = true;
                    // First key pressed is not Vertical.
                    verticalFirst = false;
                    // We are now pressing a key down.
                    firstKeyPressed = true;
                }
                else if (moveVertical == -1 || moveVertical == 1)
                {
                    // First key pressed is moving Vertical.
                    verticalFirst = true;
                    // First key pressed is moving Horizontal.
                    horizontalFirst = false;
                    // We are now pressing a key down.
                    firstKeyPressed = true;
                }
            }
            else
            {
                // IF we were holding down a key to move Horizontal as the first key pressed.
                if (horizontalFirst)
                {
                    // IF we are not moving Horizontal anymore.
                    if (moveHorizontal == 0)
                    {
                        // The first key to be pressed moving Horizontal is not being pressed anymore.
                        firstKeyPressed = false;
                    }
                }
                else
                {
                    // IF we are not moving Vertical anymore.
                    if (moveVertical == 0)
                    {
                        // The first key to be pressed moving Vertical is not being pressed anymore.
                        firstKeyPressed = false;
                    }
                }
            }
        }

        void PlayAnimation(float hor, float vert)
        {
            // IF the user has an animation set and ready to go.
            if (_playerManager.CharacterAnimator != null)
            {
                // Play animations.
                if (hor == 0f && vert == 0f)
                {
                    _playerManager.CharacterAnimator.Play("Idle");
                }
                else
                {
                    Vector2 facing = new Vector2(hor == 0f ? 0f : (hor > 0f ? 1f : -1f),
                                                 vert == 0f ? 0f : (vert > 0f ? 1f : -1f));
                    _playerManager.SetFacing(facing);
                    //Grid.helper.FourDirectionAnimation(hor * _playerManager.PlayerInvertX, vert * _playerManager.PlayerInvertY, _playerManager.CharacterAnimator);
                    _playerManager.CharacterAnimator.SetFloat("FaceX", hor);
                    _playerManager.CharacterAnimator.SetFloat("FaceY", vert);
                    _playerManager.CharacterAnimator.Play("Walking");
                }
            }
        }
    }
}
