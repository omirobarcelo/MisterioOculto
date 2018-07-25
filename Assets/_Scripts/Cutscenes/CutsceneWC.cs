﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using RSG;
using System;

namespace Shoguneko
{
    public class CutsceneWC : CutsceneGeneral
    {
        public GameObject clue;

        // Use this for initialization
        void Start()
        {
			clue.SetActive(false);

            // Change characters facing position
            dManagers["mc"].TurnUp();
            dManagers["min"].TurnLeft();
            dManagers["enfys"].TurnRight();
            dManagers["trace"].TurnUp();
            dManagers["golzar"].TurnUp();
            
            //cam.gameObject.transform.DOMoveY(3, FADE_SEC).From(isRelative: true);
            fade.DOFade(0, FADE_SEC).OnComplete(() =>
            {
                WaitFor(0.1f)
                    .Then(() => clue.SetActive(true))
                    .Then(() => WaitFor(3))
                    .Then(() => clue.SetActive(false))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => FadeIn(FADE_SEC))
                    .Done(() =>
                    {
                        //Debug.Log("Finished");
                        PlayerPrefs.SetString("wc", "true");
                        Grid.helper.ChangeScene("WC_2", "init");
                    });

            });
        }
    }
}
