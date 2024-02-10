using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskListOverall : MonoBehaviour
{
    [SerializeField]
    GameObject singleTaskDisplayPrefab;
    [SerializeField]
    List<task> taskList = new List<task>();
    [SerializeField]
    Vector3 initialPosAdjust;
    [SerializeField]
    Vector3 distanceBetweenTaskDisplays;
    private class task
    {
        public GameObject taskGameObject;
        public singleTaskScript taskScript;
        public task(taskListOverall overallList, GameObject taskPrefab,string givenTask)
        {
            taskGameObject = Instantiate(taskPrefab, new Vector3(0, 0, 0), Quaternion.identity.normalized);
            taskGameObject.transform.SetParent(overallList.gameObject.transform.parent);
            taskScript = taskGameObject.GetComponent<singleTaskScript>();
            taskScript.setUp(givenTask, overallList);
        }
    }
    private task doesContainGameObject(List<task> tasks, GameObject findObject)
    {
        for(int i = 0; i < tasks.Count ; i++)
        {
            if(findObject == tasks[i].taskGameObject)
            {
                return tasks[i];
            }
        }
        return null;
    }
    private void repositionTasks()
    {
        for(int i = 0; i < taskList.Count; i++)
        {
            taskList[i].taskGameObject.transform.position = initialPosAdjust + gameObject.transform.position + distanceBetweenTaskDisplays * i;
        }
    }
    public void takeOffTask(GameObject taskObject)
    {
        task attemtToFindObject = doesContainGameObject(taskList, taskObject);
        if (attemtToFindObject != null)
        {
            taskList.Remove(attemtToFindObject);
            Destroy(attemtToFindObject.taskGameObject);
        }
        repositionTasks();
    }
    // Returns the 
    public singleTaskScript addTask(string givenTask)
    {
        task newTask = new task(this, singleTaskDisplayPrefab, givenTask);
        taskList.Add(newTask);
        repositionTasks();
        return newTask.taskScript;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
