using System.Collections;
using CXUtils.Common;
using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Objects/Shaker2D" )]
    public class Shaker2D : MonoBehaviour
    {
        public Coroutine Shake( float duration, float intensity, Vector2? center = null ) =>
            StartCoroutine( ShakeInternal( duration, intensity, center ) );

        public Coroutine ShakeFade( float maxDuration, float maxIntensity, float minIntensity = 0f, Vector2? center = null ) =>
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

        IEnumerator ShakeInternal( float duration, float intensity, Vector2? center )
        {
            Vector3 resultCenter = center ?? Vector2.zero;

            while ( duration > 0f )
            {
                duration -= Time.deltaTime;

                Vector3 offset = Random.insideUnitCircle * intensity;

                _target.localPosition = resultCenter + offset;
                yield return null;
            }
            // when shake finish, set back to center

            _target.localPosition = resultCenter;
        }

        IEnumerator ShakeFadeInternal( float maxDuration, float maxIntensity, float minIntensity = 0f, Vector2? center = null )
        {
            float duration = maxDuration;

            Vector3 resultCenter = center ?? Vector2.zero;

            while ( duration > 0f )
            {
                duration -= Time.deltaTime;

                float progress = duration / maxDuration;

                float intensity = Tween.Lerp( minIntensity, maxIntensity, progress );

                Vector3 offset = Random.insideUnitCircle * intensity;

                _target.localPosition = resultCenter + offset;
                yield return null;
            }
            // when shake finish, set back to center

            _target.localPosition = resultCenter;
        }
    }
}
