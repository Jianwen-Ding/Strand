using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleGridLoadToggle : MonoBehaviour
{
    //Enemies in grid to be deleted on unload
    [SerializeField]
    private bool isStalkerInfected;
    [SerializeField]
    private ArrayList enemiesInGrid = new ArrayList();
    [SerializeField]
    private gridOverallLoader overallLoader;
    [SerializeField]
    private resourceSystem overallResources;
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
            for(int i = 0; i < enemiesInGrid.Count; i++)
            {
                ((GameObject)enemiesInGrid[i]).SetActive(true);
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
            for (int i = 0; i < enemiesInGrid.Count; i++)
            {
                ((GameObject)enemiesInGrid[i]).SetActive(false);
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
    public void setStalkerInfected(bool setBool)
    {
        if(setBool == true)
        {
            overallLoader.setStalkerInfected(gridPositionX, gridPositionY);
        }
        isStalkerInfected = setBool;
    }
    public int getGridPositionX()
    {
        return gridPositionX;
    }
    public int getGridPositionY()
    {
        return gridPositionY;
    }
    public bool getStalkerInfected()
    {
        return isStalkerInfected;
    }
    // Start is called before the first frame update
    void Awake()
    {
        activated = true;
        overallLoader = Camera.main.gameObject.GetComponent<gridOverallLoader>();
        overallResources = Camera.main.gameObject.GetComponent<resourceSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(overallLoader == null)
            {
                overallLoader = Camera.main.gameObject.GetComponent<gridOverallLoader>();
            }
            if(overallResources == null)
            {
                overallResources = Camera.main.gameObject.GetComponent<resourceSystem>();
            }
            overallLoader.updatePlayerPosition(gridPositionX, gridPositionY);
            overallResources.setStalkHunger(isStalkerInfected);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (!enemiesInGrid.Contains(collision.gameObject))
            {
                enemiesInGrid.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            overallResources.setStalkHunger(isStalkerInfected);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (!enemiesInGrid.Contains(collision.gameObject))
            {
                enemiesInGrid.Add(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            overallResources.setStalkHunger(false);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (enemiesInGrid.Contains(collision.gameObject) && collision.gameObject.activeInHierarchy)
            {
                enemiesInGrid.Remove(collision.gameObject);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isStalkerInfected)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
