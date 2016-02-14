using UnityEngine;

public class Fleeting : MonoBehaviour
{
    public enum Mode
    {
        Animator,
        ParticleSystem
    }

    public Mode mode = Mode.Animator;

    new private ParticleSystem particleSystem;

    public void Start()
    {
        if (mode.Equals(Mode.ParticleSystem))
        {
            this.particleSystem = this.GetComponent<ParticleSystem>();

            if (!particleSystem)
            {
                Debug.LogWarning("Fleeting mode ParticleSystem requires a ParticleSystem component.");
            }
        }
        else if (mode.Equals(Mode.Animator))
        {
            if (!this.GetComponent<Animator>())
            {
                Debug.LogWarning("Fleeting mode Animator requires a Animator component.");
            }
        }
    }

    public void Update()
    {
        if (mode.Equals(Mode.ParticleSystem) && this.particleSystem)
        {
            if (!this.particleSystem.IsAlive())
            {
                Destroy(this.gameObject);
            }
        }
    }

	public void OnAnimationEnd()
	{
		Destroy(this.gameObject);
	}
}
