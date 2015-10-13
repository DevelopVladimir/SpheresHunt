using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	Transform trans;
	public float speed;
	GameController gameController;
	public int indInBuffer;

	void OnEnable () {
		trans = gameObject.GetComponent<Transform> ();
		gameController = GameObject.Find ("Management").GetComponent<GameController> ();
		speed = gameController.baseSpeed *(1 + 6*trans.localPosition.z/gameController.spehereGenSize);

	}

	void Update () {
		trans.Translate (Vector3.down * speed * Time.deltaTime,Space.Self);
		if (-trans.localPosition.y > gameController.height - gameController.sphereSize/2) {
			gameController.sg.ResetSphere(gameObject);
		}

	}
}
