using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrapSpawner : baseRandomizedSpawner
{
    //Changes prefabList on start
    public override GameObject[] prefabGatherers()
    {
        return scrapStorer.getNormalPrefabs();
    }

    //Changes chanceList on start
    public override float[] chanceGatherers()
    {
        float[] chance;
        GameObject[] prefabs = scrapStorer.getNormalPrefabs();
        chance = new float[prefabs.Length];
        for(int i = 0; i < chance.Length; i++)
        {
            chance[i] = 1;
        }
        return chance;
    }
}
