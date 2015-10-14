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
		sphere.name = "Farik";
		Transform trans = sphere.transform;
		trans.SetParent (spheresHolderTrans, false);
		trans.localScale = new Vector3 (sphereSize, sphereSize, sphereSize);
		sphere.AddComponent<SphereController>();
		ResetSphere (sphere);
		return sphere;
	}
	public void ResetSphere(GameObject sphere){
		Renderer rend;
		MeshRenderer meshRen = sphere.GetComponent<MeshRenderer> ();
		meshRen.enabled = false;
		//sphere.SetActive (false);
		rend = sphere.GetComponent<Renderer>();
		Vector3 v = Random.onUnitSphere;
		Color col = new Color (v.x,v.y,v.z);
		rend.material.color = col;
		Transform trans = sphere.transform;
		Vector3 startPos = new Vector3(Random.Range (0,spheresGenSizes.x),0,Random.Range (0,spheresGenSizes.z));
		trans.localPosition = startPos;
		SphereController sCtrl = sphere.GetComponent<SphereController> ();
		sCtrl.enabled = false;
	}

	public void Push(int i){
		//spBuffer [i].SetActive(true);
		spBuffer [i].GetComponent<MeshRenderer>().enabled = true;;
		spBuffer[i].GetComponent<SphereController>().enabled = true;
	}



}
