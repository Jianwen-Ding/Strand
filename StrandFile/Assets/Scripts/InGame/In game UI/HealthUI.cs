using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    GameObject iconPrefab;
    [SerializeField]
    GameObject[] healthIconList = new GameObject[0];
    [SerializeField]
    Animator[] healthAnimatorList;
    [SerializeField]
    float xDiffrenceHealthPlacement;
    public void setUpHealthIcons(int maxHealth, int currentHealth)
    {
        healthIconList = new GameObject[maxHealth];
        healthAnimatorList = new Animator[maxHealth];
        for(int i = 0; i < maxHealth; i++)
        {
            healthIconList[i] = Instantiate(iconPrefab, new Vector3(gameObject.transform.position.x + xDiffrenceHealthPlacement * i, gameObject.transform.position.y, 0), Quaternion.identity.normalized);
            healthAnimatorList[i] = healthIconList[i].GetComponent<Animator>();
            healthIconList[i].transform.SetParent(transform.parent);
        }
        updateHealthIconList(currentHealth);
    }
    public void updateHealthIconList(int currentHealth)
    {
        for(int i = 0; i < healthAnimatorList.Length; i++)
        {
            if(currentHealth >= i + 1)
            {
                healthAnimatorList[i].ResetTrigger("DamageHeart");
                healthAnimatorList[i].SetTrigger("HealHeart");
            }
            else
            {
                healthAnimatorList[i].ResetTrigger("HealHeart");
                healthAnimatorList[i].SetTrigger("DamageHeart");
            }
        }
    }
}
