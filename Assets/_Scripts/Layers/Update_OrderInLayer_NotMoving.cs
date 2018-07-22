using UnityEngine;
using System.Collections;

namespace Shoguneko {

	public class Update_OrderInLayer_NotMoving : MonoBehaviour {

        public bool UseParentTransform;

		private SpriteRenderer spriteRend;
		private Transform trans;

		void Start(){
			spriteRend = GetComponent<SpriteRenderer> ();
            trans = UseParentTransform ? transform.parent : GetComponent<Transform> ();
			spriteRend.sortingOrder = (int)(trans.position.y * -1000);
            spriteRend.sortingOrder += UseParentTransform ? 1 : 0;
		}
	}
}
