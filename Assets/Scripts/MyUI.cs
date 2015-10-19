using UnityEngine;
using System.Collections;
using System.IO;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace SpheresHunt{

	public class MyUI {

		GameObject ui;
		GameObject secText;
		GameObject minText;
		GameObject hoursText;
		GameObject scoreText;
		private const int LayerUI = 5;
		int sec;
		int min;
		int hour;
		Text secT;
		Text minT;
		Text hourT;
		public Text scoreT;
		
		public MyUI(){

			ui = new GameObject();
			ui.name = "UI";
			Transform uiTrans = ui.transform;
			GameObject canvas = CreateCanvas(uiTrans);
			CreateEventSystem(canvas.transform);
			
			hoursText = CreateText(canvas.transform, -372, -200, 160, 50, "0:", 24); hoursText.name = "Hours";
			minText = CreateText(canvas.transform, -345, -200, 160, 50, "00:", 24); minText.name = "Min";
			secText = CreateText(canvas.transform, -315, -200, 160, 50, "00", 24); secText.name = "Sec";
			scoreText = CreateText(canvas.transform, -330, -170, 160, 50, "Score: 0", 24); scoreText.name = "Score";
			sec = 0;
			min = 0;
			hour = 0;
			secT = secText.GetComponent<Text>();
			minT = minText.GetComponent<Text>();
			hourT = secText.GetComponent<Text>();
			scoreT = scoreText.GetComponent<Text>();
		}

		public void AddSeconds(){
			
			sec++;
			secT.text = (sec<10)?"0"+sec:""+sec;
			if (sec == 60) {
				sec = 0;
				secT.text = (sec<10)?"0"+sec:""+sec;
				min++;
				minT.text = (min<10)?"0"+min+":":""+min+":";
				if (min == 60) {
					min = 0;
					minT.text = (min<10)?"0"+min+":":""+min+":";
					hour++;
					hourT.text = ""+hour+":";
				}
			}
		}
		
		private GameObject CreateCanvas(Transform parent) {
			
			GameObject canvasObject = new GameObject("Canvas");
			canvasObject.layer = LayerUI;
			canvasObject.AddComponent<RectTransform>();
			
			Canvas canvas = canvasObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.pixelPerfect = true;
			
			CanvasScaler canvasScal = canvasObject.AddComponent<CanvasScaler>();
			canvasScal.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScal.referenceResolution = new Vector2(800, 600);
			canvasObject.transform.SetParent(parent);
			
			return canvasObject;
		}
		
		private GameObject CreateEventSystem(Transform parent) {

			GameObject esObject = new GameObject();
			esObject.name = "EventSystem";
			EventSystem esClass = esObject.AddComponent<EventSystem>();
			esClass.sendNavigationEvents = true;
			esClass.pixelDragThreshold = 5;
			esObject.transform.SetParent(parent);
			
			return esObject;
		}
		
		private GameObject CreateText(Transform parent, float x, float y, float w, float h, string message, int fontSize) {

			GameObject textObject = new GameObject();
			textObject.transform.SetParent(parent);
			textObject.layer = LayerUI;
			RectTransform trans = textObject.AddComponent<RectTransform>();
			trans.sizeDelta.Set(w, h);
			trans.anchoredPosition3D = new Vector3(0, 0, 0);
			trans.anchoredPosition = new Vector2(x, y);
			trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			trans.localPosition.Set(0, 0, 0);
			
			textObject.AddComponent<CanvasRenderer>();
			Text text = textObject.AddComponent<Text>();
			text.supportRichText = true;
			text.text = message;
			text.fontSize = fontSize;
			text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
			text.alignment = TextAnchor.MiddleCenter;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.color = Color.green;
			
			return textObject;
		}
		
		GameObject CreateUITextObj( string name, Transform parentObjTrans){

			GameObject textObj;
			Transform trans;
			textObj = new GameObject();
			trans = textObj.transform;
			trans.SetParent (parentObjTrans, false);
			textObj.AddComponent<Text> ();
			return textObj;
		}
	}
}

