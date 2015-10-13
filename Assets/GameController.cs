using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	int width = 40;

	void Awake () {
		MyScene sc = new MyScene (width);
		SpheresGenerator sg = new SpheresGenerator(new Vector3 (-15,sc.height,0),new Vector3 (30,-sc.height,30 ));

	}
}
