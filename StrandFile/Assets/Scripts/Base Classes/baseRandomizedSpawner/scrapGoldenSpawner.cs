using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrapGoldenSpawner : baseRandomizedSpawner
{
    //Changes prefabList on start
    public override GameObject[] prefabGatherers()
    {
        return scrapStorer.getGoldenPrefabs();
    }

    //Changes chanceList on start
    public override float[] chanceGatherers()
    {
        float[] chance;
        GameObject[] prefabs = scrapStorer.getGoldenPrefabs();
        chance = new float[prefabs.Length];
        for (int i = 0; i < chance.Length; i++)
        {
            chance[i] = 1;
        }
        return chance;
    }
}
