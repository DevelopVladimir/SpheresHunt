using System;
using UnityEngine;
using System.Collections;

namespace SpheresHunt
{

    public class LoadAssetBundle : MonoBehaviour
    {

        private WWW www;
        private Type type;
        Texture[] tex;
        GameController gc;

        void Start()
        {

            //gc = GameObject.Find("Management").GetComponent<GameController>();
            gc = GameController.Instance;
            Caching.CleanCache();
            string loadUrl = "file://Textures.unity3d";
            StartCoroutine(load(loadUrl, 1));
        }

        private IEnumerator load(string url, int version)
        {

            while (!Caching.ready) yield return null;

            www = WWW.LoadFromCacheOrDownload(url, version);
            yield return www;
            if (www.error != null) throw new Exception("WWW download had an error: " + www.error);

            AssetBundle assetBundle = www.assetBundle;
            String[] assetNames = assetBundle.GetAllAssetNames();
            tex = new Texture[assetNames.Length];

            for (int i = 0; i < assetNames.Length; i++)
            {
                AssetBundleRequest request = assetBundle.LoadAssetAsync(assetNames[i], typeof(Texture));
                yield return request;
                tex[i] = request.asset as Texture;
            }

            gc.ApplyLoadedTextures(tex);

            www.Dispose();
            assetBundle.Unload(false);
        }
    }
}

