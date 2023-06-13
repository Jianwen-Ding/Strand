using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class nightToGameTransition : MonoBehaviour
{
    [SerializeField]
    string sceneKey;
    [SerializeField]
    bool activated;
    SpriteRenderer mask;
    public void startTransition()
    {
        Time.timeScale = 1;
        mask = GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<SpriteRenderer>();
        activated = true;
    }
    private void Update()
    {
        if (activated)
        {
            if(mask.color.a + Time.deltaTime > 1)
            {
                mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, 1);
                SceneManager.LoadScene(sceneKey);
            }
            mask.color = new Color(mask.color.r, mask.color.g, mask.color.b, mask.color.a + Time.deltaTime);
        }
    }
}
