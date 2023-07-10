using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXLightFade : MonoBehaviour
    {
        [SerializeField] private Light _light;

        [Header("Seconds to dim the light")]
        public float life = 0.2f;
        public bool killAfterLife = true;

        private float initIntensity;

        void Start()
        {
            initIntensity = _light.intensity;
        }

        void Update()
        {
            if(_light == null)
                return;

            _light.intensity -= initIntensity * (Time.deltaTime / life);

            if (killAfterLife && _light.intensity <= 0)
                Destroy(_light);
        }
    }
}