using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AppStateDictionary;

public interface IStateableApp
{
    public System.Enum CurrentState { get; set; }
    void LoadState(System.Enum state);
    public string StateFamilyName { get; }
    public AppStateDictionary States { get; }
}

