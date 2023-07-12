using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RieslingUtils {
    public static class ExVector {
        public static Vector3 Jiggle(this Vector3 origin, float maxAmount) {
            float xAmount = Random.Range(-maxAmount, maxAmount);
            float yAmount = Random.Range(-maxAmount, maxAmount);

            Vector3 newVector = origin + new Vector3(xAmount, yAmount);
            return newVector;
        }        
    }
}