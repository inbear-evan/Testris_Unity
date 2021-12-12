using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMove : MonoBehaviour
{

    float prevTime;
    float fallTime = 0.5f;
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0, 0, -90);
            if (!ValidMove())
                transform.Rotate(0, 0, 90);
        }

        if (Time.time - prevTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            if (!isFull())
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMove())
                {
                    transform.position += new Vector3(0, 1, 0);
                    Add2Grid();
                    CheckLines();
                    this.enabled = false;
                    FindObjectOfType<SpawnBlock>().SpawnNewBlcok();
                    
                }
            }
            prevTime = Time.time;
        }
    }
    static bool isFull()
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, height - 1] != null) {
                JsonManager.instance.canvasShow(true);
                return true;
            }
        }
        return false;
    }

    void CheckLines()
    {
        for (int i = height-1; i >= 0; i--)
        {
            if (hasLine(i))
            {
                deleteLine(i);
                RowDown(i);
            }
        }
    }

    private void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if(grid[j,y] != null)
                {
                    grid[j,y-1] = grid[j,y];
                    grid[j,y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    private void deleteLine(int i)
    {
        ScoreManager.instance.SCORELINES++;
        if (fallTime < 1.5) fallTime += 0.1f;
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    private bool hasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void Add2Grid()
    {
        foreach(Transform t in transform)
        {
            int roundX = Mathf.RoundToInt(t.transform.position.x);
            int roundY = Mathf.RoundToInt(t.transform.position.y);
            //Debug.Log(roundX+" "+ roundY);
            grid[Mathf.Abs(roundX), Mathf.Abs(roundY)] = t;
        }
    }
    bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.transform.position.x);
            int roundY = Mathf.RoundToInt(child.transform.position.y);

            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
            if (grid[Mathf.Abs(roundX), Mathf.Abs(roundY)] != null)
            {
                //
                //Debug.Log(roundX + " " + roundY);
                return false;
            }
                
        }
        return true;
    }
}

