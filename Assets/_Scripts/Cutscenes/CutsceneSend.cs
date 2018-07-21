using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class CutsceneSend : CutsceneGeneral
    {
        public GameObject options;

        bool interacted;

        // Use this for initialization
        void Start()
        {
            // Change characters facing position
            dManagers["mc"].TurnDown();
            dManagers["min"].TurnRight();
            dManagers["enfys"].TurnLeft();
            dManagers["trace"].TurnLeft();
            dManagers["golzar"].TurnRight();
            
            //cam.gameObject.transform.DOMoveY(3, FADE_SEC).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                WaitFor(0.5f)
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => options.SetActive(true))
                    .Then(() => WaitForUserInput())
                    .Then(() => options.SetActive(false))
                    .Then(() => FadeIn(FADE_SEC))
                    .Then(() => WaitFor(0.5f))
                    .Then(() => FadeOut(FADE_SEC))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "mc"]))
                    .Then(() => FadeIn(FADE_SEC))
                    .Done(() =>
                    {
                        //Debug.Log("Finished");
                        //PlayerPrefs.SetString("kitchen_mid", "true");
                        Grid.helper.ChangeScene("Cave1", "kitchen_cave_01");
                    });

            });
        }

        private void Update()
        {
            base.Update();

            if (options.activeInHierarchy && Input.GetKeyDown(Grid.setup.GetInteractionKey()))
            {
                interacted = true;
            }
        }

        protected IPromise WaitForUserInput()
        {
            return promiseTimer.WaitUntil(t => interacted);
        }
    }
}
