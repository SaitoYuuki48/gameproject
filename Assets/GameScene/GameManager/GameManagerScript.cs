using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject floor;
    public GameObject wall;
    public GameObject ceiling;
    public GameObject background;

    public GameObject enemy1;

    //îzóÒÇÃêÈåæ
    int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,]
        {
            {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0},
            {2,0,0,0,0,0,0,0,0,2,2,0,0,0,9,0,0,0,2,0,0,0,0,0,0,0},
            {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };

        int lenY = map.GetLength(0);
        int lenX = map.GetLength(1);

        Vector3 position = Vector3.zero;

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                position.x = x;
                position.y = -y + 5;
                if (map[y, x] == 1)
                {
                    Instantiate(floor, position, Quaternion.identity);
                }

                if (map[y, x] == 2)
                {
                    Instantiate(wall, position, Quaternion.identity);
                }

                if (map[y, x] == 3)
                {
                    Instantiate(ceiling, position, Quaternion.identity);
                }

                if (map[y, x] == 9)
                {
                    Instantiate(enemy1, position, Quaternion.identity);
                }
            }
        }

        //îwåi
        for (int y = 0; y < lenY; y++)
        {
            for (int x = 0; x < lenX; x++)
            {
                position.x = x;
                position.y = -y + 5;
                position.z = 1;
                Instantiate(background, position, Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
