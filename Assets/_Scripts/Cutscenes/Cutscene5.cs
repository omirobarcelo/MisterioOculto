using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class Cutscene5 : CutsceneGeneral
    {
        public GameObject hole;

        // Use this for initialization
        void Start()
        {
            hole.SetActive(false);

            // Save MC's transform
            Transform mc_trans = dManagers["mc"].GetComponentInChildren<SpriteRenderer>().gameObject.transform;

            // Change characters facing position
            dManagers["mc"].TurnUp();
            dManagers["min"].TurnUp();
            dManagers["enfys"].TurnUp();
            dManagers["trace"].TurnUp();
            dManagers["golzar"].TurnUp();

            PromMoveY(dManagers["mc"], 2);
            PromMoveY(dManagers["min"], 2);
            PromMoveY(dManagers["enfys"], 2);
            PromMoveY(dManagers["trace"], 2);
            PromMoveY(dManagers["golzar"], 2);

            cam.gameObject.transform.DOMoveY(3, FADE_SEC).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                ShakeCamera(2)
                    .Then(() => hole.SetActive(true))
                    .Then(() => mc_trans.DOScale(new Vector3(0.75f, 0.75f, 1f), 0.5f))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => { 
                        PromMoveY(dManagers["min"], -1.5f, speed: 4);
                        PromMoveY(dManagers["enfys"], -1.5f, speed: 4); 
                        dManagers["trace"].TurnDown();
                        dManagers["golzar"].TurnDown();
                                })
                    .Then(() => WaitFor(1))
                    .Then(() => PromMoveY(dManagers["mc"], 0.25f))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => PromMoveY(dManagers["mc"], -0.25f, backwards: true))
                    .Then(() => {
                        mc_trans.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
                        PromMoveY(dManagers["mc"], 1f);
                        PromMoveY(dManagers["min"], 1f, backwards: true);
                        PromMoveY(dManagers["enfys"], 1f, backwards: true);
                                })
                    .Then(() => WaitFor(0.5f))
                    .Then(() => {
                        dManagers["trace"].TurnLeft();
                        dManagers["golzar"].TurnRight();
                                })
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "mc"]))
                    .Then(() => FadeIn(FADE_SEC))
                    .Done(() =>
                    {
                        Debug.Log("Finished");
                        //Grid.helper.ChangeScene("Outside3", "init");
                    });

            });
        }
    }
}
