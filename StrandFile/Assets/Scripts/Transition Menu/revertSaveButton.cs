using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revertSaveButton : MonoBehaviour
{
    [SerializeField]
    GameObject getRevert;
    public void revert()
    {
        getRevert.GetComponent<scrapStorer>().revertCurrentScrap();
    }
}
