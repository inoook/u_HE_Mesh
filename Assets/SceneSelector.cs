using UnityEngine;
using System.Collections;

public class SceneSelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GUISkin skin;
	public bool isMenu = true;
	
	void OnGUI()
	{
		GUI.skin = skin;
		
		
		if(isMenu){
			GUILayout.BeginArea(new Rect(20,20,140,100));
			if( GUILayout.Button("stickyCube") ){
				SelectScene(1);
			}
			if( GUILayout.Button("buckySlice") ){
				SelectScene(2);
			}
			if( GUILayout.Button("HE_Mesh test") ){
				SelectScene(3);
			}
			GUILayout.EndArea();
		}else{
			GUILayout.BeginArea(new Rect(Screen.width - 100 - 20,20,100,100));
			if( GUILayout.Button("menu") ){
				BackToMenu();
			}
			GUILayout.EndArea();
		}
	}
	
	void SelectScene(int id)
	{
		isMenu = false;
		Application.LoadLevel(id);
	}
	
	void BackToMenu()
	{
		isMenu = true;
		Destroy(this.gameObject);
		Application.LoadLevel(0);
	}
}
