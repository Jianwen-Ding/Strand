using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskCreatorDefeat : taskCreator
{
    [SerializeField]
    ArrayList allEnemies = new ArrayList();
    [SerializeField]
    float timeUntilClose = 1;
    [SerializeField]
    bool hasInitialized = false;

    public override void onTrigger(Collider2D col)
    {
        if (timeUntilClose >= 0)
        {
            baseEnemy attemptGather = col.gameObject.GetComponent<baseEnemy>();
            if (attemptGather != null && !allEnemies.Contains(attemptGather))
            {
                allEnemies.Add(attemptGather);
                hasInitialized = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilClose -= Time.deltaTime;
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] == null || ((baseEnemy)allEnemies[i]).getEnemyState() == "death")
            {
                allEnemies.RemoveAt(i);
            }
        }
        if (hasInitialized && allEnemies.Count == 0)
        {
            attemptSignalTaskComplete();
        }
    }
}
