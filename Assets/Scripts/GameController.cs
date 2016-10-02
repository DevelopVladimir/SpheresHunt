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
        public readonly Vector3 sceneSizes = new Vector3(50 * (4.0f / 5.0f) * (float)Screen.width / Screen.height, 60, 30);
        private Vector3 camPosition = new Vector3(0, 20, -34);
        public float sphereSize = 3;
        public int spBufferSize = 16;
        public float baseSpeed = 5;
        public float levelDuration = 30;
        public float timeScale = 1;
        private int texBufferSize = 16;

        MyScene sc;
        MyUI ui;
        public SpheresGenerator sg;
        public float sphereGenRate = 1.0f;

        Camera cam;
        float timer;
        int tempTimer;
        public int score;
        public int level = 0;

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

            sc = new MyScene(sceneSizes);
            StartCoroutine(load(loadUrl, 1));

            sg = new SpheresGenerator(sceneSizes, sphereSize, spBufferSize, texBufferSize);

            ui = new MyUI();
            cam = Camera.main;
            cam.transform.position = camPosition;

            timer = 0;
            score = 0;
            tempTimer = 0;
        }

        void Start()
        {
            StartCoroutine("ChangeLevel");
            StartCoroutine("PushSpheres");
            
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
                    sg.PushSphereToPool(sphere.GetComponent<SphereController>());
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
                sg.GetNextSphere();
                yield return new WaitForSeconds(sphereGenRate);
            }
        }

        IEnumerator ChangeLevel()
        {
            while (true)
            {
                level++;
                baseSpeed = level * 2 + 3;

                for (int i = 0; i < texBufferSize; i++)
                {
                    sg.GenSphereTexForNextLevel(i);
                    yield return new WaitForSeconds(levelDuration / texBufferSize);
                }
            }
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
