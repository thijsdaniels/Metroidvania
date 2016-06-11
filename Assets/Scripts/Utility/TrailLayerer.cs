using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class TrailLayerer : MonoBehaviour
{
    protected TrailRenderer trailRenderer;

    public string sortingLayerName = "Default";
    public int sortingOrder = 0;

    public void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();

        Apply();
    }

    protected void Apply()
    {
        trailRenderer.sortingLayerName = sortingLayerName;
        trailRenderer.sortingOrder = sortingOrder;
    }
}
