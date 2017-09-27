using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isolate_component_v3 : MonoBehaviour {
    public Transform parent;
    public string reveal;
    public AudioSource holo_audio;

    private void Start()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            for (int j = 0; j < parent.GetChild(i).childCount; j++)
            {
                for (int k = 0; k < parent.GetChild(i).GetChild(j).childCount; k++)
                {
                    // enable all standard objects
                    if (parent.GetChild(i).GetChild(j).GetChild(k).gameObject.CompareTag("standard"))
                    {
                        parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(true);
                    }
                    // disable all holo objects
                    else
                    {
                        parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    public void set_reveal(string input)
    {
        reveal = input;
    }
    public void reveal_component()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            for (int j = 0; j < parent.GetChild(i).childCount; j++)
            {
                if (parent.GetChild(i).GetChild(j).gameObject.name == reveal)
                {
                    for (int k = 0; k < parent.GetChild(i).GetChild(j).childCount; k++)
                    {
                        // reveal standard version of reveal object to true (reveal object)
                        if(parent.GetChild(i).GetChild(j).GetChild(k).gameObject.CompareTag("standard"))
                        {
                            parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(true);
                        }
                        // disable holo version of reveal object
                        else
                        {
                            parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    for (int k = 0; k < parent.GetChild(i).GetChild(j).childCount; k++)
                    {
                        // reveal holo version of all other objects
                        if (parent.GetChild(i).GetChild(j).GetChild(k).gameObject.CompareTag("holo"))
                        {
                            parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(true);
                        }
                        // disable standard version of all other objects
                        else
                        {
                            parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(false);
                        }
                    }
                }
                // play hologram audio
                holo_audio.Play();
            }
        }
    }

    public void return_normality()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            for (int j = 0; j < parent.GetChild(i).childCount; j++)
            {
                for (int k = 0; k < parent.GetChild(i).GetChild(j).childCount; k++)
                {
                    // enable all standard objects
                    if (parent.GetChild(i).GetChild(j).GetChild(k).gameObject.CompareTag("standard"))
                    {
                        parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(true);
                    }
                    // disable all holo objects
                    else
                    {
                        parent.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(false);
                    }
                }
            }
        }
        // pause holo audio
        holo_audio.Pause();
        

    }
}

