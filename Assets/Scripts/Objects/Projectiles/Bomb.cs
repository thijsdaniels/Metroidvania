using UnityEngine;
using Objects.Collectables.Items;

/**
 * 
 */
public class Bomb : MonoBehaviour {

	private bool fuseLit;
	public float fuseLength = 5f;

	public Fleeting explosion;

    [HideInInspector]
    public Bombs origin;

	/**
	 * 
	 */
	public void Update()
	{
		if (fuseLit)
        {
            fuseLength -= Time.deltaTime;

			if (fuseLength <= 0)
            {
				Explode();
			}
		}
	}

	/**
	 * 
	 */
	public void LightFuse() {
        fuseLit = true;
	}

	/**
	 * 
	 */
	public void Explode()
	{
        if (origin)
        {
            origin.bombCount--;
        }

		Instantiate(explosion, transform.position, Quaternion.identity);

		Destroy(gameObject);
	}

    /**
     *
     */
    public void OnFlameEnter(Flame flame)
    {
        this.fuseLength = 0f;
    }

    /**
     *
     */
    public void OnExplosionEnter(Explosion explosion)
    {
        this.fuseLength = 0.15f;
    }

}
