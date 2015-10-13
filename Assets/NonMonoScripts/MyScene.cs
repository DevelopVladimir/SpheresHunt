using UnityEngine;
using System.Collections;

public class MyScene {

	public GameObject walls;
	public GameObject leftWall;
	public GameObject rightWall;
	public GameObject backWall;
	public GameObject floor;

	public MyScene (int width){
		int depth;
		int height;
		Vector3 camPosition;

		depth = width*4/5;
		height = depth*2;
		
		camPosition = new Vector3(0,width*2/5, -width*19.5f/40);
		Camera.main.transform.position = camPosition;
		
		walls = new GameObject();
		walls.name = "Walls";
		Transform wallsTrans = walls.GetComponent<Transform>();
		walls.isStatic = true;
		
		leftWall = CreateWall ("LeftWall", wallsTrans, new Vector3(- width / 2, height / 2, depth / 2), new Vector3 (0, 90, 0), height, depth );
		rightWall = CreateWall ("RightWall", wallsTrans, new Vector3(width / 2, height / 2, depth / 2), new Vector3 (0, 270, 0), height, depth );
		backWall = CreateWall ("BackWall", wallsTrans, new Vector3(0, height / 2, depth), new Vector3 (0, 180, 0), height, width );
		floor = CreateWall ("Floor", wallsTrans, new Vector3(0, 0, depth/2), new Vector3 (270, 0, 0), depth, width );

	}


	GameObject CreateWall(string name, Transform parentObjTrans, Vector3 localPos, Vector3 localRot, float wHeight, float wWidth){
		Transform trans;
		float thickness = 0.1f;
		GameObject wall = GameObject.CreatePrimitive (PrimitiveType.Cube);
		Object.Destroy (wall.GetComponent<BoxCollider>());
		MeshRenderer meshRen = wall.GetComponent<MeshRenderer> ();
		meshRen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		meshRen.receiveShadows = false;
		wall.name = name;
		trans = wall.transform;
		trans.SetParent (parentObjTrans, false);
		trans.localPosition = localPos;
		trans.localEulerAngles = localRot;
		trans.localScale = new Vector3 (wWidth, wHeight, thickness);
		wall.isStatic = true;
		
		return wall;
	}	
}
