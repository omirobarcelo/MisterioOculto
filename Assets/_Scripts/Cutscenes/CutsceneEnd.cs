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
        public AudioClip voice;
        public AudioClip sound;

        public Image img1, img2, img3, img4;

        // Use this for initialization
        void Start()
        {
            // Deactivate all images
            img1.gameObject.SetActive(false);
            img2.gameObject.SetActive(false);
            img3.gameObject.SetActive(false);
            img4.gameObject.SetActive(false);

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
                WaitFor(1f)
                //Show image 1 and wait
                    .Then(() => img1.gameObject.SetActive(true))
                    .Then(() => WaitFor(4f))
                    .Then(() => img1.gameObject.SetActive(false))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "mc"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "min"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "enfys"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "golzar"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "trace"]))
                    .Then(() => WaitWhileCharaSpeaks(dManagers[CharaTalking = "mc"]))
                    //.Then(() => {
                    //    dManagers["mc"].TurnDown();
                    //    dManagers["min"].TurnDown();
                    //    dManagers["enfys"].TurnDown();
                    //    dManagers["trace"].TurnDown();
                    //    dManagers["golzar"].TurnDown();
                    //})
                    .Then(() => {
                        PromMoveY(dManagers["mc"], -2);
                        PromMoveY(dManagers["min"], -2);
                        PromMoveY(dManagers["enfys"], -2);
                        PromMoveY(dManagers["trace"], -2);
                        PromMoveY(dManagers["golzar"], -2);
                    })
                    .Then(() => WaitFor(0.5f))
                    .Then(() => Grid.soundManager.PlaySound(voice))
                    .Then(() => WaitFor(5f))
                    .Then(() => {
                        dManagers["mc"].TurnUp();
                        dManagers["min"].TurnUp();
                        dManagers["enfys"].TurnUp();
                        dManagers["trace"].TurnUp();
                        dManagers["golzar"].TurnUp();
                    })
                    .Then(() => WaitFor(0.5f))
                    .Then(() => Grid.soundManager.PlaySound(sound))
                    .Then(() => img2.gameObject.SetActive(true))
                    .Then(() => WaitFor(3f))
                    .Then(() => img2.gameObject.SetActive(false))
                    .Then(() => img3.gameObject.SetActive(true))
                    .Then(() => WaitFor(3f))
                    .Then(() => img3.gameObject.SetActive(false))
                    .Then(() => img4.gameObject.SetActive(true))
                    .Then(() => WaitFor(3f))
                    //.Then(() => img4.gameObject.SetActive(false))
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
