using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();//resizable array
    public bool openWhenEnemiesCleared;

    public Room theRoom;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (theRoom.roomActive && openWhenEnemiesCleared)
        {
            if (enemies.Count > 0)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i] == null)
                    {
                        enemies.RemoveAt(i);
                        Debug.Log(enemies.Count);
                        i--;//for items removed the next item's index becomes the removed items index
                    }
                }
            }

            else if (enemies.Count == 0)
            {
                theRoom.OpenDoors();
            }
        }

    }
}
