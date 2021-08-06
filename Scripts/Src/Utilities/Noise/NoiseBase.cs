using CXUtils.Types;

namespace CXUtils.CodeUtils
{
    public interface IGradientNoiseBase<T>
    {
        /// <summary>
        /// Samples a simplex noise output from <paramref name="value"/>
        /// </summary>
        public float Sample(T value);
    }

    public interface IGradientNoise1D : IGradientNoiseBase<float>  { }
    public interface IGradientNoise2D : IGradientNoiseBase<Float2> { }
    public interface IGradientNoise3D : IGradientNoiseBase<Float3> { }
    public interface IGradientNoise4D : IGradientNoiseBase<Float4> { }
}