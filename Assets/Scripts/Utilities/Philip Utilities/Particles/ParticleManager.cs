using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    [System.Serializable]
    public class ParticleEffect
    {
        [field: SerializeField] public string Key { private set; get; }
        [field: SerializeField] public ParticleSystem ParticleSystem { private set; get; }
    }

    public ParticleEffect[] ParticleEffects { private set; get; } = new ParticleEffect[0];

    public ParticleEffect FindParticleEffect(string key)
    {
        foreach (ParticleEffect effect in ParticleEffects)
        {
            if(effect.Key == key)
            {
                return effect;
            }
        }

        return null;
    }

    public ParticleSystem FindParticleSystem(string key)
    {
        foreach (ParticleEffect effect in ParticleEffects)
        {
            if(effect.Key == key)
            {
                return effect.ParticleSystem;
            }
        }

        return null;
    }

}
