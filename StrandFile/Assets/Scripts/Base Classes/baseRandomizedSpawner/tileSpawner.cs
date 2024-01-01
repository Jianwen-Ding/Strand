using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tileSpawner : baseRandomizedSpawner
{
    singleGridPageLoader givenLoader;

    public void setLoader(singleGridPageLoader set)
    {
        givenLoader = set;
    }

    public override GameObject gameObjectSpawn(GameObject prefab, Vector3 position)
    {
        GameObject returnObject = base.gameObjectSpawn(prefab, position);
        givenLoader.addLoadedObject(returnObject);
        return returnObject;
    }
}
