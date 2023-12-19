using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class storageEnemy : baseEnemy
{
    //STORAGE ENEMY
    //Does no damage,
    //Only meant to drop a useful object
    //Animation State
    //0 - Default
    //1 - Open

    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                getObjectAnimator().SetInteger("EnemyState", 1);
                break;
            case "stunned":
                if (getObjectAnimator().GetInteger("EnemyState") != 3)
                    getObjectAnimator().SetInteger("EnemyState", 2);
                break;
            case "death":
                getObjectAnimator().SetInteger("EnemyState", 3);
                break;
        }
    }

}
