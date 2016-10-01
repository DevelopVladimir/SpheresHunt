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
        

        public SpheresGenerator(Vector3 _sceneSizes, float _sphereSize, int _spBufferSize)
        {
            sphereSize = _sphereSize;
            SpheresHolder = new GameObject();
            SpheresHolder.name = "SpheresHolder";
            spheresHolderTrans = SpheresHolder.GetComponent<Transform>();
            spheresHolderTrans.position = new Vector3(-_sceneSizes.x / 2 + _sphereSize, _sceneSizes.y, _sphereSize);
            sphereSpawnGenSize = new Vector3(_sceneSizes.x - 2 * _sphereSize, _sceneSizes.y, _sceneSizes.z - 2 * _sphereSize);

            SpheresPool = new GameObject();
            SpheresPool.name = "SpheresPool";
            spheresPoolTrans = SpheresPool.GetComponent<Transform>();

            gc.isTexGradientHorizontal = ((gc.level + 1) % 2 == 0) ? true : false;
            for (int i = 0; i < gc.textures[0].Length; i++)
            {
                int power = (int)Mathf.Pow(2, i / 4 + 5);
                gc.textures[0][i] = GenTexture(power, gc.isTexGradientHorizontal);
            }

            spPool = new Stack<SphereController>(gc.spBufferSize);

            for (int i = 0; i < gc.spBufferSize; i++)
                PushSphereToPool(GenNewSphere(i));
        }

        private SphereController GenNewSphere(int i)
        {           
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Farik" + i;
            Transform trans = sphere.transform;
            trans.SetParent(spheresPoolTrans, false);
            trans.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
            SphereController spCtrl = sphere.AddComponent<SphereController>();
            spCtrl.DeactivateSphere();
            return spCtrl;
        }

        public void PushSphereToPool(SphereController spCtrl)
        {
            spCtrl.DeactivateSphere();
            spCtrl.transform.SetParent(spheresPoolTrans, false);
            spPool.Push(spCtrl);
        }

        public void GetNextSphere()
        {
            SphereController spCtrl;
            if (spPool.Count > 0)
                spCtrl = spPool.Pop();
            else
            {
                spCtrl = GenNewSphere(gc.spBufferSize);
                gc.spBufferSize++;
            }

            Transform spTrans = spCtrl.transform;
            spTrans.SetParent(spheresHolderTrans, false);
            spTrans.localPosition = new Vector3(Random.Range(0.0f, sphereSpawnGenSize.x), 0, Random.Range(0.0f, sphereSpawnGenSize.z));

            float normalizedDepth = spTrans.localPosition.z / sphereSpawnGenSize.z;
            spCtrl.speed = gc.baseSpeed + 6 * normalizedDepth;

            int textureNum;
            if (normalizedDepth > 0.75) textureNum = Random.Range(0, 4);
            else if (normalizedDepth > 0.5) textureNum = Random.Range(4, 8);
            else if (normalizedDepth > 0.25) textureNum = Random.Range(8, 12);
            else textureNum = Random.Range(12, 16);

            spCtrl.GetComponent<MeshRenderer>().material.mainTexture = gc.textures[(gc.level + 1) % 2][textureNum];

            spCtrl.GetComponent<Renderer>().enabled = true;
            spCtrl.enabled = true;
            spCtrl.GetComponent<Collider>().enabled = true;
        }

        public Texture2D GenTexture(int size, bool horizont)
        {
            Texture2D myTex;
            myTex = new Texture2D(size, size);
            Vector3 v = Random.onUnitSphere;
            Color col = new Color(v.x, v.y, v.z);

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    myTex.SetPixel(i, j, Color.Lerp(col, (horizont) ? Color.white : Color.red, ((horizont) ? (float)i / size : (float)j / size)));
                }

            myTex.Apply();
            return myTex;
        }
    }
}

