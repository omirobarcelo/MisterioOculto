using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class DisablePlayer : MonoBehaviour
    {
        [Inject(InjectFrom.Anywhere)]
        public PlayerManager _player;

        // Use this for initialization
        void Start()
        {
            _player.characterEntity.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
