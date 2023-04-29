using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleGridLoadToggle : MonoBehaviour
{
    //Enemies in grid to be deleted on unload
    [SerializeField]
    private ArrayList enemiesInGrid = new ArrayList();
    [SerializeField]
    private gridOverallLoader overallLoader;
    [SerializeField]
    private int gridPositionX;
    [SerializeField]
    private int gridPositionY;
    [SerializeField]
    private bool activated = true;
    //PUBLIC FUNTIONS
    public void activateGrid()
    {
        if (!activated)
        {
            activated = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
    public void deactivateGrid()
    {
        if (activated)
        {
            activated = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            if(enemiesInGrid.Count != 0)
            {
                GameObject[] enemyArray = (GameObject[])enemiesInGrid.ToArray();
                for (int i = 0; i < enemyArray.Length; i++)
                {
                    Destroy(enemyArray[i]);
                }
                enemiesInGrid.Clear();
            }
        }
    }
    //GET/SET FUNCTIONS
    public void setGridPositionX(int setPosition)
    {
        gridPositionX = setPosition;
    }
    public void setGridPositionY(int setPosition)
    {
        gridPositionY = setPosition;
    }
    // Start is called before the first frame update
    void Awake()
    {
        activated = true;
        overallLoader = Camera.main.gameObject.GetComponent<gridOverallLoader>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(overallLoader == null)
            {
                overallLoader = Camera.main.gameObject.GetComponent<gridOverallLoader>();
            }
            overallLoader.updatePlayerPosition(gridPositionX, gridPositionY);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
