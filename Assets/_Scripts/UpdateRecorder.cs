using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public enum Operation { Interacted, Agreed, Disagreed, Joined, Sent };

    public class UpdateRecorder : MonoBehaviour
    {
        [System.Serializable]
        public struct NPCRecordOperation
        {
            public string npc;
            public Operation operation;
        }
        public NPCRecordOperation[] Operations;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Increment()
        {
            foreach (var operation in Operations)
            {
                Increment(operation);
            }
        }

        public void Increment(NPCRecordOperation operation)
        {
            switch (operation.operation)
            {
                case Operation.Interacted:
                    Grid.recorder.Interacted(operation.npc);
                    break;
                case Operation.Agreed:
                    Grid.recorder.Agreed(operation.npc);
                    break;
                case Operation.Disagreed:
                    Grid.recorder.Disagreed(operation.npc);
                    break;
                case Operation.Joined:
                    Grid.recorder.Joined(operation.npc);
                    break;
                case Operation.Sent:
                    Grid.recorder.Sent(operation.npc);
                    break;
                default:
                    break;
            }
        }
    }
}
