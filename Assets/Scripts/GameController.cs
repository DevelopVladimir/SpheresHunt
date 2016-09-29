using UnityEngine;
using System.Collections;
using System;
namespace SpheresHunt
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance;
        public static GameController Instance
        {
            get
            {
                if (null == instance) Debug.Log("There are no GameController instance in the scene" + Application.loadedLevelName);
                return instance;
            }
        }
        public float sceneWidth = 50;
        //[ReadOnly]
        public Vector3 sceneProportions = new Vector3(1.0f, 1.2f, 0.8f); //Depend of sceneWidth
        public float sphereSize = 3;
        public int spBufferSize = 16;
        public float sphereSpawnGenSize = 30;
        public float baseSpeed;
        public int levelDuration = 30;
        public float timeScale = 1;
        public bool isTexGradientHorizontal;

        MyScene sc;
        MyUI ui;
        public SpheresGenerator sg;
        float period = 0.5f;
        
        
        Camera cam;
        float timer;
        int tempTimer;
        public int score;
        public int level = 0;
        
        public Texture[][] textures;
                
        
        private string loadUrl = "file://Textures.unity3d";

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (instance != this)
                {
                    Debug.Log("Duplicate of '" + this.GetType() + "' script was removed from '" + gameObject.name + "' gameobject");
                    Destroy(this);
                }
                return;
            }

            sc = new MyScene(sceneWidth * sceneProportions);
            StartCoroutine(load(loadUrl, 1));

            textures = new Texture[2][];
            textures[0] = new Texture[16];
            textures[1] = new Texture[16];

            sg = new SpheresGenerator(new Vector3(-15, sceneWidth * sceneProportions.y, 0), new Vector3(sphereSpawnGenSize, sceneWidth * sceneProportions.y, sphereSpawnGenSize), sphereSize, spBufferSize);

            ui = new MyUI();

            baseSpeed = 3;
            //height = sc.height;

            Vector3 camPosition = new Vector3(0, sceneWidth * 2 / 5, -sceneWidth * 19.5f / 40);
            cam = Camera.main;
            cam.transform.position = camPosition;

            timer = 0;
            score = 0;
            tempTimer = 0;
        }

        void Start()
        {

            StartCoroutine("PushSpheres");
            StartCoroutine("ChangeLevel");
        }

        void Update()
        {

            Fire();
            UpdateTime();
            Time.timeScale = timeScale;
        }

        void Fire()
        {

            RaycastHit hit;
            GameObject sphere;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    sphere = hit.collider.gameObject;
                    score += Mathf.RoundToInt(sphere.transform.localPosition.z);
                    ui.scoreT.text = "Score: " + score;
                    sg.ResetSphere(sphere);
                }
            }
        }

        void UpdateTime()
        {
            timer += Time.deltaTime;
            if (timer - tempTimer > 1)
            {
                tempTimer++;
                ui.AddSeconds();
            }
        }

        IEnumerator PushSpheres()
        {

            while (true)
            {
                for (int i = 0; i < spBufferSize; i++)
                {
                    sg.Push(i);
                    yield return new WaitForSeconds(period);
                }
            }
        }

        IEnumerator ChangeLevel()
        {

            while (true)
            {

                isTexGradientHorizontal = (level % 2 == 0) ? true : false;
                for (int i = 0; i < textures[(level + 1) % 2].Length; i++) textures[(level + 1) % 2][i] = null;

                for (int i = 0; i < textures[(level + 1) % 2].Length; i++)
                {
                    int power = (int)Mathf.Pow(2, i / 4 + 5);
                    textures[(level + 1) % 2][i] = sg.GenTexture(power, !isTexGradientHorizontal);
                    yield return new WaitForSeconds((float)levelDuration / 16.0f);
                }

                level++;
                baseSpeed = level + 3;
            }
        }

        public void ApplyLoadedTextures(Texture[] tex)
        {

            //sc.ApplyAssetBundleTexture(tex);
        }

        private IEnumerator load(string url, int version)
        {
            WWW www;
            Texture[] tex;
            Caching.CleanCache();
            while (!Caching.ready) yield return null;

            www = WWW.LoadFromCacheOrDownload(url, version);
            yield return www;
            if (www.error != null) throw new Exception("WWW download had an error: " + www.error);

            AssetBundle assetBundle = www.assetBundle;
            string[] assetNames = assetBundle.GetAllAssetNames();
            tex = new Texture[assetNames.Length];

            for (int i = 0; i < assetNames.Length; i++)
            {
                AssetBundleRequest request = assetBundle.LoadAssetAsync(assetNames[i], typeof(Texture));
                yield return request;
                tex[i] = request.asset as Texture;
            }

            sc.ApplyAssetBundleTexture(tex);

            www.Dispose();
            assetBundle.Unload(false);
        }
    }
}
