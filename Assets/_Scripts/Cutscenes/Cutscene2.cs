﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class Cutscene2 : CutsceneGeneral
    {

        // Use this for initialization
        void Start()
        {
            // Change characters facing position
            dManagers["mc"].TurnUp();
            dManagers["min"].TurnRight();
            dManagers["enfys"].TurnDown();
            dManagers["trace"].TurnDown();
            dManagers["golzar"].TurnLeft();

            //cam.gameObject.transform.DOMoveY(-3, 2).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                WaitFor(1)
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "mc"]))
                    .Then(() => {
                        dManagers["mc"].TurnDown();
                        dManagers["min"].TurnUp();
                        dManagers["enfys"].TurnUp();
                        dManagers["trace"].TurnUp();
                        dManagers["golzar"].TurnUp();
                                })
                    .Then(() => MoveCameraY(-3, 1))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => PromMoveY(dManagers["mc"], -3))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "mc"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => FadeIn(FADE_SEC))
                    .Done(() =>
                    {
                        Debug.Log("Finished");
                        Grid.helper.ChangeScene("Outside2", "init");
                    });

            });
        }
    }
}
