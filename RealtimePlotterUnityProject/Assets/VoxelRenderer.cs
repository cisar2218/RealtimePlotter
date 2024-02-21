using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelRenderer : MonoBehaviour
{
    ParticleSystem system;
    ParticleSystem.Particle[] voxels;
    bool voxelsUpdated = false;

    public int numPoints = 10000;
    public float voxelScale = 0.1f;
    public float scale = 1f;
    public float updateDelay = 0.3f;

    void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private IEnumerator UpdateLoop()
    {
        while (true)
        {
            Init();
            yield return new WaitForSeconds(updateDelay);
        }
    }

    void Init()
    {
        float bound = 10;
        Vector3[] positions = new Vector3[numPoints];
        Color[] colors = new Color[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            positions[i] = new Vector3(
                Random.Range(-bound, bound), Random.Range(-bound/2, bound/2), Random.Range(-bound, bound)
                );
            colors[i] = new Color(Random.value, Random.value, Random.value);
        }

        SetVoxels(positions, colors);
    }

    private void Start()
    {
        StartCoroutine(UpdateLoop());
    }

    void Update()
    {
        if (voxelsUpdated)
        {
            system.SetParticles(voxels, voxels.Length);
            voxelsUpdated = false;
        }
    }

    void SetVoxels(Vector3[] positions, Color[] colors)
    {
        voxels = new ParticleSystem.Particle[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            voxels[i].position = positions[i]*scale;
            voxels[i].startColor = colors[i];
            voxels[i].startSize = voxelScale;
        }

        Debug.Log("Voxel count:" + voxels.Length);
        voxelsUpdated = true;
    }
}
