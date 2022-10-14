using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public static class ClassExtends
    {
        public static void ClearAllChilds(this Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Debug.Log(transform.GetChild(i).name);
            }
        }
    }
}
