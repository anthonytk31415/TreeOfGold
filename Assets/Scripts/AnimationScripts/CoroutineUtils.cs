using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class CoroutineUtils {




    // in increments, do an action

    public static IEnumerator Lerp(
        float totalTime, 
        float totalFrames, 
        Action<float> action) 
    {
        float time = 0f;
        float timeInterval = totalTime/totalFrames; 
        // by default we use a linear evaluation
        Debug.Log("interval: " + timeInterval + ", totalTime: " + totalTime + "totalFrames: " + totalFrames); 
        while (time < totalTime) {
            Debug.Log("current time: " + time);
            action(time/totalTime);
            yield return new WaitForSeconds(timeInterval);
            time += timeInterval;
        }
        action(1);

    }


}
