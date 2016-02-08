using UnityEngine;
using System.Collections;

public class Explodable : Damagable
{
    protected override bool Immune(GameObject cause)
    {
        return !cause.GetComponent<Explosion>();
    }
}
