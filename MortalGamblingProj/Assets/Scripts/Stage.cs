using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stage : MonoBehaviour
{
    bool active;

    public delegate void TransitionCallBack(string id);
    public delegate void ExecuteDelegate();

    struct StageData
    {
        public string id;
        public TransitionCallBack transition;
        public Stage StageToTransition;
    }

    List<StageData> transitions;
    public ExecuteDelegate execute; 

    void Initialize(List<StageData> transitions, ExecuteDelegate execution)
    {
        execute = execution;
        this.transitions = transitions;
        transitions.ForEach(delegate (StageData stageData)
        {
            stageData.transition += Transition;
        });
    }

    public void SetActivate(bool isActive)
    {
        active = isActive;
        ExecuteStage();
    }

    public void Transition(string id)
    {
        if(active)
        {
            StageData data = transitions.Find(temp => temp.id == id);
            data.StageToTransition.SetActivate(true);
            SetActivate(false);
        }
    }

    public void ExecuteStage() 
    {
        execute.Invoke();
    }

}
