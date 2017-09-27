using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scene_state_machine : MonoBehaviour {
    
    public struct scene
    {
        string name;
        float time_open;
        public void setName(string newName)
        {
            name = newName;
        }
        public void setTime(float newTime)
        {
            time_open = newTime;
        }
        public string getName()
        {
            return name;
        }
        public float get_time_open()
        {
            return time_open;
        }
    };
    public scene currentScene;

    // button variables for scroll_scene
    public enum scene_select // enumerated scene variable
    {
        main_menu,
        game1,
        game2
    };
    public scene_select next_scene; //scene selector

    // Use this for initialization
    public void start_scene()
    {
        currentScene.setName(Application.loadedLevelName);
        currentScene.setTime(Time.timeSinceLevelLoad);
    }

    // Update is called once per frame
    public void update_scene () {
        currentScene.setTime(Time.timeSinceLevelLoad); // update time level open
        Debug.Log("Level: " + currentScene.getName()); // output level to console
        Debug.Log("Time Level Open: " + currentScene.get_time_open()); // output time open to console

        switch (next_scene)
        {
            case scene_select.main_menu:
                if(currentScene.getName() == "JWST_mainMenu(leap)(SteamVR)")
                {
                    Debug.Log("Scene: " + currentScene.getName() + "already loaded.");
                }
                else
                {
                    //Application.LoadLevel("jwst_space_mainMenu");
                    SteamVR_LoadLevel.Begin("JWST_mainMenu(leap)(SteamVR)", false, 0.5f);
                }
                break;
            case scene_select.game1:
                if (currentScene.getName() == "JWST_smallInteractive(leap)(SteamVR)")
                {
                    Debug.Log("Scene: " + currentScene.getName() + "already loaded.");
                }
                else if (currentScene.getName() == "JWST_smallInteractive(leap)(SteamVR)")
                {
                    Debug.Log("Cannot load game1 from game.\nGo to main menu.");
                }
                else
                {
                    //Application.LoadLevel("JWST_smallInteractive(leap)(SteamVR)");
                    SteamVR_LoadLevel.Begin("JWST_smallInteractive(leap)(SteamVR)", false, 0.5f);
                }
                break;
            case scene_select.game2:
                if (currentScene.getName() == "JWST_largeReachAccess(leap)(SteamVR)")
                {
                    Debug.Log("Scene: " + currentScene.getName() + "already loaded.");
                }
                else if (currentScene.getName() == "JWST_largeReachAccess(leap)(SteamVR)")
                {
                    Debug.Log("Cannot load game2 from game.\nGo to main menu.");
                }
                else
                {
                    //Application.LoadLevel("jwst_space_game2");
                    SteamVR_LoadLevel.Begin("JWST_largeReachAccess(leap)(SteamVR)", false, 0.5f);
                }
                break;
        }
	}
}
