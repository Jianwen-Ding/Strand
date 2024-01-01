using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class textTyper : MonoBehaviour
{
    [SerializeField]
    string givenText;
    TextMeshProUGUI objectText;
    AudioSource objectAudio;
    AudioScaler objectAudioScale;
    [SerializeField]
    float timePerWordBase;
    float timePerWordExpressed;
    [SerializeField]
    float baseVolume;
    float volumeExpressed;
    float timePerWordLeft = 0;
    int currentIndex = 0;
    bool hasCompleted = false;
    public virtual void setText(string set)
    {
        givenText = set;
        clearText();
    }

    public virtual void setTextStart()
    {
    }

    public void clearText()
    {
        timePerWordExpressed = timePerWordBase;
        volumeExpressed = baseVolume;
        objectAudioScale.setVolume(baseVolume);
        timePerWordLeft = 0;
        currentIndex = 0;
        hasCompleted = false;
        objectText.text = "";
    }

    public bool hasCompletedText()
    {
        return hasCompleted;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        objectAudio = gameObject.GetComponent<AudioSource>();
        objectAudioScale = gameObject.GetComponent<AudioScaler>();
        objectText = gameObject.GetComponent<TextMeshProUGUI>();
        objectText.text = "";
        clearText();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!hasCompleted)
        {
            timePerWordLeft -= Time.deltaTime;
            if (timePerWordLeft <= 0)
            {
                int indexAdvance = 0;
                switch (givenText[currentIndex])
                {
                    default:
                        objectAudio.Play();
                        objectText.text = objectText.text + givenText[currentIndex];
                        currentIndex++;
                        timePerWordLeft = timePerWordExpressed;
                        break;
                    case '%':
                        objectText.text = objectText.text + "\n";
                        indexAdvance = 1;
                        break;
                    case ' ':
                        timePerWordLeft = timePerWordExpressed;
                        objectText.text = objectText.text + givenText[currentIndex];
                        indexAdvance = 1;
                        break;
                    case '-':
                        timePerWordLeft = timePerWordExpressed;
                        indexAdvance = 1;
                        break;
                    case '|':
                        string subStringLeft2 = givenText.Substring(currentIndex + 1);
                        timePerWordExpressed = float.Parse(subStringLeft2.Substring(0, subStringLeft2.IndexOf("|")));
                        indexAdvance = subStringLeft2.IndexOf("|") + 2;
                        break;
                    case '^':
                        string subStringLeft1 = givenText.Substring(currentIndex + 1);
                        volumeExpressed = float.Parse(subStringLeft1.Substring(0, subStringLeft1.IndexOf("^")));
                        objectAudioScale.setVolume(volumeExpressed);
                        indexAdvance = subStringLeft1.IndexOf("^") + 2;
                        break;
                }
                currentIndex += indexAdvance;
                if (currentIndex >= givenText.Length)
                {
                    hasCompleted = true;;
                }
            }
        }
    }
}
