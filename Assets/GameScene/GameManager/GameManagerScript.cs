using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject floor; //1
    public GameObject wall;  //2
    public GameObject ceiling; //3
    public GameObject airFloor; //4
    public GameObject damageFloor; //5
    public GameObject goal;

    public GameObject background;

    public GameObject enemy1;
    public GameObject enemy2;

    //�z��̐錾
    int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        LoadMap("map1");
    }

    void LoadMap(string fileName)
    {
        TextAsset csv = Resources.Load<TextAsset>(fileName);
        if (csv == null)
        {
            Debug.LogError("CSV��������܂���: " + fileName);
            return;
        }
        else
        {
            Debug.Log("CSV�ǂݍ��ݐ���: " + fileName);
        }

        StringReader reader = new StringReader(csv.text);
        int y = 0;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            for (int x = 0; x < values.Length; x++)
            {
                int tileType = int.Parse(values[x]);
                if (!int.TryParse(values[x], out tileType))
                {
                    Debug.LogWarning("�����ȃf�[�^: " + values[x] + " (X:" + x + " Y:" + y + ")");
                    continue;
                }
                Vector3 position = new Vector3(x, 5 - y, 0);

                //�@�P���@�Q�ǁ@�R�V��@�S�����Ă鏰�@�T�_���[�W���@�U�����ŕ߂�@�Q�O���s����

                switch (tileType)
                {
                    case 1:
                        if (floor != null)
                            Instantiate(floor, position, Quaternion.identity);
                        break;
                    case 2:
                        if (wall != null)
                            Instantiate(wall, position, Quaternion.identity);
                        break;
                    case 3:
                        if (ceiling != null)
                            Instantiate(ceiling, position, Quaternion.identity);
                        break;
                    case 4:
                        if (airFloor != null)
                            Instantiate(airFloor, position, Quaternion.identity);
                        break;
                    case 5:
                        if (damageFloor != null)
                            Instantiate(damageFloor, position, Quaternion.identity);
                        break;
                    case 6:
                        if (goal != null)
                            Instantiate(goal, position, Quaternion.identity);
                        break;
                    case 10:
                        if (enemy1 != null)
                            Instantiate(enemy1, position + new Vector3(0,0.1f,0), Quaternion.identity);
                        break;
                    case 11:
                        //if (enemy2 != null)
                        //    Instantiate(enemy2, position, Quaternion.identity);
                        break;
                    default:
                        Debug.LogWarning("����`�̃^�C���^�C�v: " + tileType);
                        break;
                }

            }
            y++;
        }
        Debug.Log("�}�b�v��������");
    }

}
