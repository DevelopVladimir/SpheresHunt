using System;
using UnityEngine;
using System.Collections;

public class LoadAssetBundle : MonoBehaviour {
	private WWW www;
	private Type type;
	Texture[] tex;

	void Start () {
		Caching.CleanCache();
		string loadUrl = "file://Textures.unity3d";
		StartCoroutine(load(loadUrl, 1));
	}

	private IEnumerator load(string url, int version) {

    	while (!Caching.ready) yield return null;
    	www = WWW.LoadFromCacheOrDownload(url, version);
		yield return www;
		if (www.error != null) throw new Exception("WWW download had an error: " + www.error);
    
    AssetBundle assetBundle = www.assetBundle;
		String [] assetNames = assetBundle.GetAllAssetNames ();
		tex = new Texture[assetNames.Length];
		for (int i=0; i < assetNames.Length; i++){
			AssetBundleRequest request = assetBundle.LoadAssetAsync (assetNames[i], typeof(Texture));
			yield return request;
			tex[i] = request.asset as Texture;
		}
	
		Material mat = GameObject.Find ("LeftWall").GetComponent<Renderer> ().material;
		mat.mainTexture = tex[0];
		mat.mainTextureScale = new Vector2 (2,2);

		mat = GameObject.Find ("RightWall").GetComponent<Renderer> ().material;
		mat.mainTexture = tex[0];
		mat.mainTextureScale = new Vector2 (2,2);

		mat = GameObject.Find ("BackWall").GetComponent<Renderer> ().material;
		mat.mainTexture = tex[0];
		mat.mainTextureScale = new Vector2 (2,2);

		GameObject.Find("Floor").GetComponent<Renderer> ().material.mainTexture = tex[1];


	www.Dispose();
    assetBundle.Unload(false);
  }

}
