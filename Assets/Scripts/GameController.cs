using UnityEngine;
using System.Collections;
namespace SpheresHunt{

	public class GameController : MonoBehaviour {
		
		readonly int width = 40;
		public int height;
		public int sphereSize = 3;
		int spBufferSize = 16;
		MyScene sc;
		MyUI ui;
		public SpheresGenerator sg;
		float period = 0.5f;
		public int spehereGenSize = 30;
		public float baseSpeed;
		Camera cam;
		float timer;
		int tempTimer;
		public int score;
		public int level = 0;
		int levelDuration = 30;
		public Texture[][] textures;
		public bool isTexGradientHorizontal;
		public float timeScale = 1;

		void Awake () {

			sc = new MyScene (width);
			
			textures = new Texture[2][];
			textures [0] = new Texture[16];
			textures [1] = new Texture[16];

			sg = new SpheresGenerator(new Vector3 (-15,sc.height,0),new Vector3 (spehereGenSize, sc.height,spehereGenSize ), sphereSize, spBufferSize);

			ui = new MyUI ();

			baseSpeed = 3;
			height = sc.height;
			
			Vector3 camPosition = new Vector3(0,width*2/5, -width*19.5f/40);
			cam = Camera.main;
			cam.transform.position = camPosition;

			timer = 0;
			score = 0;
			tempTimer = 0;
		}
		
		void Start(){
			
			StartCoroutine ("PushSpheres");
			StartCoroutine ("ChangeLevel");
		}
		
		void Update(){
			
			Fire ();
			UpdateTime ();
			Time.timeScale = timeScale;
		}
		
		void Fire (){

			RaycastHit hit;
			GameObject sphere;
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit)) {
					sphere = hit.collider.gameObject;
					score += Mathf.RoundToInt(sphere.transform.localPosition.z);
					ui.scoreT.text = "Score: "+score;
					sg.ResetSphere(sphere);
				}
			}
		}

		void UpdateTime(){
			timer += Time.deltaTime;
			if (timer - tempTimer > 1) {
				tempTimer++;
				ui.AddSeconds();
			}
		}

		IEnumerator PushSpheres(){

			while(true){
				for (int i = 0;  i < spBufferSize; i++){
					sg.Push(i);
					yield return new WaitForSeconds(period);
				}
			}
		}

		IEnumerator ChangeLevel(){

			while (true) {

				isTexGradientHorizontal = (level%2 == 0)?true:false;
				for (int i = 0; i < textures[(level+1)%2].Length; i++)textures [(level+1)%2] [i] = null;

				for (int i = 0; i < textures[(level+1)%2].Length; i++) {
					int power = (int)Mathf.Pow(2,i/4 + 5);
					textures [(level+1)%2] [i] = sg.GenTexture (power,!isTexGradientHorizontal);
					yield return new WaitForSeconds((float)levelDuration/16.0f);
				}

				level++;
				baseSpeed = level+3;
			}
		}

		public void ApplyLoadedTextures(Texture[] tex){

			sc.ApplyAssetBundleTexture (tex);
		}
	}
}
