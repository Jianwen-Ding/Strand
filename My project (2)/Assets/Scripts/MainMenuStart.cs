using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuStart : MonoBehaviour
{
    [SerializeField]
    string nextScene;
    [SerializeField]
    bool activated;
    [SerializeField]
    Image transitionImage;
    [SerializeField]
    float transitionSpeed;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            activated = true;
        }
        if (activated)
        {
            transitionImage.gameObject.SetActive(true);
            if (transitionImage.color.a + Time.fixedDeltaTime * transitionSpeed >= 1)
            {
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1);
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, transitionImage.color.a + Time.fixedDeltaTime * transitionSpeed);
            }
        }
    }
}
