using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nightSystem : MonoBehaviour
{
    //Only one of this should exist since it uses static
    //--STATIC VARIABLES--
    [SerializeField]
    private static int daysSpent;
    [SerializeField]
    private static float daySpendDifficultyMultiplier;
    [SerializeField]
    private static float timeUntilNight;
    [SerializeField]
    private static float currentTimePassed;
    [SerializeField]
    private static float timeDifficultyMultiplier;
    //--STATIC SETTING VARIABLES--
    [SerializeField] 
    private float currentTimePassedSet;
    [SerializeField]
    private float timeDifficultyMultiplierSet;
    //--public fucntions--
    public static void setTimeUntilNight(float setTime)
    {
        timeUntilNight = setTime;
    }
    public static float getTimeUntilNight()
    {
        return timeUntilNight;
    }
    public static float getCurrentTimePassed()
    {
        return currentTimePassed;
    }
    public static float getTimeDifficultyMultiplier()
    {
        return timeDifficultyMultiplier;
    }
    // Start is called before the first frame update
    void Awake()
    {
        daySpendDifficultyMultiplier = PlayerPrefs.GetInt("daysSpent", 0);
        currentTimePassed = currentTimePassedSet;
        timeDifficultyMultiplier = timeDifficultyMultiplierSet;
    }
    // Update is called once per frame
    void Update()
    {
        currentTimePassed += Time.deltaTime;
    }
}
