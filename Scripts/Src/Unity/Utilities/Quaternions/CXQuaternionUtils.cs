using UnityQuaternion = UnityEngine.Quaternion;
using CxQuaternion = CXUtils.Types.Quaternion;

namespace CXUtils.Unity
{
    public static class CxQuaternionUtils
    {
        public static UnityQuaternion ToUnity( this CxQuaternion value ) => new UnityQuaternion( value.values.x, value.values.y, value.values.z, value.values.w );
        public static CxQuaternion ToCxType( this UnityQuaternion value ) => new CxQuaternion( value.x, value.y, value.z, value.w );
    }
}