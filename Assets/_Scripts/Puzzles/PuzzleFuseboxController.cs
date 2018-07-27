using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class PuzzleFuseboxController : MonoBehaviour
    {
        [Tooltip("The fuses gameobjects. Should be sorted from low ot high.")]
        public GameObject[] fuses;
        public Transform[] positions;

        [Tooltip("The correct positions of the fuses, sorted in the fuse order.")]
        public Transform[] Solution;

        List<Transform> freePositions;
        int index;

        int currentFuse;

        int[] playerSolution;

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
                Grid.helper.ChangeScene("Outside3", "init");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                index = (index - 1) < 0 ? freePositions.Count - 1 : index - 1;
                fuses[currentFuse].transform.position = freePositions[index].position;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                index = (index + 1) % freePositions.Count;
                fuses[currentFuse].transform.position = freePositions[index].position;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Save player solution
                playerSolution[currentFuse] = index;

                // Check if puzzle solved correctly when all fuses are set
                if (++currentFuse == fuses.Length)
                {
                    bool correct = true;
                    for (int i = 0; i < Solution.Length; i++)
                    {
                        correct &= (Solution[i].position == fuses[i].transform.position);
                    }

                    if (correct)
                    {
                        //Debug.Log("done");
                        Grid.inventory.RemoveItem(1);
                        Grid.helper.ChangeScene("Outside4", "init");
                    }
                    else
                    {
                        Reset();
                    }
                }
                else
                {
                    // Remove current position from the free positions
                    freePositions.RemoveAt(index);

                    // Show the next fuse
                    fuses[currentFuse].transform.position = freePositions[index = 0].position;
                    fuses[currentFuse].SetActive(true);
                }
            }
        }

        private void Reset()
        {
            // Make sure all the fuses are not active
            foreach (var fuse in fuses)
            {
                fuse.SetActive(false);
            }

            // Init list
            freePositions = new List<Transform>(positions);
            index = 0;

            currentFuse = 0;


            // Reset player solution
            playerSolution = new int[Solution.Length];

            // Get the first fuse and show it in the first free position
            fuses[currentFuse = 0].transform.position = freePositions[index = 0].position;
            fuses[currentFuse].SetActive(true);
        }
    }
}
