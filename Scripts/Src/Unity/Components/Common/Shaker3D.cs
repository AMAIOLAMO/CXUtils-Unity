using System.Collections;
using CXUtils.Common;
using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Objects/Shaker3D" )]
    public class Shaker3D : MonoBehaviour
    {
        public Coroutine Shake( float duration, float intensity, Vector3? center = null ) =>
            StartCoroutine( ShakeInternal( duration, intensity, center ) );

        public Coroutine ShakeFade( float maxDuration, float maxIntensity, float minIntensity = 0f, Vector3? center = null ) =>
            StartCoroutine( ShakeFadeInternal( maxDuration, maxIntensity, minIntensity, center ) );

        void Awake()
        {
            if ( _target == null )
                _target = transform;
        }

        public Transform Target
        {
            get => _target;
            set => _target = value;
        }

        [SerializeField] Transform _target;

        IEnumerator ShakeInternal( float duration, float intensity, Vector3? center )
        {
            var resultCenter = center ?? Vector3.zero;

            while ( duration > 0f )
            {
                duration -= Time.deltaTime;

                var offset = Random.insideUnitSphere * intensity;

                _target.localPosition = resultCenter + offset;
                yield return null;
            }
            // when shake finish, set back to center

            _target.localPosition = resultCenter;
        }

        IEnumerator ShakeFadeInternal( float maxDuration, float maxIntensity, float minIntensity = 0f, Vector3? center = null )
        {
            float duration = maxDuration;

            var resultCenter = center ?? Vector3.zero;

            while ( duration > 0f )
            {
                duration -= Time.deltaTime;

                float progress = duration / maxDuration;

                float intensity = Tween.Lerp( minIntensity, maxIntensity, progress );

                var offset = Random.insideUnitSphere * intensity;

                _target.localPosition = resultCenter + offset;
                yield return null;
            }
            // when shake finish, set back to center

            _target.localPosition = resultCenter;
        }
    }
}
