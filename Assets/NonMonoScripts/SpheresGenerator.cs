using UnityEngine;
using System.Collections;

public class SpheresGenerator {
	public GameObject SpheresHolder { get; private set;}
	Transform spheresHolderTrans;
	public GameObject[] spBuffer; 
	public readonly Vector3 spheresStartPos;
	public readonly Vector3 spheresGenSizes;
	int sphereSize;

	public SpheresGenerator(Vector3 _spheresStartPos, Vector3 _spheresGenSizes, int _sphereSize, int _spBufferSize ){
		spheresGenSizes = _spheresGenSizes;
		spheresStartPos = _spheresStartPos;
		sphereSize = _sphereSize;
		spBuffer = new GameObject[_spBufferSize];
		
		SpheresHolder = new GameObject();
		SpheresHolder.name = "Spheres";
		spheresHolderTrans = SpheresHolder.GetComponent<Transform>();
		spheresHolderTrans.position = spheresStartPos;
		for (int i = 0; i < spBuffer.Length; i++)
			spBuffer [i] = GenNewSphere ();
	}

	private  GameObject GenNewSphere(){

		GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		sphere.SetActive (false);
		//MeshRenderer meshRen = sphere.GetComponent<MeshRenderer> ();
		sphere.name = "farik";
		Transform trans = sphere.transform;
		trans.SetParent (spheresHolderTrans, false);
		Vector3 startPos = new Vector3(Random.Range (0,spheresGenSizes.x),0,Random.Range (0,spheresGenSizes.z));
		trans.localPosition = startPos;
		trans.localScale = new Vector3 (sphereSize, sphereSize, sphereSize);
		sphere.AddComponent<SphereController>();
		SphereController sCtrl = sphere.GetComponent<SphereController> ();
		return sphere;
	}
	public void Push(int i){
		spBuffer [i].SetActive(true);
	}

}
