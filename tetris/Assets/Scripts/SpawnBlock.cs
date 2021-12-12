using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlock : MonoBehaviour
{
    public GameObject[] blockList;
    

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBlcok();
    }

    public void SpawnNewBlcok()
    {
        int i;
        
        i = Random.Range(0, blockList.Length);
        Instantiate(blockList[i], transform.position, Quaternion.identity);
    }

    

}
