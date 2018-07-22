﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class CutsceneCave : CutsceneGeneral
    {
        // Use this for initialization
        void Start()
        {
            // Change characters facing position
            dManagers["mc"].TurnUp();
            dManagers["min"].TurnUp();
            dManagers["enfys"].TurnUp();
            dManagers["trace"].TurnUp();
            dManagers["golzar"].TurnUp();

            PromMoveY(dManagers["mc"], 2);

            cam.gameObject.transform.DOMoveY(3, FADE_SEC).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                WaitFor(1)
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => {
                        PromMoveY(dManagers["trace"], 2);
                        WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]);
                                })
                    .Then(() => WaitFor(1))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => FadeIn(FADE_SEC))
                    .Done(() =>
                    {
                        Debug.Log("Finished");
                        PlayerPrefs.SetString("cave", "true");
                        PlayerPrefs.SetString("join", "true");
                        Grid.helper.ChangeScene("Cave1", "init");
                    });

            });
        }
    }
}
