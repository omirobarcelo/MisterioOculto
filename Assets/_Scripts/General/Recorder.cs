﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shoguneko
{
    public class Recorder : MonoBehaviour
    {
        public class Record
        {
            public int TimesInteracted;
            public int TimesAgreed;
            public int TimesDisagreed;
            public int TimesJoined;
            public int TimesSent;

            public Record()
            {
                TimesInteracted = 0;
                TimesAgreed = 0;
                TimesDisagreed = 0;
                TimesJoined = 0;
                TimesSent = 0;
            }

            public string ToCSVString()
            {
                string[] arr = { TimesInteracted.ToString(), 
                    TimesAgreed.ToString(), TimesDisagreed.ToString(), 
                    TimesJoined.ToString(), TimesSent.ToString() };
                return string.Join(",", arr);
            }
        }

        public class GeneralRecord
        {
            public List<int> interactions;

            public GeneralRecord()
            {
                interactions = new List<int>();
            }

            public string ToListString()
            {
                string[] record = new string[interactions.Count];
                for (int i = 0; i < interactions.Count; i++)
                {
                    record[i] = interactions[i].ToString();
                }
                return string.Join(",", record);
            }
        }

        Dictionary<string, Record> dicRecords;
        GeneralRecord generalRecord;

        private void Awake()
        {
            generalRecord = new GeneralRecord();
            //Debug.Log("Awake done");
        }

        // Use this for initialization
        void Start()
        {
            dicRecords = new Dictionary<string, Record>();
            dicRecords.Add("min", new Record());
            dicRecords.Add("enfys", new Record());
            dicRecords.Add("trace", new Record());
            dicRecords.Add("golzar", new Record());

            //generalRecord = new GeneralRecord();
            //Debug.Log("Start done");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Interacted(string npc)
        {
            dicRecords[npc].TimesInteracted++;
        }

        public void Agreed(string npc)
        {
            dicRecords[npc].TimesAgreed++;
        }

        public void Disagreed(string npc)
        {
            dicRecords[npc].TimesDisagreed++;
        }

        public void Joined(string npc)
        {
            dicRecords[npc].TimesJoined++;
        }

        public void Sent(string npc)
        {
            dicRecords[npc].TimesSent++;
        }


        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AddInteraction(); // Add 0 to symbolize a new scene
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void AddInteraction(string npc = "")
        {
            int npcCode = -1;
            switch (npc)
            {
                case "min":
                    npcCode = 1;
                    break;
                case "enfys":
                    npcCode = 2;
                    break;
                case "trace":
                    npcCode = 3;
                    break;
                case "golzar":
                    npcCode = 4;
                    break;
                case "": // Scene change
                    npcCode = 0;
                    break;
            }

            //Debug.Log(generalRecord);
            generalRecord.interactions.Add(npcCode);
        }


        private void OnApplicationQuit()
        {
            string path = Application.dataPath + "/records.txt";

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("npc,interacted,agreed,disagreed,joined,sent");

                foreach (var kvp in dicRecords)
                {
                    writer.WriteLine(kvp.Key + "," + kvp.Value.ToCSVString());
                }

                writer.WriteLine("\n" + generalRecord.ToListString());
            }
        }
    }
}
