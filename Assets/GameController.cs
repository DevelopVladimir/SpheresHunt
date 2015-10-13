using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public readonly int width = 40;
	public int height;
	public readonly int sphereSize = 3;
	int spBufferSize = 10;
	MyScene sc;
	public SpheresGenerator sg;
	float period = 0.5f;
	public int spehereGenSize = 30;
	public float baseSpeed;
	Camera cam;
	float timer;
	public int score;

	void Awake () {
		sc = new MyScene (width);
		sg = new SpheresGenerator(new Vector3 (-15,sc.height,0),new Vector3 (spehereGenSize, sc.height,spehereGenSize ), sphereSize, spBufferSize);
		baseSpeed = 5;
		height = sc.height;

		Vector3 camPosition = new Vector3(0,width*2/5, -width*19.5f/40);
		cam = Camera.main;
		cam.transform.position = camPosition;
		timer = 0;
		score = 0;
	}

	void Start(){
		StartCoroutine ("PushSpheres");
	}

	void Update(){
		Fire ();
		timer += Time.deltaTime;
	}

	void Fire (){
		RaycastHit hit;
		GameObject sphere;
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				sphere = hit.collider.gameObject;
				score += Mathf.RoundToInt(sphere.transform.localPosition.z);
				sg.ResetSphere(sphere);
			}
		}
	}


	IEnumerator PushSpheres(){
		while(true){
			for (int i =0;  i < spBufferSize; i++){
				sg.Push(i);
				yield return new WaitForSeconds(period);
			}
		}
	}

}
