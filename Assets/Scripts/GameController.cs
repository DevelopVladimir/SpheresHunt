using UnityEngine;
using System.Collections;
using System;
namespace SpheresHunt
{
    public class GameController : MonoBehaviour
    {
        #region Singleton
        private static GameController _instance;
        public static GameController Instance
        {
            get
            {
                if (null == _instance) Debug.Log("There are no GameController instance in the scene" + Application.loadedLevelName);
                return _instance;
            }
        }
        #endregion

        public readonly Vector3 sceneSizes = new Vector3(50 * (4.0f / 5.0f) * (float)Screen.width / Screen.height, 60, 30);
        private Vector3 camPosition = new Vector3(0, 20, -34);
        public float sphereSize = 3;
        public int spBufferSize = 16;
        public float baseSpeed = 5;
        private float levelDuration = 30;
        private float timeScale = 1;
        private int texBufferSize = 16;
        public float sphereGenRate = 1.0f;
        private string assetFileName = "Textures.unity3d";

        private MyScene sc;
        private MyUI ui;
        public SpheresGenerator sg;
        private Camera cam;
        private float timer = 0;
        private int tempTimer = 0;
        public int score = 0;
        public int level = 0;

        void Awake()
        {
            #region Singleton
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                if (_instance != this)
                {
                    Debug.Log("Duplicate of '" + this.GetType() + "' script was removed from '" + gameObject.name + "' gameobject");
                    Destroy(this);
                }
                return;
            }
            #endregion
            sc = new MyScene(sceneSizes);
            StartCoroutine(load(assetFileName, 1));
            sg = new SpheresGenerator(sceneSizes, sphereSize, texBufferSize);
            ui = new MyUI();
            cam = Camera.main;
            cam.transform.position = camPosition;
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    SphereController spCtrl = hit.collider.GetComponent<SphereController>();
                    score += Mathf.RoundToInt(spCtrl.trans.localPosition.z);
                    ui.scoreT.text = "Score: " + score;
                    sg.PushSphereToPool(spCtrl);
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

        private IEnumerator load(string assetFileName, int version)
        {
            WWW www;
            Texture[] tex;
            Caching.CleanCache();
            while (!Caching.ready) yield return null;

            www = WWW.LoadFromCacheOrDownload("file:///" + Application.dataPath + "/../" + assetFileName, version);
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
