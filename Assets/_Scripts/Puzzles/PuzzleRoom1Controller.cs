using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shoguneko
{
    public class PuzzleRoom1Controller : MonoBehaviour
    {
        readonly Color COL_DEF = new Color(1f, 1f, 1f);
        readonly Color COL_ON = new Color(1f, 1f, 0.75f);
        readonly Color COL_SEL = new Color(1f, 0.8f, 0.8f);

        public UnityEvent SolvedA;
        public UnityEvent SolvedB;

        [Tooltip("All the buttons, from the first stone to the last stone, " +
                 "from left to right and top to bottom.")]
        public SpriteRenderer[] buttons;
        [Tooltip("The number of lines in each stone.")]
        public int lines;
        [Tooltip("The number of columns in each stone.")]
        public int columns;
        [Tooltip("The number of stones.")]
        public int stones;

        [System.Serializable]
        public struct Point3
        {
            public int x, y, z;
            public Point3(int px, int py, int pz)
            {
                x = px;
                y = py;
                z = pz;
            }
        }
        public Point3[] SolutionA;
        public Point3[] SolutionB;

        int stone, i, j;
        int elemXstone;

        Color prevColor;

        HashSet<Point3> playerSolution;

        // Use this for initialization
        void Start()
        {
            elemXstone = lines * columns;
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Grid.helper.ChangeScene("Room1_1", "init");
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Set the current button to its previous color
                buttons[stone * elemXstone + j * columns + i].color = prevColor;

                // Hover over the next button
                i = (i - 1) < 0 ? columns - 1 : i - 1;
                prevColor = buttons[stone * elemXstone + j * columns + i].color;
                buttons[stone * elemXstone + j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Set the current button to its previous color
                buttons[stone * elemXstone + j * columns + i].color = prevColor;

                // Hover over the next button
                i = (i + 1) % columns;
                prevColor = buttons[stone * elemXstone + j * columns + i].color;
                buttons[stone * elemXstone + j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Set the current button to its previous color
                buttons[stone * elemXstone + j * columns + i].color = prevColor;

                // Hover over the next button
                j = (j - 1) < 0 ? lines - 1 : j - 1;
                prevColor = buttons[stone * elemXstone + j * columns + i].color;
                buttons[stone * elemXstone + j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Set the current button to its previous color
                buttons[stone * elemXstone + j * columns + i].color = prevColor;

                // Hover over the next button
                j = (j + 1) % columns;
                prevColor = buttons[stone * elemXstone + j * columns + i].color;
                buttons[stone * elemXstone + j * columns + i].color = COL_ON;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Select and save the current button
                prevColor = COL_SEL; // This way keeps in ON, but when we move will be SEL
                playerSolution.Add(new Point3(stone, i, j));
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                // Recover previous color from current button
                buttons[stone * elemXstone + j * columns + i].color = prevColor;

                if (++stone >= stones)
                {
                    // Check win conditions
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
                        Debug.Log("enter");
                        PlayerPrefs.SetString("room1", "true");
                        Grid.helper.ChangeScene("Room1_2", "init");
                    }
                    else
                    {
                        Reset();
                    }
                }
                else
                {
                    // Set first button of new stone
                    i = 0; j = 0;
                    prevColor = buttons[stone * elemXstone + j * columns + i].color;
                    buttons[stone * elemXstone + j * columns + i].color = COL_ON;
                }
            }
        }

        private void Reset()
        {
            // Set all the buttons to its default color
            foreach (var button in buttons)
            {
                button.color = COL_DEF;
            }

            // Set first button
            stone = 0; i = 0; j = 0;
            prevColor = COL_DEF;
            buttons[0].color = COL_ON;

            // Reset player solution
            playerSolution = new HashSet<Point3>();
        }
    }
}
