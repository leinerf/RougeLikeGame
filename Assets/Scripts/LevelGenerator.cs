using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelGenerator : MonoBehaviour
{
    public GameObject layoutRoom;
    public Color startColor, endColor;
    public int distanceToEnd;//rooms before get to the end
    // Start is called before the first frame update
    public Transform generatorPoint;
    //0 = top, 1 = right, 2 = bottom, 3 = left
    public enum Direction { up, right, down, left };
    private Direction selectedDirection;
    public float xOffset = 18;
    public float yOffset = 10;
    public LayerMask whatIsRoom;
    private GameObject endRoom;
    private List<GameObject> layoutRooms = new List<GameObject>();//dont add end and start room

    public RoomPrefabs rooms;

    private List<GameObject> generatedOutlines = new List<GameObject>();

    public RoomCenter centerStart, centerEnd;
    public RoomCenter[] RoomCenterArr;
    void Start()
    {
        Instantiate(
            layoutRoom,
            generatorPoint.position,
            generatorPoint.rotation
            ).GetComponent<SpriteRenderer>().color = startColor;

        for (int i = 0; i < distanceToEnd; i++)
        {
            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();
            while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
            {
                MoveGenerationPoint();
            }
            GameObject newRoom = Instantiate(
            layoutRoom,
            generatorPoint.position,
            generatorPoint.rotation
            );

            if (i + 1 == distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
                endRoom = newRoom;
            }
            else
            {
                layoutRooms.Add(newRoom);
            }
        }

        //create room outlines
        CreateRoomOutline(Vector3.zero);
        foreach (GameObject room in layoutRooms)
        {
            CreateRoomOutline(room.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
        Debug.Log(generatedOutlines.Count);

        foreach (GameObject outline in generatedOutlines)
        {
            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else if(outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else
            {
                int centerSelect = Random.Range(0, RoomCenterArr.Length);
                Instantiate(RoomCenterArr[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }

        }
        // Instantiate(centerStart, transform.position, transform.rotation).theRoom = 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void MoveGenerationPoint()
    {
        //18 units by x
        //10 units by y
        switch (selectedDirection)
        {
            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);
                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0, 0f);
                break;
            case Direction.down:
                generatorPoint.position -= new Vector3(0f, yOffset, 0f);
                break;
            case Direction.left:
                generatorPoint.position -= new Vector3(xOffset, 0, 0f);
                break;

        }
    }

    public void CreateRoomOutline(Vector3 roomPosition)
    {
        bool roomTop = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
        bool roomBottom = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);

        int directionCount = 0;
        if (roomTop)
        {
            directionCount++;
        }
        if (roomBottom)
        {
            directionCount++;
        }
        if (roomRight)
        {
            directionCount++;
        }
        if (roomLeft)
        {
            directionCount++;
        }

        switch (directionCount)
        {
            case 0:
                Debug.LogError("found no room exist");
                break;
            case 1:
                if (roomTop)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomT, roomPosition, transform.rotation));
                }
                if (roomBottom)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomB, roomPosition, transform.rotation));
                }
                if (roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomR, roomPosition, transform.rotation));
                }
                if (roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomL, roomPosition, transform.rotation));
                }
                break;
            case 2:
                if (roomTop && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomTR, roomPosition, transform.rotation));
                }
                if (roomTop && roomBottom)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomTB, roomPosition, transform.rotation));
                }
                if (roomTop && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomTL, roomPosition, transform.rotation));
                }
                if (roomRight && roomBottom)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomRB, roomPosition, transform.rotation));
                }
                if (roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomRL, roomPosition, transform.rotation));
                }
                if (roomBottom && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomBL, roomPosition, transform.rotation));
                }
                break;
            case 3:
                if (roomTop && roomRight && roomBottom)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomTRB, roomPosition, transform.rotation));
                }
                if (roomRight && roomBottom && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomRBL, roomPosition, transform.rotation));
                }
                if (roomBottom && roomLeft && roomTop)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomTBL, roomPosition, transform.rotation));
                }
                if (roomLeft && roomTop && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.roomTRL, roomPosition, transform.rotation));
                }
                break;
            case 4:
                generatedOutlines.Add(Instantiate(rooms.roomTRBL, roomPosition, transform.rotation));
                break;
        }

    }
}

[System.Serializable]
public class RoomPrefabs
{
    public GameObject roomT, roomR, roomB, roomL,
    roomRL, roomTB, roomTR, roomRB, roomBL, roomTL,
    roomTRB, roomRBL, roomTBL, roomTRL,
    roomTRBL;
}