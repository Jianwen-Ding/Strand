using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EnterMainMenuScript : MonoBehaviour
{
    [SerializeField]
    private Image transitionImage;
    [SerializeField]
    private bool activated;
    [SerializeField]
    private string mainMenuName;
    [SerializeField]
    private float transitionSpeed;
    public void activate()
    {
        activated = true;
    }
    private void Update()
    {
        if (activated) {
            transitionImage.gameObject.SetActive(true);
            Time.timeScale = (float)0.1;
            if (transitionImage.color.a + Time.fixedDeltaTime * transitionSpeed >= 1)
            {
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1);
                SceneManager.LoadScene(mainMenuName);
            }
            else
            {
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, transitionImage.color.a + Time.fixedDeltaTime * transitionSpeed);
            }
        }
    }
}
