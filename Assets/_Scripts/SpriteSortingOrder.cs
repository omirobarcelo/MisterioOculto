using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoguneko
{
    public class SpriteSortingOrder : MonoBehaviour
    {

        private const int UNDER = 0;
        private const int OVER = 2;

        [Inject(InjectFrom.Anywhere)]
        public PlayerManager playerManager;

        private SpriteRenderer sprite;

        // Use this for initialization
        void Start()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerManager.GetPlayerY() < transform.position.y)
                sprite.sortingOrder = UNDER;
            else
                sprite.sortingOrder = OVER;
        }
    }
}
