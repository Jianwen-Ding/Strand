using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskCreator : MonoBehaviour
{
    [SerializeField]
    public string taskRequest;
    private bool hasCreatedTask = false;
    private singleTaskScript createdTask;
    public void attemptTaskCreate()
    {
        if (!hasCreatedTask)
        {
            createdTask = GameObject.FindObjectOfType<taskListOverall>().addTask(taskRequest);
            hasCreatedTask = true;
        }
    }
    public void attemptSignalTaskComplete()
    {
        if (hasCreatedTask)
        {
            createdTask.completeTask();
            this.enabled = false;
        }
    }
    public virtual void onTrigger(Collider2D col)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTrigger(collision);
        if(collision.tag == "Player")
        {
            attemptTaskCreate();
        }
    }
}
