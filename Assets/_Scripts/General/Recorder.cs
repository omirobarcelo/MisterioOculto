using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

        Dictionary<string, Record> dicRecords;

        // Use this for initialization
        void Start()
        {
            dicRecords = new Dictionary<string, Record>();
            dicRecords.Add("min", new Record());
            dicRecords.Add("enfys", new Record());
            dicRecords.Add("trace", new Record());
            dicRecords.Add("golzar", new Record());
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
            }
        }
    }
}
