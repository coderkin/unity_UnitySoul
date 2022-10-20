using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : TestExtends,TestInterface,TestInterface2
{
    public override void Talk()
    {
        base.Talk();
        Debug.Log("Test");
    }

    public void Walk()
    {
        Debug.Log("TestInterface Walk");
    }

    public void Jump()
    {
        Debug.Log("Test Jump");
    }
}
