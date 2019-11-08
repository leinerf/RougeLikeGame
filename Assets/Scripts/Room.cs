using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public bool closedWhenEntered, openWhenEnemiesCleared;

    public GameObject[] doors;//cant resize without work
    public List<GameObject> enemies = new List<GameObject>();//resizable array

    private bool roomActive = false;

    // Before game starts
    private void Awake()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (roomActive && openWhenEnemiesCleared && enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null)
                {
                    enemies.RemoveAt(i);
                    i--;//for items removed the next item's index becomes the removed items index
                }
            }
            if (enemies.Count == 0)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(false);
                }
                closedWhenEntered = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);
            roomActive = true;
            if (closedWhenEntered)
            {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }
}
//on child object because we want to have the walls interact with bulletes