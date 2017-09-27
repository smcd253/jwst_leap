using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_manager : MonoBehaviour {

    // scene manager
    public scene_state_machine scene_manager;
    public scene_state_machine.scene thisScene;
    public void enableSceneMenu()
    {
        thisScene = scene_manager.currentScene;
        //if (thisScene.getName() == "JWST_mainMenu(leap)(SteamVR)")
        //{
        //    mainMenu();
        //}
    }

    public void mainMenu()
    {
        scene_manager.next_scene = scene_state_machine.scene_select.main_menu;
        scene_manager.update_scene();
    }

    public void mainMenu_chooseGame1()
    {
        scene_manager.next_scene = scene_state_machine.scene_select.game1;
        scene_manager.update_scene();
    }

    public void mainMenu_chooseGame2()
    {
        scene_manager.next_scene = scene_state_machine.scene_select.game2;
        scene_manager.update_scene();
    }
}
