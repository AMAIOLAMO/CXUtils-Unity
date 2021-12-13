using System.Collections;
using UnityEngine;

namespace CXUtils.Components
{
    [AddComponentMenu( "CXUtils/Objects/Shaker2D" )]
    public class Shaker2D : MonoBehaviour
    {
        public void Shake( float maxDuration, float maxIntensity, float minIntensity, Vector2? center = null, bool intensityFade = true )
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

        IEnumerator ShakeInternal( float maxDuration, float maxIntensity, float minIntensity, Vector2? center, bool intensityFade )
        {
            float duration = maxDuration;
            float intensity = maxIntensity;

            Vector3 resultCenter = center ?? Vector2.zero;

            while ( duration > 0f )
            {
                duration -= Time.deltaTime;

                float progress = duration / maxDuration;

                if ( intensityFade )
                    intensity = Mathf.Lerp( minIntensity, maxIntensity, progress );

                Vector3 offset = Random.insideUnitCircle * intensity;

                _target.localPosition = resultCenter + offset;
                yield return null;
            }
            // when shake finish, set back to center

            _target.localPosition = resultCenter;
        }
    }
}
