using UnityEngine;

namespace KeepsakeSDK.Example.Game.Particle.Provider
{
    public class ParticleProvider : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleF;
        [SerializeField] private ParticleSystem _particleS;

        public void SetColorParticles(Color color)
        {
            _particleF.startColor = color;
            _particleS.startColor = color;
        }
    }
}
