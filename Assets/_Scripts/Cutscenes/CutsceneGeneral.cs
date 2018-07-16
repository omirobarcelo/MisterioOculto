using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class CutsceneGeneral : MonoBehaviour
    {
        
        protected  float FADE_SEC = 2f;

        [Tooltip("Camera that will shake.")]
        public Camera shake;
        [Tooltip("Black image used for fading.")]
        public Image fade;

        [System.Serializable]
        public struct NamedManager
        {
            public string name;
            public CSCManager manager;
        }
        public NamedManager[] managers;
        protected Dictionary<string, CSCManager> dManagers;
        protected bool SomeoneIsTalking = false;
        protected string CharaTalking = "";

        protected PromiseTimer promiseTimer = new PromiseTimer();

        private void Awake()
        {
            //Debug.Log("General Awake");

            dManagers = new Dictionary<string, CSCManager>();
            foreach (NamedManager elem in managers)
            {
                dManagers.Add(elem.name, elem.manager);
            }
        }

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        protected void Update()
        {
            //Debug.Log("General Update");

            promiseTimer.Update(Time.deltaTime);

            if (SomeoneIsTalking)
            {
                if (Input.GetKeyDown(Grid.setup.GetInteractionKey()))
                {
                    dManagers[CharaTalking].Speak();
                }
                return;
            }
        }

        protected IEnumerator WaitAndSetBool(float seconds, System.Action<bool> act, bool value)
        {
            yield return new WaitForSeconds(seconds);
            act(value);
        }

        protected IEnumerator MoveY(Action resolve, CSCManager chara, float dist, bool followUp = false, float speed = 1f)
        {
            Debug.Log("here MoveY");

            // Change chara sprite/animation
            // If dist > 0, chara faces up
            // Else faces down
            if (dist > 0)
            {
                chara.TurnUp();
            }
            else
            {
                chara.TurnDown();
            }
            chara.CharacterAnimator.Play("Walking");

            float elapsedDist = 0f;
            Vector3 pos = chara.GetCharaTransform().position;
            while (elapsedDist < Mathf.Abs(dist))
            {
                elapsedDist += Time.deltaTime * chara.AlterSpeed * speed;
                //Debug.Log("elapsedDist: " + elapsedDist);

                //Debug.Log("pos: " + pos);

                //Debug.Log("pos: " + pos);
                chara.GetCharaTransform().position = pos + new Vector3(0, Mathf.Sign(dist) * elapsedDist, 0);
                yield return null;
            }

            if (!followUp)
            {
                // Stop character when finished
                chara.CharacterAnimator.Play("Idle");
            }

            resolve();
        }

        protected IEnumerator MoveX(Action resolve, CSCManager chara, float dist, bool followUp = false, float speed = 1f)
        {
            Debug.Log("here MoveX");

            // Change chara sprite/animation
            // If dist > 0, chara faces right
            // Else faces left
            if (dist > 0)
            {
                chara.TurnRight();
            }
            else
            {
                chara.TurnLeft();
            }
            chara.CharacterAnimator.Play("Walking");

            float elapsedDist = 0f;
            Vector3 pos = chara.GetCharaTransform().position;
            while (elapsedDist < Mathf.Abs(dist))
            {
                elapsedDist += Time.deltaTime * chara.AlterSpeed * speed;
                //Debug.Log("elapsedDist: " + elapsedDist);

                //Debug.Log("pos: " + pos);

                //Debug.Log("pos: " + pos);
                chara.GetCharaTransform().position = pos + new Vector3(Mathf.Sign(dist) * elapsedDist, 0, 0);
                yield return null;
            }

            if (!followUp)
            {
                // Stop character when finished
                chara.CharacterAnimator.Play("Idle");
            }

            resolve();
        }



        protected IPromise PromMoveY(CSCManager chara, float dist, bool followUp = false, float speed = 1f)
        {
            return new Promise((resolve, reject) => StartCoroutine(MoveY(resolve, chara, dist, followUp, speed)));
        }

        protected IPromise PromMoveX(CSCManager chara, float dist, bool followUp = false, float speed = 1f)
        {
            return new Promise((resolve, reject) => StartCoroutine(MoveX(resolve, chara, dist, followUp, speed)));
        }

        protected IPromise WaitFor(float timeToWait)
        {
            return promiseTimer.WaitFor(timeToWait);
        }

        protected IPromise WaitWhileCharaSpeaks(CSCManager chara)
        {
            chara.Speak();
            return promiseTimer.WaitUntil(t => !(SomeoneIsTalking = chara.Talking()));
        }

        protected IPromise ShakeCamera()
        {
            bool completed = false;
            shake.DOShakePosition(3).OnComplete(() => completed = true);
            return promiseTimer.WaitUntil(t => completed);
        }
    }
}
