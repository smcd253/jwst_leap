using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class difficulty_state_machine : MonoBehaviour {
    // difficulty struct
    public struct difficulty
    {
        string name;
        float speed;
        float time_limit;
        public void setName(string newName) { name = newName; }
        public void setSpeed(float newSpeed) { speed = newSpeed; }
        public void setTime_Lim(float newTime_Lim) { time_limit = newTime_Lim; }
        public string getName() { return name; }
        public float getSpeed() { return speed; }
        public float getTime_Lim() { return time_limit; }
    };
    // difficulty select enum
    public enum diff_select
    {
        trainee,
        experienced,
        veteran
    };
    public diff_select next_diff;
	
    public void set_difficulty()
    {
        switch (next_diff)
        {
            case diff_select.trainee:
                break;
            case diff_select.experienced:
                break;
            case diff_select.veteran:
                break;
        }
    }
}
