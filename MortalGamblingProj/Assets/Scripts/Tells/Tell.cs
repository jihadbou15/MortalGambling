using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tell : MonoBehaviour
{
    //float time in seconds, Vector3 Position
    public struct KeyFrame
    {
        public float tick;
        public Vector3 position;
        public Quaternion rotation;
    }
    
    [SerializeField] private List<KeyFrame> timeline = new List<KeyFrame>();
    private float timer = 0;
    private int currentFrame = 0;
    private bool isRunning = false;
    public new Transform transform = null;

    public void Initialize(List<KeyFrame> newTimeline, Transform newTransform)
    {
        timeline = newTimeline;
        transform = newTransform;
    }

    public void Tick()
    {
        if(isRunning)
        {
            UpdateFrame();
            ApplyFrame();
        }

    }

    private void ApplyFrame()
    {
        float delta = timeline[currentFrame].tick - timeline[currentFrame - 1].tick;
        float percentageintoframe = timer - timeline[currentFrame - 1].tick;
        transform.localPosition = Vector3.Lerp(transform.localPosition, timeline[currentFrame].position, percentageintoframe / delta);
    }

    private void UpdateFrame()
    {
        timer += Time.deltaTime;
        if((currentFrame+1) >= timeline.Count)
        if (timer > timeline[currentFrame + 1].tick) currentFrame++;
    }

    public void play()
    {
        isRunning = true;
    }
}
