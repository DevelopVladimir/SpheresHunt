using UnityEngine;
using System.Collections;

public class MyScene {

	public GameObject Walls { get; private set;}
	public GameObject LeftWall{ get; private set;}
	public GameObject RightWall{ get; private set;}
	public GameObject BackWall{ get; private set;}
	public GameObject Floor{ get; private set;}
	public readonly int width;
	public readonly int depth;
	public readonly int height;

	public MyScene (int _width){
		width = _width;


		depth = width*4/5;
		height = depth*2;
		

		
		Walls = new GameObject();
		Walls.name = "Walls";
		Transform wallsTrans = Walls.GetComponent<Transform>();
		Walls.isStatic = true;
		
		LeftWall = CreateWall ("LeftWall", wallsTrans, new Vector3(- width / 2, height / 2, depth / 2), new Vector3 (0, 90, 0), height, depth );
		RightWall = CreateWall ("RightWall", wallsTrans, new Vector3(width / 2, height / 2, depth / 2), new Vector3 (0, 270, 0), height, depth );
		BackWall = CreateWall ("BackWall", wallsTrans, new Vector3(0, height / 2, depth), new Vector3 (0, 180, 0), height, width );
		Floor = CreateWall ("Floor", wallsTrans, new Vector3(0, 0, depth/2), new Vector3 (270, 0, 0), depth, width );

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
