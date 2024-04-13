using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditDebug
{
    // Start is called before the first frame update
    public static void DebugIter(IEnumerable myIter){
        string debugMsg = ""; 
        foreach (var item in myIter){
            debugMsg += item; 
        }
        Debug.Log(debugMsg);
    }
}

