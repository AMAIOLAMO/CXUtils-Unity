using System;
using System.Collections.Generic;

namespace CXUtils.CodeUtils
{
    public class CXSequence
    {
        readonly List<Action> _sequenceTriggerList;

        int _sequenceIndex;

        public CXSequence()
        {
            _sequenceTriggerList = new List<Action>();
        }

        public CXSequence Append( Action trigger )
        {
            _sequenceTriggerList.Add( trigger );
            return this;
        }

        /// <summary>
        /// triggers to the next trigger, will return true if sequence ended
        /// </summary>
        public bool NextTrigger()
        {
            if ( _sequenceIndex >= _sequenceTriggerList.Count )
                return true;
            
            _sequenceTriggerList[_sequenceIndex]?.Invoke();
            return false;
        }

        public void Reset()
        {
            _sequenceIndex = 0;
        }
    }
}
