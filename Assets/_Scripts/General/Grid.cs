using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    static class Grid
    {

        public static HelperManager helper;
        public static SoundManager soundManager;
        public static ItemDatabase itemDataBase;
        public static Inventory inventory;
        public static OptionsManager optionsManager;
        public static Setup setup;
        public static Recorder recorder;


        static Grid()
        {
            GameObject g;
            g = SafeFind("_Holder");
            helper = (HelperManager)SafeComponent(g, "HelperManager");
            itemDataBase = (ItemDatabase)SafeComponent(g, "ItemDatabase");
            inventory = (Inventory)SafeComponent(g, "Inventory");
            setup = (Setup)SafeComponent(g, "Setup");
            recorder = (Recorder)SafeComponent(g, "Recorder");

            g = SafeFind("SoundManager");
            soundManager = (SoundManager)SafeComponent(g, "SoundManager");

            g = SafeFind("OptionsCanvas");
            optionsManager = (OptionsManager)SafeComponent(g, "OptionsManager");
        }

        private static GameObject SafeFind(string s)
        {
            GameObject g = GameObject.Find(s);
            if (g == null)
            {
                BigProblem("The " + s + " GameObject is not in this scene.");
            }
            return g;
        }

        private static Component SafeComponent(GameObject g, string s)
        {
            Component c = g.GetComponent(s);
            if (c == null)
            {
                BigProblem("The " + s + " Component is not attached to the " + g.name + " GameObject.");
            }
            return c;
        }

        private static void BigProblem(string error)
        {
            Debug.Log("Cannot proceed : " + error);
            Debug.Break();
        }
    }
}
