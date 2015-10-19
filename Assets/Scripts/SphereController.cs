using UnityEngine;
using System.Collections;

namespace SpheresHunt{

	public class SphereController : MonoBehaviour {

		Transform trans;
		public float speed;
		GameController gc;
		float angleSpeed = 90;

		void OnEnable () {

			trans = gameObject.GetComponent<Transform> ();
			gc = GameObject.Find ("Management").GetComponent<GameController> ();
			speed = gc.baseSpeed *(1 + 6*trans.localPosition.z/gc.spehereGenSize);
		}
		
		void Update () {

			trans.Translate (Vector3.down * speed * Time.deltaTime,Space.Self);
			if (-trans.localPosition.y > gc.height - gc.sphereSize/2)
				gc.sg.ResetSphere(gameObject);
			trans.Rotate(Vector3.up * Time.deltaTime * angleSpeed);
		}
	}
}


