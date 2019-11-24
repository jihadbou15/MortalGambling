using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void InputHandler(KeyCode keyCode);
    public event InputHandler KeyDown;

    [SerializeField] private List<KeyCode> _keyCodes = new List<KeyCode>();
    public void Initialize()
    {
        
    }

    public void Tick()
    {
        foreach(KeyCode keyCode in _keyCodes)
        {
            if(Input.GetKeyDown(keyCode))
            {
                KeyDown?.Invoke(keyCode);
            }
        }
    }
}
