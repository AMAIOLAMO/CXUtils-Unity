using UnityQuaternion = UnityEngine.Quaternion;
using CXQuaternion = CXUtils.Types.Quaternion;

namespace CXUtils.Unity
{
    public static class CXQuaternionUtils
    {
        public static UnityQuaternion ToUnity( this CXQuaternion value ) => new UnityQuaternion( value.values.x, value.values.y, value.values.z, value.values.w );
        public static CXQuaternion ToCxType( this UnityQuaternion value ) => new CXQuaternion( value.x, value.y, value.z, value.w );
    }
}
