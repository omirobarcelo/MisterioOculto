using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class CutsceneJoin : CutsceneGeneral
    {
        public GameObject options;

        bool interacted;

        // Use this for initialization
        void Start()
        {
            // Change characters facing position
            dManagers["mc"].TurnLeft();
            dManagers["min"].TurnLeft();
            dManagers["enfys"].TurnLeft();
            dManagers["trace"].TurnLeft();
            dManagers["golzar"].TurnLeft();

            PromMoveX(dManagers["mc"], -2);
            PromMoveX(dManagers["min"], -2);
            PromMoveX(dManagers["enfys"], -2);
            PromMoveX(dManagers["trace"], -2);
            PromMoveX(dManagers["golzar"], -2);
            
            cam.gameObject.transform.DOMoveX(3, FADE_SEC).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                WaitFor(0.5f)
                    .Then(() => dManagers["min"].TurnRight())
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
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
                        PlayerPrefs.DeleteKey("join");
                        Grid.helper.ChangeScene("Hall1", "hall_kitchen_01");
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
