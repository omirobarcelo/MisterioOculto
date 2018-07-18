using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class Cutscene4 : CutsceneGeneral
    {

        // Use this for initialization
        void Start()
        {
            // Change characters facing position
            dManagers["mc"].TurnRight();
            dManagers["min"].TurnRight();
            dManagers["enfys"].TurnUp();
            dManagers["trace"].TurnRight();
            dManagers["golzar"].TurnRight();

            cam.gameObject.transform.DOMoveX(3, 2).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                WaitFor(1)
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => dManagers["min"].TurnLeft())
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => {
                        dManagers["mc"].TurnDown();
                        dManagers["min"].TurnDown();
                        dManagers["enfys"].TurnDown();
                        dManagers["trace"].TurnDown();
                        dManagers["golzar"].TurnDown();
                    })
                    .Then(() => MoveCameraY(-16, 3))
                    .Then(() => MoveCameraX(-6, 2f))
                    .Then(() => WaitFor(1))
                    .Then(() => MoveCameraX(6, 0.25f))
                    .Then(() => MoveCameraY(16, 0.5f))
                    .Then(() => {
                        dManagers["mc"].TurnRight();
                        dManagers["min"].TurnLeft();
                        dManagers["enfys"].TurnLeft();
                        dManagers["trace"].TurnLeft();
                        dManagers["golzar"].TurnLeft();
                    })
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => MoveCameraX(-2, 0.5f))
                    //.Then(() => FadeIn(FADE_SEC))
                    .Done(() =>
                    {
                        Debug.Log("Finished");
                        Grid.helper.ChangeScene("Outside3", "init");
                    });

            });
        }
    }
}
