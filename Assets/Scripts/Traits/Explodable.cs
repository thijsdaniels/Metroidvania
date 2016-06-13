using UnityEngine;
using System.Collections;

public class Explodable : Damagable
{
    protected override bool Immune(Damager cause)
    {
        return !cause.GetComponent<Explosion>();
    }
}
