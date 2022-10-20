using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestExtends
{
    public virtual void Talk()
    {
        Debug.Log("TestExtends Talk");
    }

    public void Jump()
    {
        Debug.Log("TestExtends Jump");
    }
}
