using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	int width = 40;
	int sphereSize = 3;
	int spBufferSize = 100;
	MyScene sc;
	SpheresGenerator sg;
	float period = 0.5f;
	public int spehereGenSize = 30;
	public float baseSpeed = 10;
	void Awake () {
		sc = new MyScene (width);
		sg = new SpheresGenerator(new Vector3 (-15,sc.height,0),new Vector3 (spehereGenSize,-sc.height,spehereGenSize ), sphereSize, spBufferSize);
		baseSpeed = 5;
	}
	void Start(){
		StartCoroutine ("PushSpheres");
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
