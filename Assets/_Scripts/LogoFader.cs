using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Shoguneko
{
    public class LogoFader : MonoBehaviour
    {
        private const float FADE_SEC = 3;
        private const float WAIT_SEC = 2;

        Sequence seq;
        Image img;
        bool playedOnce;

        // Use this for initialization
        void Start()
        {
            img = GetComponent<Image>();
            playedOnce = false;
            Fade();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            Grid.inventory.AddItem(3, 1);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
#endif

            if (playedOnce)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
            }
        }

        private void Fade()
        {
            DOTween.Sequence().Append(img.DOFade(0, FADE_SEC))
                   .Insert(FADE_SEC + WAIT_SEC, img.DOFade(1, FADE_SEC))
                   .OnComplete(() => { playedOnce = true; });
        }
    }
}
