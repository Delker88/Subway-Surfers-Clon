using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    [SerializeField] private List<GameObject> blocksSpawn;
    [SerializeField] private List<GameObject> blocksCounter;
    [SerializeField]private int spawnCounter;
    int blockNum = 0;

    private void Awake()
    {
        blocksCounter.Add (Instantiate(blocksSpawn[0]));
        SpawnerBlock();
    }
    private void SpawnerBlock()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnBlocks(true);
            if (spawnCounter >= 5)
            {
                GameObject block = Instantiate((blocksSpawn[0]), blocksCounter[spawnCounter].transform.Find("spawn_end").transform.position, blocksCounter[spawnCounter].transform.Find("spawn_end").transform.rotation);
                blocksCounter.Add(block);
                spawnCounter++;
                GameObject block1 = Instantiate((blocksSpawn[1]), blocksCounter[spawnCounter].transform.Find("spawn_end").transform.position, blocksCounter[spawnCounter].transform.Find("spawn_end").transform.rotation);
                blocksCounter.Add(block1);
                spawnCounter++;
            }
        }
    }
    private void SpawnBlocks(bool spawn)


    {
        if (spawn)
        {
               spawn = false;
              //int numberRandom = Random.Range(0, blocksSpawn.Count);
              print("Numero de blocke"+blockNum);
               GameObject block = Instantiate(blocksSpawn[blockNum ], blocksCounter[spawnCounter].transform.Find("spawn_end").transform.position, blocksCounter[spawnCounter].transform.Find("spawn_end").transform.rotation);
               //block.name = block + spawnCounter.ToString();
               blocksCounter.Add(block);
               spawnCounter++;
               blockNum ++;
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
