using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class sleepButton : MonoBehaviour
{
    [SerializeField]
    GameObject transitionScreen;
    [SerializeField]
    Image transitionRender;
    [SerializeField]
    float transitionSpeed;
    [SerializeField]
    bool startedTransition = false;
    [SerializeField]
    string nightSceneName;
    public void transitionToNextDay()
    {
        PlayerPrefs.SetInt("daysSpent", PlayerPrefs.GetInt("daysSpent", 0) + 1);
        startedTransition = true;
        transitionScreen.SetActive(true);
        transitionRender = transitionScreen.GetComponent<Image>();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (startedTransition)
        {
            if(transitionRender.color.a + Time.fixedDeltaTime * transitionSpeed > 1)
            {
                transitionRender.color = new Color(transitionRender.color.r, transitionRender.color.g, transitionRender.color.b, 1);
                SceneManager.LoadScene(nightSceneName);
            }
            else
            {
                transitionRender.color = new Color(transitionRender.color.r, transitionRender.color.g, transitionRender.color.b, transitionRender.color.a + Time.fixedDeltaTime * transitionSpeed);
            }
        }
    }
}
