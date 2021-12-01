using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(string filepath)
    {
        Transform spawnableParent = GameObject.Find("Spawnables").transform;
        var entity = Resources.Load(filepath);
        var spawnable = (GameObject) Instantiate(entity, transform.position, Quaternion.identity);
        spawnable.transform.parent = spawnableParent;
    }
}