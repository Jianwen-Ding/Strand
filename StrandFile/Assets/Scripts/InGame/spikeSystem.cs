using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeSystem : MonoBehaviour
{
    bool playerInSpike;
    PlayerMainScript player;
    [SerializeField]
    int connections;
    [SerializeField]
    float playerHurtTime;
    [SerializeField]
    float playerHurtTimeLeft;
    [SerializeField]
    float enemyTimeUntilHurt;
    [SerializeField]
    float enemyStunTime;
    List<baseEnemy> enemiesInSpike = new List<baseEnemy>();
    List<int> connectionsInSpike = new List<int>();
    List<float> timeInSpike = new List<float>();
    public void attemptAdd(GameObject attemptedAdd)
    {
        PlayerMainScript attemptPlayer = attemptedAdd.GetComponent<PlayerMainScript>();
        if(attemptPlayer != null)
        {
            if (!playerInSpike)
            {
                playerInSpike = true;
                player = attemptPlayer;
                playerHurtTimeLeft = -1;
            }
            connections++;
        }
        else
        {
            baseEnemy attemptEnemy = attemptedAdd.GetComponent<baseEnemy>();
            if(attemptEnemy != null && attemptEnemy.GetType() != typeof(flyingHeadScript) && attemptEnemy.GetType() != typeof(stalkerEnemy))
            {
                int index = enemiesInSpike.IndexOf(attemptEnemy);
                if (index == -1)
                {
                    enemiesInSpike.Add(attemptEnemy);
                    timeInSpike.Add(-1);
                    connectionsInSpike.Add(1);
                }
                else
                {
                    connectionsInSpike[index] = connectionsInSpike[index] + 1;
                }
            }
        }
    }
    public void attemptDelete(GameObject attemptedDelete)
    {
        if(attemptedDelete != null)
        {
            if (playerInSpike && attemptedDelete == player.gameObject)
            {
                if (connections - 1 <= 0)
                {
                    playerInSpike = false;
                    connections = 0;
                }
                else
                {
                    connections--;
                }
            }
            baseEnemy attemptEnemy = attemptedDelete.GetComponent<baseEnemy>();
            if (attemptEnemy != null)
            {
                int index = enemiesInSpike.IndexOf(attemptEnemy);
                if (index != -1)
                {
                    if (connectionsInSpike[index] - 1 <= 0)
                    {
                        enemiesInSpike.RemoveAt(index);
                        timeInSpike.RemoveAt(index);
                        connectionsInSpike.RemoveAt(index);
                    }
                    else
                    {
                        connectionsInSpike[index] = connectionsInSpike[index] - 1;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInSpike)
        {
            playerHurtTimeLeft -= Time.deltaTime;
            if(playerHurtTimeLeft <= 0)
            {
                playerHurtTimeLeft = playerHurtTime;
                player.damagePlayer(1);
            }
        }
        for(int i = 0; i < enemiesInSpike.Count; i++)
        {
            timeInSpike[i] -= Time.deltaTime;
            if (timeInSpike[i] <= 0)
            {
                timeInSpike[i] = enemyTimeUntilHurt;
                enemiesInSpike[i].isDamaged(1);
                enemiesInSpike[i].stunEnemy(enemyStunTime);
            }
        }
    }
}
