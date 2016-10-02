using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SpheresHunt
{
    public class SpheresGenerator
    {
        public GameObject SpheresHolder;   
        Transform spheresHolderTrans;
        public GameObject SpheresPool;
        Transform spheresPoolTrans;
        public readonly Vector3 sphereSpawnGenSize;
        float sphereSize;
        GameController gc = GameController.Instance;
        Stack<SphereController> spPool;
        public Texture[,] textures;
        int texBufferSize;

        public SpheresGenerator(Vector3 _sceneSizes, float _sphereSize, int _spBufferSize, int _texBufferSize)
        {
            sphereSize = _sphereSize;
            texBufferSize = _texBufferSize;
            SpheresHolder = new GameObject();
            SpheresHolder.name = "SpheresHolder";
            spheresHolderTrans = SpheresHolder.GetComponent<Transform>();
            spheresHolderTrans.position = new Vector3(-_sceneSizes.x / 2 + _sphereSize, _sceneSizes.y, _sphereSize);
            sphereSpawnGenSize = new Vector3(_sceneSizes.x - 2 * _sphereSize, _sceneSizes.y, _sceneSizes.z - 2 * _sphereSize);

            SpheresPool = new GameObject();
            SpheresPool.name = "SpheresPool";
            spheresPoolTrans = SpheresPool.GetComponent<Transform>();

            textures = new Texture[2, texBufferSize];
            for (int i = 0; i < texBufferSize; i++)
                GenSphereTexForNextLevel(i);

            spPool = new Stack<SphereController>(gc.spBufferSize);
            for (int i = 0; i < gc.spBufferSize; i++)
                GenNewSphereToPool(i);
        }

        private void GenNewSphereToPool(int i)
        {           
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            SphereController spCtrl = sphere.AddComponent<SphereController>();
            sphere.name = "Farik" + i;
            spCtrl.trans.localScale = new Vector3(sphereSize, sphereSize, sphereSize);          
            PushSphereToPool(spCtrl);
        }

        public void PushSphereToPool(SphereController spCtrl)
        {
            spCtrl.col.enabled = false;
            spCtrl.rend.enabled = false;
            spCtrl.enabled = false;
            spCtrl.trans.localPosition = Vector3.zero;
            spCtrl.trans.SetParent(spheresPoolTrans, false);
            spPool.Push(spCtrl);
        }

        public void GetNextSphere()
        {        
            if (0 == spPool.Count)
                GenNewSphereToPool(gc.spBufferSize++);
               
            SphereController spCtrl = spPool.Pop();
            spCtrl.trans.SetParent(spheresHolderTrans, false);
            spCtrl.trans.localPosition = new Vector3(Random.Range(0.0f, sphereSpawnGenSize.x), 0, Random.Range(0.0f, sphereSpawnGenSize.z));

            float normalizedDepth = spCtrl.trans.localPosition.z / sphereSpawnGenSize.z;
            spCtrl.speed = gc.baseSpeed + 6 * normalizedDepth;

            int textureNum;
            if (normalizedDepth > 0.75) textureNum = Random.Range(0, 4);
            else if (normalizedDepth > 0.5) textureNum = Random.Range(4, 8);
            else if (normalizedDepth > 0.25) textureNum = Random.Range(8, 12);
            else textureNum = Random.Range(12, 16);

            spCtrl.rend.material.mainTexture = textures[gc.level % 2, textureNum];

            spCtrl.rend.enabled = true;
            spCtrl.enabled = true;
            spCtrl.col.enabled = true;
        }

        public void GenSphereTexForNextLevel(int texNum)
        {
            int nextLevelTexSubmassive = (gc.level + 1) % 2;
            bool isTexGradientHorizontal = (1 == nextLevelTexSubmassive) ? true : false;
            int texSize = (int)Mathf.Pow(2, texNum / 4 + 5);

            Texture2D myTex = new Texture2D(texSize, texSize);
            Vector3 v = Random.onUnitSphere;
            Color col = new Color(v.x, v.y, v.z);
            //Color col = new Color(Random.value, Random.value, Random.value);

            for (int i = 0; i < texSize; i++)
                for (int j = 0; j < texSize; j++)
                {
                    myTex.SetPixel(i, j, Color.Lerp(col, (isTexGradientHorizontal) ? Color.white : Color.red, ((isTexGradientHorizontal) ? (float)i / texSize : (float)j / texSize)));
                }

            myTex.Apply();
            textures[nextLevelTexSubmassive, texNum] = myTex;
        }
    }
}

