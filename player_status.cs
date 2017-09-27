using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_status : MonoBehaviour {
    public enum Experience
    {
        beginner,
        trainee,
        expert,
        veteran
    }; // player experience (based on time in game)
    public struct status
    {
        string name;
        string companyRoll;
        scene_state_machine scene;
        float timeInGame;
        float timeInScene;
        
        Experience playerXP;

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
        public scene_state_machine Scene
        {
            get
            {
                return scene;
            }

            set
            {
                scene = value;
            }
        }
        public float TimeInGame
        {
            get
            {
                return timeInGame;
            }

            set
            {
                timeInGame = value;
            }
        }
        public float TimeInScene
        {
            get
            {
                return timeInScene;
            }

            set
            {
                timeInScene = value;
            }
        }
        public string CompanyRoll
        {
            get
            {
                return companyRoll;
            }

            set
            {
                companyRoll = value;
            }
        }
        public Experience PlayerXP
        {
            get
            {
                return playerXP;
            }

            set
            {
                playerXP = value;
            }
        }
    } // player status
    public status currentPlayerStatus = new status();
	// Use this for initialization
	void Start ()
    {
        currentPlayerStatus.TimeInGame = Time.realtimeSinceStartup;
        currentPlayerStatus.TimeInScene = Time.timeSinceLevelLoad;
        currentPlayerStatus.Scene.name = Application.loadedLevelName;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // update time variables
        currentPlayerStatus.TimeInGame = Time.realtimeSinceStartup;
        currentPlayerStatus.TimeInScene = Time.timeSinceLevelLoad;
        // update experience
        if (currentPlayerStatus.TimeInGame < 60f)
        {
            currentPlayerStatus.PlayerXP = Experience.beginner;
        }
        else if (currentPlayerStatus.TimeInGame >= 60f)
        {
            currentPlayerStatus.PlayerXP = Experience.trainee;
        }
        else if (currentPlayerStatus.Scene.name == "jwst_space_game1" || currentPlayerStatus.Scene.name == "jwst_space_game2")
        {
            /*------------------pause to start debugging early code --------------------*/
        }
    }
}
