using System;

namespace CXUtils.Components
{
	public interface ITickManager<in T>
	{
		/// <summary>
		///     Set's the tick to the given <paramref name="tick" />
		/// </summary>
		void Set(int tick);

		/// <summary>
		///     Set's the <see cref="TickManager{T}.Current" /> to 0 and returns the last tick
		/// </summary>
		int Reset();

		/// <summary>
		///     Ticks the Ticker using <paramref name="delta" />
		/// </summary>
		bool TickMax(T delta, T max);


		event Action<int> OnTicked;

		int Current { get; }
	}
}
