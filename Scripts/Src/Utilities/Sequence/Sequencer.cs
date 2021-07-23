using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CXUtils.CodeUtils
{
    /// <summary>
    ///     A class that is used to enumerate through triggers / <see cref="Action" />
    /// </summary>
    public class Sequencer : IEnumerable<Action>
    {
        readonly List<Action> sequenceTriggerList;

        public Sequencer() => sequenceTriggerList = new List<Action>();
        public IEnumerator<Action> GetEnumerator() => new SequencerEnumerator( sequenceTriggerList );
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Sequencer Append( Action trigger )
        {
            sequenceTriggerList.Add( trigger );
            return this;
        }

        class SequencerEnumerator : IEnumerator<Action>
        {
            readonly ReadOnlyCollection<Action> actionCollection;
            Queue<Action> actionQueue;
            public SequencerEnumerator( IList<Action> actions )
            {
                actionCollection = new ReadOnlyCollection<Action>( actions );
                actionQueue = new Queue<Action>( actions );
            }
            public bool MoveNext() => ( Current = actionQueue.Dequeue() ) != null;
            public void Reset() => actionQueue = new Queue<Action>( actionCollection );
            public Action Current { get; private set; }
            object IEnumerator.Current => Current;
            public void Dispose() { }
        }
    }
}
