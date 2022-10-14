using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CF
{
    public class IllusionaryWall : MonoBehaviour
    {
        public Material material;

        public void DisappaerWall()
        {
            StartCoroutine(DestoryWall());
        }

        private void Awake()
        {
            Color materialColor = material.color;
            materialColor.a = 1;
            material.color = materialColor;
        }

        private IEnumerator DestoryWall()
        {
            Color materialColor = material.color;

            while(materialColor.a > 0)
            {
                materialColor.a -= Time.deltaTime;
                material.color = materialColor;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
