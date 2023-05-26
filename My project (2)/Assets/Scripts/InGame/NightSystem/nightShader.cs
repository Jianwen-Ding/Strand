using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nightShader : MonoBehaviour
{
    //It starts at morning shade and moves to evening shade at the mid point at which point it goes to night shade
    [SerializeField]
    private Color morningShade;
    //Stores speed of transition
    //[R, B, G, A]
    [SerializeField]
    private Vector4 morToEveTransitionSpeed;
    [SerializeField]
    private Color eveningShade;
    //Stores speed of transition
    //[R, B, G, A]
    [SerializeField]
    private Vector4 eveToNighTransitionSpeed;
    [SerializeField]
    private Color nightShade;
    //Setup variable
    [SerializeField]
    private Image render;
    // Start is called before the first frame update
    void Start()
    {
        render = gameObject.GetComponent<Image>();
        float halfTime = (nightSystem.getTimeUntilNight() / 2);
        //Sets transition speed based on time and color points
        morToEveTransitionSpeed.x = (eveningShade.r - morningShade.r) / halfTime;
        morToEveTransitionSpeed.y = (eveningShade.g - morningShade.g) / halfTime;
        morToEveTransitionSpeed.z = (eveningShade.b - morningShade.b) / halfTime;
        morToEveTransitionSpeed.w = (eveningShade.a - morningShade.a) / halfTime;
        eveToNighTransitionSpeed.x = (nightShade.r - eveningShade.r) / halfTime;
        eveToNighTransitionSpeed.y = (nightShade.g - eveningShade.g) / halfTime;
        eveToNighTransitionSpeed.z = (nightShade.b - eveningShade.b) / halfTime;
        eveToNighTransitionSpeed.w = (nightShade.a - eveningShade.a) / halfTime;
        //Sets color to morning
        render.color = morningShade;
    }

    // Update is called once per frame
    void Update()
    {
        //Changes color
        float currentTime = nightSystem.getCurrentTimePassed();
        float maxTime = nightSystem.getTimeUntilNight();
        if (currentTime < maxTime / 2)
        {
            render.color = new Color(render.color.r + morToEveTransitionSpeed.x * Time.deltaTime, render.color.g + morToEveTransitionSpeed.y * Time.deltaTime, render.color.b + morToEveTransitionSpeed.z * Time.deltaTime, render.color.a + morToEveTransitionSpeed.w * Time.deltaTime);
        }
        else if(currentTime < maxTime)
        {
            render.color = new Color(render.color.r + eveToNighTransitionSpeed.x * Time.deltaTime, render.color.g + eveToNighTransitionSpeed.y * Time.deltaTime, render.color.b + eveToNighTransitionSpeed.z * Time.deltaTime, render.color.a + eveToNighTransitionSpeed.w * Time.deltaTime);
        }
        else
        {
            render.color = nightShade;
        }
    }
}
