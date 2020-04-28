using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Tell : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    public void Initialize(PlayableDirector director)
    {
        this.director = director;
        director.extrapolationMode = DirectorWrapMode.Loop;
    }

    public void Tick()
    {

    }


    public void play()
    {
        director.Play();
    }
}
