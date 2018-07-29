using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Shoguneko
{
    public class PuzzleRubbleController : MonoBehaviour
    {
        readonly Color COL_DEF = new Color(1f, 1f, 1f);
        readonly Color COL_ON = new Color(1f, 1f, 0.75f);

        public UnityEvent SolvedA;
        public UnityEvent SolvedB;

        [Tooltip("Panels, from left to right.")]
        public Image[] panels;

        public string SolutionA;
        public string SolutionB;

        int index;

        Text currentText;
        int currentNumber;

        string playerSolution;

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
                Grid.helper.ChangeScene("RubbleRoom1", "init");
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Update current number and the text
                currentNumber = (currentNumber + 1) % 10;
                currentText.text = currentNumber.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Update current number and the text
                currentNumber = (currentNumber - 1) < 0 ? 10 - 1 : currentNumber - 1;
                currentText.text = currentNumber.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                // Set current panel to its default color
                panels[index].color = COL_DEF;

                // Save the player's choice
                playerSolution += currentNumber.ToString();

                // If the player has chosen for all the panels, 
                // check if the solution is correct
                if (++index >= panels.Length)
                {
                    //Debug.Log("playerSolution: " + playerSolution);
                    //Debug.Log("SolutionA: " + SolutionA);
                    //Debug.Log("SolutionB: " + SolutionB);
                    if (playerSolution.Equals(SolutionA) || playerSolution.Equals(SolutionB))
                    {
                        if (playerSolution.Equals(SolutionA))
                        {
                            Debug.Log("A");
                            SolvedA.Invoke();
                        }
                        if (playerSolution.Equals(SolutionB))
                        {
                            Debug.Log("B");
                            SolvedB.Invoke();
                        }
                        PlayerPrefs.SetString("rubble", "true");
                        Grid.helper.ChangeScene("RubbleRoom2", "init");
                    }
                    else
                    {
                        Reset();
                    }
                }
                else
                {
                    // Advance to the next panel
                    panels[index].color = COL_ON;
                    currentText = panels[index].GetComponentInChildren<Text>();
                    // TODO safe type it
                    currentNumber = int.Parse(currentText.text);
                }
            }
        }

        private void Reset()
        {
            // Set all the texts to 0 and to its default color
            foreach (var panel in panels)
            {
                panel.GetComponentInChildren<Text>().text = "0";
                panel.color = COL_DEF;
            }

            // Save first text and first number
            currentText = panels[index = 0].GetComponentInChildren<Text>();
            currentNumber = 0;

            // Set the first panel to its on color
            panels[0].color = COL_ON;

            // Reset player's solution
            playerSolution = "";
        }
    }
}
