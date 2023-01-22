using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableScriptEnemy : grabbableObject
{
    //--VARIENT OF GRABBABLE OBJECT THAT NEEDS TO BE ATTACHED WITH BASE ENEMY
    private baseEnemy enemyScript;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        enemyScript = gameObject.GetComponent<baseEnemy>();
        if(enemyScript == null)
        {
            print("The grabbableScripEnemy script attached to " + gameObject.name + "requires the script baseChildren, or any children scripts");
        }
    }
}
