using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class miniMapGenerator : MonoBehaviour
{
    //--Generation Parameters--
    [SerializeField]
    private Vector2 distanceApart;
    [SerializeField]
    private GameObject[][] generatedGrids;
    //sprite renderers used for player tracking
    [SerializeField]
    private Image[][] generatedSpriteRenderers;
    [SerializeField]
    private Vector2 displacementStart;
    [SerializeField]
    private Color defualtGridColor;
    [SerializeField]
    private Color playerOnGridColor;
    //--Generative Prefabs--
    [SerializeField]
    private GameObject[] gridSymbol;
    [SerializeField]
    private GameObject baseSymbol;
    [SerializeField]
    private GameObject foodSymbol;
    [SerializeField]
    private GameObject waterSymbol;
    [SerializeField]
    private GameObject scrapSymbol;
    //--Setup variables--
    [SerializeField]
    private gridOverallLoader gridLoader;
    //--Changing variables--
    [SerializeField]
    private bool loadedIn = false;
    [SerializeField]
    private int loadedPlayerX = -99;
    [SerializeField]
    private int loadedPlayerY = -99;
    // Start is called before the first frame update
    public void loadMiniMap()
    {
        loadedIn = true;
        gridLoader = Camera.main.gameObject.GetComponent<gridOverallLoader>();
        generatedGrids = new GameObject[gridLoader.getYGridLength()][];
        generatedSpriteRenderers = new Image[gridLoader.getYGridLength()][];
        for (int y = 0; y < generatedGrids.Length; y++)
        {
            generatedGrids[y] = new GameObject[gridLoader.getXGridLength()];
            generatedSpriteRenderers[y] = new Image[gridLoader.getXGridLength()];
            for (int x = 0; x < generatedGrids[y].Length; x++)
            {
                //Finds matching generated grid prefab
                int foundMatch = -1;
                for(int i = 0; i < gridSymbol.Length && foundMatch == -1; i++)
                {
                     if ((gridLoader.getPageGridMap()[y][x].getUpOpen() == (gridSymbol[i].name.IndexOf("D") != -1)) && (gridLoader.getPageGridMap()[y][x].getDownOpen() == (gridSymbol[i].name.IndexOf("U") != -1)) && (gridLoader.getPageGridMap()[y][x].getRightOpen() == (gridSymbol[i].name.IndexOf("R") != -1)) && (gridLoader.getPageGridMap()[y][x].getLeftOpen() == (gridSymbol[i].name.IndexOf("L") != -1)))
                    {
                        foundMatch = i;
                    }
                }
                //Spawns in grid
                generatedGrids[y][x] = Instantiate(gridSymbol[foundMatch], new Vector3(distanceApart.x * x, distanceApart.y * y), Quaternion.identity.normalized);
                generatedGrids[y][x].transform.SetParent(gameObject.transform);
                //gets sprite renderer component
                generatedSpriteRenderers[y][x] = generatedGrids[y][x].GetComponent<Image>();
                //Spawns in special symbol
                GameObject foundSymbolPrefab = null;
                switch (gridLoader.getPageGridMap()[y][x].getPageSpecialUse()) 
                {
                    case "default":
                        break;
                    case "food":
                        foundSymbolPrefab = foodSymbol;
                        break;
                    case "water":
                        foundSymbolPrefab = waterSymbol;
                        break;
                    case "scrap":
                        foundSymbolPrefab = scrapSymbol;
                        break;
                    case "base":
                        foundSymbolPrefab = baseSymbol;
                        break;
                }
                if(foundSymbolPrefab != null)
                {
                    GameObject newSymbol = Instantiate(foundSymbolPrefab, new Vector3(distanceApart.x * x, distanceApart.y * y), Quaternion.identity.normalized);
                    newSymbol.transform.SetParent(generatedGrids[y][x].transform);
                }
                //Shuts off grid to be activated on discovery
                generatedGrids[y][x].SetActive(false);
            }
        }
    }
    public void destroyMiniMap()
    {
        for(int y = 0; y < generatedGrids.Length; y++)
        {
            for(int x = 0; x < generatedGrids[y].Length; x++)
            {
                Destroy(generatedGrids[y][x]);
            }
        }
        generatedGrids = new GameObject[gridLoader.getYGridLength()][];
    }
    //public functions
    public void recenterMiniMap(int xGrid, int yGrid)
    {
        if (loadedPlayerX == -99 && loadedPlayerY == -99)
        {
            loadedPlayerX = xGrid;
            loadedPlayerY = yGrid;
        }
        generatedSpriteRenderers[loadedPlayerY][loadedPlayerX].color = defualtGridColor;
        generatedSpriteRenderers[yGrid][xGrid].color = playerOnGridColor;
        gameObject.transform.position += gameObject.transform.parent.position - generatedGrids[yGrid][xGrid].transform.position;
        loadedPlayerX = xGrid;
        loadedPlayerY = yGrid;
    }
    public GameObject[][] getMiniMapGridSymbols()
    {
        return generatedGrids;
    }
    // Update is called once per frame
    void Update()
    {  
    }
}
