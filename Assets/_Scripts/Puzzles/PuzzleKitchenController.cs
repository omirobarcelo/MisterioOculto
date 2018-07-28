using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shoguneko
{
    public class PuzzleKitchenController : MonoBehaviour
    {
        readonly Color COL_DEF = new Color(1f, 1f, 1f);
        readonly Color COL_ON = new Color(1f, 1f, 0.75f);
        readonly Color COL_SEL = new Color(0.8f, 0.8f, 0.8f);

        public UnityEvent SolvedA;
        public UnityEvent SolvedB;

        [Tooltip("The tiles from left to right, and from top to bottom.")]
        public SpriteRenderer[] tiles;
        [Tooltip("The number of columns.")]
        public int columns;

        [System.Serializable]
        public struct Point
        {
            public int x, y;
            public Point(int px, int py)
            {
                x = px;
                y = py;
            }
        }
        public Point[] SolutionA;
        public Point[] SolutionB;

        int i, j;

        Color prevColor;

        HashSet<Point> playerSolution;

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
                if (string.IsNullOrEmpty(PlayerPrefs.GetString("kitchen_mid", null)))
                {
                    Grid.helper.ChangeScene("Kitchen1", "kitchen_cave_01");
                }
                else
                {
                    Grid.helper.ChangeScene("KitchenMid", "kitchen_cave_01");
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Set the current tile to its previous color
                tiles[j * columns + i].color = prevColor;

                // Hover over the next tile
                i = (i - 1) < 0 ? columns - 1 : i - 1;
                prevColor = tiles[j * columns + i].color;
                tiles[j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Set the current tile to its previous color
                tiles[j * columns + i].color = prevColor;

                // Hover over the next tile
                i = (i + 1) % columns;
                prevColor = tiles[j * columns + i].color;
                tiles[j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Set the current tile to its previous color
                tiles[j * columns + i].color = prevColor;

                // Hover over the next tile
                j = (j - 1) < 0 ? tiles.Length/columns - 1 : j - 1;
                prevColor = tiles[j * columns + i].color;
                tiles[j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Set the current tile to its previous color
                tiles[j * columns + i].color = prevColor;

                // Hover over the next tile
                j = (j + 1) % columns;
                prevColor = tiles[j * columns + i].color;
                tiles[j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Select and save the current tile
                //tiles[j * columns + i].color = COL_SEL;
                prevColor = COL_SEL; // This way keeps in ON, but when we move will be SEL
                playerSolution.Add(new Point(i, j));

                //foreach (var point in playerSolution)
                //{
                //    Debug.Log("(" + point.x + ", " + point.y + ")");
                //}

                bool correctA = false, correctB = false;
                // Check solution A
                if (correctA = playerSolution.Count == SolutionA.Length)
                {
                    foreach (var point in SolutionA)
                    {
                        correctA &= playerSolution.Contains(point);
                    }
                }
                // Check solution B
                if (correctB = playerSolution.Count == SolutionB.Length)
                {
                    foreach (var point in SolutionB)
                    {
                        correctB &= playerSolution.Contains(point);
                    }
                }

                if (correctA || correctB)
                {
                    if (correctA)
                    {
                        Debug.Log("A");
                        SolvedA.Invoke();
                    }
                    if (correctB)
                    {
                        Debug.Log("B");
                        SolvedB.Invoke();
                    }
                    PlayerPrefs.DeleteKey("kitchen_mid");
                    PlayerPrefs.SetString("kitchen_2", "true");
                    Grid.helper.ChangeScene("CutsceneSend");
                }
                else
                {
                    // Reset if the solution is not correct and the player pressed
                    // the same or more tiles than the solution requires
                    if (playerSolution.Count >= Mathf.Max(SolutionA.Length, SolutionB.Length))
                    {
                        Reset();
                    }
                }
            }
        }

        private void Reset()
        {
            // Set all tiles to its default color
            foreach (var tile in tiles)
            {
                tile.color = COL_DEF;
            }

            // Hover over first tile
            i = 0; j = 0;
            prevColor = tiles[0].color;
            tiles[0].color = COL_ON;

            playerSolution = new HashSet<Point>();
        }
    }
}
