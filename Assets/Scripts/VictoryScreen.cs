﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VictoryScreen : MonoBehaviour
{
    public float waitForAnyKey = 2f;
    public GameObject anyKeyText;
    public string mainMenuScene;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitForAnyKey > 0)
        {
            waitForAnyKey -= Time.deltaTime;
            if(waitForAnyKey <= 0)
            {
                anyKeyText.SetActive(true);
            }
        }
        else 
        {
            if(Input.anyKeyDown)
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }
    }
}
