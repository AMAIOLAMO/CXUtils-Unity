using UnityEngine;
using System.Collections;

namespace CXUtils
{
    public class OtherUsefulFunctions : MonoBehaviour
    {
        ///<summary> Wait Time Type for waiting time </summary>
        public enum WaitTimeType
        {
            WaitForSeconds, WaitForSecondsRealtime, WaitForEndOfFrame, WaitForFixedUpdate
        }

        private IEnumerator _waitTime(float TimeInSec, WaitTimeType waitTimeType)
        {
            if (waitTimeType == WaitTimeType.WaitForSeconds)
                yield return new WaitForSeconds(TimeInSec);

            else if (waitTimeType == WaitTimeType.WaitForSecondsRealtime)
                yield return new WaitForSecondsRealtime(TimeInSec);

            else if (waitTimeType == WaitTimeType.WaitForEndOfFrame)
                yield return new WaitForEndOfFrame();

            else
                yield return new WaitForFixedUpdate();
        }

        ///<summary> Wait's the time using Ienumerator </summary>
        public void WaitTime(float TimeInSec, WaitTimeType waitTimeType) => StartCoroutine(_waitTime(TimeInSec, waitTimeType));
        ///<summary> Stop's the wait Time couroutine </summary>
        public void StopWaitTime() => StopCoroutine("_waitTime");
    }
}

