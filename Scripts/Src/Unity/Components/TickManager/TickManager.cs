using System;
using CXUtils.Common;

namespace CXUtils.Components
{
	/// <summary>
	///     A basic ticker system
	/// </summary>
	public class TickManager<T> : ITickManager<T>
	{
		public TickManager(ITicker<T> ticker) => this.ticker = ticker;

		public int Current { get; private set; }

		public event Action<int> OnTicked;

		public void Set(int tick) => Current = tick;

		public int Reset()
		{
			int last = Current;
			Current = 0;
			return last;
		}

		public bool TickMax(T delta, T max)
		{
			if (!ticker.TickMax(delta, max)) return false;

			Current++;
			OnTicked?.Invoke(Current);
			return true;
		}
		readonly ITicker<T> ticker;
	}
}
