using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour {
    // declare subscript objects
    public scene_state_machine sceneManager;
    public menu_manager menuManager;
	// Use this for initialization
	void Start () {
        // initialize managers
        sceneManager.start_scene();
        menuManager.enableSceneMenu();
	}
	
	// Update is called once per frame
	void Update () {
		// move some functionality over from menu manager
	}
}
