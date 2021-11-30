using UnityEngine;

public class HealingParticle : MonoBehaviour
{
    private ParticleSystem[] particleSystems = null; 

    private void Awake() => particleSystems = GetComponentsInChildren<ParticleSystem>();

    public void PlayParticles() 
    {
        foreach(ParticleSystem particleSystem in particleSystems)
        {
            Debug.Log("Starting particle systems -> " + particleSystem);
            if (!particleSystem.isPlaying)
                particleSystem.Play();
        }
    }

    public void StopParticles()
    {
        foreach(ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem.isPlaying)
                particleSystem.Stop();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
            FindObjectOfType<AudioManager>().Play("Powerup");
    }
}
