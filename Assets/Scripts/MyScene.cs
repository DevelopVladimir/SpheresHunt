using System;
using UnityEngine;
using System.Collections;

namespace SpheresHunt
{
    public class MyScene
    {
        public GameObject Walls { get; private set; }
        public GameObject LeftWall { get; private set; }
        public GameObject RightWall { get; private set; }
        public GameObject BackWall { get; private set; }
        public GameObject Floor { get; private set; }

        private float width;
        private float depth;
        private float height;

        private Material material = new Material(Shader.Find("Mobile/Diffuse"));

        public MyScene(Vector3 sceneSizes)
        {
            width = sceneSizes.x;
            height = sceneSizes.y;
            depth = sceneSizes.z;

            Walls = new GameObject();
            Walls.name = "Walls";
            Transform wallsTrans = Walls.GetComponent<Transform>();
            Walls.isStatic = true;

            LeftWall = CreateWall("LeftWall", wallsTrans, new Vector3(-width / 2, height / 2, depth / 2), new Vector3(0, 90, 0), height, depth);
            RightWall = CreateWall("RightWall", wallsTrans, new Vector3(width / 2, height / 2, depth / 2), new Vector3(0, 270, 0), height, depth);
            BackWall = CreateWall("BackWall", wallsTrans, new Vector3(0, height / 2, depth), new Vector3(0, 180, 0), height, width);
            Floor = CreateWall("Floor", wallsTrans, new Vector3(0, 0, depth / 2), new Vector3(270, 0, 0), depth, width);
        }

        GameObject CreateWall(string name, Transform parentObjTrans, Vector3 localPos, Vector3 localRot, float wHeight, float wWidth)
        {
            Transform trans;
            float thickness = 0.1f;
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(wall.GetComponent<BoxCollider>());
            MeshRenderer meshRen = wall.GetComponent<MeshRenderer>();
            meshRen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRen.receiveShadows = false;
            meshRen.material = material;
            wall.name = name;
            trans = wall.transform;
            trans.SetParent(parentObjTrans, false);
            trans.localPosition = localPos;
            trans.localEulerAngles = localRot;
            trans.localScale = new Vector3(wWidth, wHeight, thickness);
            wall.isStatic = true;

            return wall;
        }

        public void ApplyAssetBundleTexture(Texture[] tex)
        {
            Material mat = LeftWall.GetComponent<Renderer>().material;
            mat.mainTexture = tex[0];
            mat.mainTextureScale = new Vector2(2, 2);

            mat = RightWall.GetComponent<Renderer>().material;
            mat.mainTexture = tex[0];
            mat.mainTextureScale = new Vector2(2, 2);

            mat = BackWall.GetComponent<Renderer>().material;
            mat.mainTexture = tex[0];
            mat.mainTextureScale = new Vector2(2, 2);

            Floor.GetComponent<Renderer>().material.mainTexture = tex[1];
        }
    }
}

