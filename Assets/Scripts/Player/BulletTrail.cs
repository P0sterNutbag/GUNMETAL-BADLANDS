using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Trail", menuName = "Bullet Trail Config")]
public class BulletTrail : ScriptableObject
{
    public AnimationCurve widthCurve;
    public float time = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient colorGradient;
    public Material material;

    public void SetupTrail(TrailRenderer trailRenderer)
    {
        trailRenderer.widthCurve = widthCurve;
        trailRenderer.time = time;
        trailRenderer.colorGradient = colorGradient;
        trailRenderer.sharedMaterial = material;
        trailRenderer.minVertexDistance = minVertexDistance;
    }
}
