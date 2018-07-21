using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Shoguneko
{
    public class ChooserController : MonoBehaviour
    {
        public bool Sent;

        Image[] panels;
        int selected;

        // Use this for initialization
        void Start()
        {
            panels = GetComponentsInChildren<Image>();
            // Remove first Image (image of the holder panel)
            List<Image> tmp = new List<Image>(panels);
            panels = tmp.GetRange(1, 4).ToArray();
            selected = 0;

            SelectPanel();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UnselectPanel();
                selected = (selected + 1) % (panels.Length);
                SelectPanel();

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UnselectPanel();
                selected = (selected - 1) < 0 ? panels.Length - 1 : selected - 1;
                SelectPanel();
            }
            else if (Input.GetKeyDown(Grid.setup.GetInteractionKey()))
            {
                string npc = selected == 0 ? "min" : (selected == 1 ? "enfys"
                                                      : (selected == 2 ? "trace" : "golzar"));
                if (Sent)
                {
                    Grid.recorder.Sent(npc);
                }
                else 
                {
                    Grid.recorder.Joined(npc);
                }
            }
        }

        private void SelectPanel()
        {
            Color tmp = panels[selected].color;
            tmp = new Color(tmp.r, tmp.g, tmp.b, 100f / 255f);
            panels[selected].color = tmp;
        }

        private void UnselectPanel()
        {
            Color tmp = panels[selected].color;
            tmp = new Color(tmp.r, tmp.g, tmp.b, 0f / 255f);
            panels[selected].color = tmp;
        }
    }
}
