using System.Collections;
using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Objects/Shaker3D" )]
    public class Shaker3D : MonoBehaviour
    {
        public void Shake( float maxDuration, float maxIntensity, float minIntensity = 0f, Vector3? center = null, bool intensityFade = true )
        {
            if ( _shakeCoroutine != null )
                StopCoroutine( _shakeCoroutine );

            _shakeCoroutine = StartCoroutine( ShakeInternal( maxDuration, maxIntensity, minIntensity, center, intensityFade ) );
        }

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

        Coroutine _shakeCoroutine;

        IEnumerator ShakeInternal( float maxDuration, float maxIntensity, float minIntensity, Vector3? center, bool intensityFade )
        {
            float duration = maxDuration;
            float intensity = maxIntensity;

            var resultCenter = center ?? Vector3.zero;

            while ( duration > 0f )
            {
                duration -= Time.deltaTime;

                float progress = duration / maxDuration;

                if ( intensityFade )
                    intensity = Mathf.Lerp( minIntensity, maxIntensity, progress );

                var offset = Random.insideUnitSphere * intensity;

                _target.localPosition = resultCenter + offset;
                yield return null;
            }
            // when shake finish, set back to center

            _target.localPosition = resultCenter;
        }
    }
}
