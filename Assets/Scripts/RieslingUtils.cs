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

        public static Vector3 ChangeXPos(this Vector3 origin, float xValue) {
            Vector3 newVector = new Vector3(xValue, origin.y, origin.z);
            return newVector;
        }

        public static Vector3 ChangeYPos(this Vector3 origin, float yValue) {
            Vector3 newVector = new Vector3(origin.x, yValue, origin.z);
            return newVector;
        }

        public static Vector3 ChangeZPos(this Vector3 origin, float zValue) {
            Vector3 newVector = new Vector3(origin.x, origin.y, zValue);
            return newVector;
        }

        public static Vector3 GetMouseWorldPosition() {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;
            return worldPosition;
        }
    }
}