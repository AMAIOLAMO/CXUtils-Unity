using System.Collections;
using UnityEngine;
using System;

namespace CXUtils.EventUtils
{
    /// <summary> A simple story event </summary>
    [Serializable]
    public struct StoryEvent<T>
    {
        #region Vars and fields
        [SerializeField] private static float waitTimeBefore, waitTimeAfter;
        [SerializeField] public static event EventHandler<T> Event_BeforeAction, Event_AfterAction;
        [SerializeField] public static event Action Event_MainAction;

        public static float WaitTimeBefore { get => waitTimeBefore; set => waitTimeBefore = value; }
        public static float WaitTimeAfter { get => waitTimeAfter; set => waitTimeAfter = value; }
        #endregion

        #region ClassConstructors
        /// <summary> A simple story event </summary>
        /// <param name="event_MainAction">The main action of the event</param>
        /// <param name="event_BeforeAction">The event before the action</param>
        /// <param name="event_AfterAction">the event after the action</param>
        /// <param name="waitTimeBefore">The time needed to wait before all actions and events</param>
        /// <param name="waitTimeAfter">The time needed to wait after all actions and events</param>
        public StoryEvent(Action event_MainAction = null, EventHandler<T> event_BeforeAction = null, EventHandler<T> event_AfterAction = null,
        float waitTimeBefore = 0, float waitTimeAfter = 0) =>
            SetInit(event_MainAction, event_BeforeAction, event_AfterAction, waitTimeBefore, waitTimeAfter);

        #endregion

        #region ScriptsMethods(Public)

        #region InvokeMethods
        /// <summary> Invokes the story event </summary>
        /// <param name="sender_Before">The sender obj for the before action event</param>
        /// <param name="e1">The eventArgument for the before action event</param>
        /// <param name="sender_After">The sender obj for the after action event</param>
        /// <param name="e2">The eventArgument for the after action event</param>
        /// <param name="isRealTime">If using real time</param>
        public IEnumerator Invoke(object sender_Before, T e1, object sender_After, T e2, bool isRealTime = false)
        {
            if (isRealTime)
            {
                yield return new WaitForSecondsRealtime(waitTimeBefore);
                TriggerNoTime(sender_Before, e1, sender_After, e2);
                yield return new WaitForSecondsRealtime(waitTimeAfter);
            }
            else
            {
                yield return new WaitForSeconds(waitTimeBefore);
                TriggerNoTime(sender_Before, e1, sender_After, e2);
                yield return new WaitForSeconds(waitTimeAfter);
            }
        }
        
        /// <summary> Invokes the coroutine on another monoBehaviour </summary>
        /// <param name="monBehav">The host of this invoke method to invoke on</param>
        /// <param name="sender">The global sender</param>
        /// <param name="isRealTime">Using real time or not</param>
        public void Invoke(MonoBehaviour monBehav, object sender, T e, bool isRealTime = false) =>
            monBehav.StartCoroutine(Invoke(sender, e, sender, e, isRealTime));

        /// <summary> Trigger's the event with no waiting time </summary>
        /// <param name="sender_Before">The sender obj for the before action event</param>
        /// <param name="e1">The eventArgument for the before action event</param>
        /// <param name="sender_After">The sender obj for the after action event</param>
        /// <param name="e2">The eventArgument for the after action event</param>
        public static void TriggerNoTime(object sender_Before, T e1, object sender_After, T e2)
        {
            Event_BeforeAction?.Invoke(sender_Before, e1);
            Event_MainAction?.Invoke();
            Event_AfterAction?.Invoke(sender_After, e2);
        }
        #endregion

        #endregion

        #region ScriptsMethods(Private)
        private static void SetInit(Action event_MainAction, EventHandler<T> event_BeforeAction, EventHandler<T> event_AfterAction,
        float waitTimeBefore, float waitTimeAfter) =>
            (Event_MainAction, Event_BeforeAction, Event_AfterAction, WaitTimeBefore, WaitTimeAfter) =
            (event_MainAction, event_BeforeAction, event_AfterAction, waitTimeBefore, waitTimeAfter);

        #endregion
    }
}

