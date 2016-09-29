using UnityEngine;
using System.Collections;

namespace SpheresHunt
{

    public class SpheresGenerator
    {

        public GameObject SpheresHolder { get; private set; }
        Transform spheresHolderTrans;
        public GameObject[] spBuffer;
        public readonly Vector3 spheresStartPos;
        public readonly Vector3 spheresGenSizes;
        float sphereSize;
        //GameController gc = GameObject.Find("Management").GetComponent<GameController>();
        GameController gc = GameController.Instance;

        public SpheresGenerator(Vector3 _spheresStartPos, Vector3 _spheresGenSizes, float _sphereSize, int _spBufferSize)
        {

            spheresGenSizes = _spheresGenSizes;
            spheresStartPos = _spheresStartPos;
            sphereSize = _sphereSize;
            spBuffer = new GameObject[_spBufferSize];

            SpheresHolder = new GameObject();
            SpheresHolder.name = "Spheres";
            spheresHolderTrans = SpheresHolder.GetComponent<Transform>();
            spheresHolderTrans.position = spheresStartPos;

            gc.isTexGradientHorizontal = (gc.level % 2 == 0) ? true : false;
            for (int i = 0; i < gc.textures[0].Length; i++)
            {
                int power = (int)Mathf.Pow(2, i / 4 + 5);
                gc.textures[0][i] = GenTexture(power, gc.isTexGradientHorizontal);
            }

            for (int i = 0; i < spBuffer.Length; i++)
                spBuffer[i] = GenNewSphere(i);
        }

        private GameObject GenNewSphere(int i)
        {

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Farik" + i;
            Transform trans = sphere.transform;
            trans.SetParent(spheresHolderTrans, false);
            trans.localScale = new Vector3(sphereSize, sphereSize, sphereSize);
            sphere.AddComponent<SphereController>();
            ResetSphere(sphere);
            return sphere;
        }

        public void ResetSphere(GameObject sphere)
        {

            Renderer rend;
            MeshRenderer meshRen = sphere.GetComponent<MeshRenderer>();
            meshRen.enabled = false;
            rend = sphere.GetComponent<Renderer>();
            Transform trans = sphere.transform;
            Vector3 startPos = new Vector3(Random.Range(0.0f, spheresGenSizes.x), 0, Random.Range(0.0f, spheresGenSizes.z));
            trans.localPosition = startPos;
            SphereController sCtrl = sphere.GetComponent<SphereController>();
            sCtrl.enabled = false;
            float normalDepth = trans.localPosition.z / spheresGenSizes.z;

            if (normalDepth > 0.75) rend.material.mainTexture = gc.textures[gc.level % 2][Random.Range(0, 3)];
            else if (normalDepth > 0.5) rend.material.mainTexture = gc.textures[gc.level % 2][Random.Range(4, 7)];
            else if (normalDepth > 0.25) rend.material.mainTexture = gc.textures[gc.level % 2][Random.Range(8, 11)];
            else rend.material.mainTexture = gc.textures[gc.level % 2][Random.Range(12, 15)];
        }

        public void Push(int i)
        {

            spBuffer[i].GetComponent<MeshRenderer>().enabled = true;
            spBuffer[i].GetComponent<SphereController>().enabled = true;
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

