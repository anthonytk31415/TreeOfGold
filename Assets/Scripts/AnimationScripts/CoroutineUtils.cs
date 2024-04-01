using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class CoroutineUtils {

    public static IEnumerator Lerp(
        float totalTime, 
        int totalFrames, 
        Action<float> action) 
    {
        float time = 0;
        float timeInterval = totalTime/totalFrames; 
        // by default we use a linear evaluation
        Debug.Log("interval: " + timeInterval + ", totalTime: " + totalTime + "totalFrames: " + totalFrames); 
        while (time <= totalTime) {
            action(time/totalTime);
            yield return new WaitForSeconds(timeInterval);
            time += timeInterval;
        }
    }


}
