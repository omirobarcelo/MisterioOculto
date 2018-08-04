using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class PuzzleDanceController : MonoBehaviour
    {
        readonly Color COL_DEF = new Color(1f, 1f, 1f);
        readonly Color COL_ON = new Color(1f, 1f, 0.75f);

        [System.Serializable]
        public struct AudioKey
        {
            [Tooltip("The SpriteRenderer of the key gameobject.")]
            public SpriteRenderer key;
            [Tooltip("The note to play when pressing this key.")]
            public AudioClip note;
        }
        [Tooltip("The keys from left to right.")]
        public AudioKey[] keys;

        public int[] Solution;

        int index;

        List<int> playerSolution;

        // Use this for initialization
        void Start()
        {
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Grid.helper.ChangeScene("DanceRoom1", "init");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Return current key to its default color
                keys[index].key.color = COL_DEF;

                // Hover over next key
                index = (index - 1) < 0 ? keys.Length - 1 : index - 1;
                keys[index].key.color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Return current key to its default color
                keys[index].key.color = COL_DEF;

                // Hover over next key
                index = (index + 1) % keys.Length;
                keys[index].key.color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Play sound and save player's choice
                Grid.soundManager.PlaySound(keys[index].note);
                playerSolution.Add(index);

                if (playerSolution.Count == Solution.Length)
                {
                    bool correct = true;
                    for (int i = 0; i < Solution.Length; i++)
                    {
                        correct &= playerSolution[i] == Solution[i];
                    }

                    if (correct)
                    {
                        //Debug.Log("done");
                        StartCoroutine(Grid.helper.WaitAndChangeScene(1.5f, "DanceRoom2", "init"));
                        PlayerPrefs.SetString("dance", "true");
                        //Grid.helper.ChangeScene("DanceRoom2", "init");
                    }
                    else
                    {
                        foreach (var key in keys)
                        {
                            Grid.soundManager.PlaySound(key.note);
                        }
                        Reset();
                    }
                }
            }
        }

        private void Reset()
        {
            // Set all the keys to its default color
            foreach (var key in keys)
            {
                key.key.color = COL_DEF;
            }

            // Hover over the first key
            keys[index = 0].key.color = COL_ON;

            // Reset player solution
            playerSolution = new List<int>();
        }

        IEnumerator WaitAndC(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}
