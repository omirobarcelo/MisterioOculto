using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class CutsceneEnd : CutsceneGeneral
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
            PromMoveY(dManagers["min"], 2);
            PromMoveY(dManagers["enfys"], 2);
            PromMoveY(dManagers["trace"], 2);
            PromMoveY(dManagers["golzar"], 2);
            
            cam.gameObject.transform.DOMoveY(-3, FADE_SEC).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                WaitFor(0.5f)
                //Show image 1 and wait
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    // Show Image 2 and wait, maybe play sound
                    .Done(() =>
                    {
                        //Debug.Log("Finished");
                        Grid.helper.ChangeScene("Menu");
                    });

            });
        }
    }
}
