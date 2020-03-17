using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class EdenAcademy : Academy
{
    public float CatchRadius{get;private set;}

    public override void InitializeAcademy()
        {
            CatchRadius=0f;

            FloatProperties.RegisterCallback("catch_radius",f=>
            {
                CatchRadius=f;
            });
        }
}
