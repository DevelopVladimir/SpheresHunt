using UnityEngine;
using System.Collections;

public class SphereController : MonoBehaviour {
	Transform trans;
	public float speed;
	GameController gameController;
	// Use this for initialization
	void OnEnable () {
		trans = gameObject.GetComponent<Transform> ();
		gameController = GameObject.Find ("Management").GetComponent<GameController> ();
		//speed = gameController.baseSpeed;
		speed = gameController.baseSpeed *(1 + 6*trans.localPosition.z/gameController.spehereGenSize);
		//speed = 10;
	}
	
	// Update is called once per frame
	void Update () {
		//speed = gameController.baseSpeed *(1 + trans.localPosition.z/gameController.spehereGenSize);
		trans.position -= Vector3.up * speed * Time.deltaTime;
	}
}
