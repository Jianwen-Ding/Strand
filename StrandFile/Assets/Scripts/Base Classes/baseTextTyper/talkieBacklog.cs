using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkieBacklog : textTyper
{
    [SerializeField]
    float untilNextText;
    [SerializeField]
    float untilNextTextLeft;
    [SerializeField]
    bool deactivated = true;
    List<string> talkList = new List<string>();

    public bool getDeactivated()
    {
        return deactivated;
    }

    public void addToTalk(string add)
    {
        talkList.Add(add);
        if (deactivated)
        {
            setText(add);
            deactivated = false;
        }
    }
    public override void Start()
    {
        base.Start();
        untilNextTextLeft = untilNextText;
    }
    public override void Update()
    {
        if (!deactivated)
        {
            base.Update();
            if (hasCompletedText())
            {
                untilNextTextLeft -= Time.deltaTime;
                if (untilNextTextLeft <= 0)
                {
                    untilNextTextLeft = untilNextText;
                    talkList.RemoveAt(0);
                    if (talkList.Count <= 0)
                    {
                        deactivated = true;
                    }
                    else
                    {
                        setText(talkList[0]);
                    }
                }
            }
        }
    }
}
