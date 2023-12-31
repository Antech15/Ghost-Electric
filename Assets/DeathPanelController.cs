using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanelController : MonoBehaviour
{

    private int buttonWidth;
    private int buttonHeight;
    private int origin_x;
    private int origin_y;

    // Start is called before the first frame update
    void Start()
    {
        buttonWidth = Screen.width / 4;
        buttonHeight = Screen.height / 20;

        origin_x = Screen.width / 2 - buttonWidth / 2;
        origin_y = Screen.height / 2 - buttonHeight * 2;
    }
    
	
    void OnGUI() {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = Mathf.RoundToInt(Screen.width * 0.015f);

		if(GUI.Button(new Rect(origin_x + 220, origin_y+240, 160, 60), "Restart", buttonStyle)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
        
        if(GUI.Button(new Rect(origin_x + 220, origin_y+170, 160, 60), "Menu", buttonStyle)) {
			SceneManager.LoadScene("Scene 0 - Title");
		}
	}
    
    
}
