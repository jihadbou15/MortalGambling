using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public delegate void PhaseHandler();
    public event PhaseHandler OnPhaseEnd;

    public void Initialize()
    {

    }

    public void Tick()
    {

    }


}
