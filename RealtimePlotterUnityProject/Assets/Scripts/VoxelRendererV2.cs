using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class VoxelRenderer2 : MonoBehaviour
{
    ParticleSystem system;
    ParticleSystem.Particle[] voxels;
    bool voxelsUpdated = false;

    public int voxelsCount = 0;
    public int numPoints = 10000;
    public int pointPerSec = 20;
    public float voxelScale = 0.1f;
    public float scale = 1f;
    public float updateDelay = 0.3f;

    public float bound = 10.0f;


    void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private IEnumerator UpdateLoop()
    {
        InitVoxels();
        SetVoxels();
        bool isUpdating = true;
        while (isUpdating)
        {
            // TODO
            yield return new WaitForSeconds(updateDelay);
        }
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
            Debug.Log("Update called");
        }
    }

    void InitVoxels()
    {
        voxels = new ParticleSystem.Particle[numPoints];

        Vector3[] positions = new Vector3[numPoints];
        Color[] colors = new Color[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            voxels[i].position = new Vector3(
                Random.Range(-bound, bound), Random.Range(-bound / 2, bound / 2), Random.Range(-bound, bound)
                );
            positions[i] = new Vector3(
                Random.Range(-bound, bound), Random.Range(-bound / 2, bound / 2), Random.Range(-bound, bound)
                );
            float intensity = Mathf.Clamp((float)voxelsCount / (float)numPoints, 0.0f, 1.0f);
            colors[i] = new Color(intensity, 0.0f, 0.0f);
            voxels[i].startColor = colors[i];
            voxels[i].startSize = voxelScale;
        }
    }

    void SetVoxels()
    {
        foreach (var voxel in voxels)
        {
            // UPDATE VOXEL HERE
        }

        Debug.Log("Voxel count:" + voxels.Length);
        voxelsUpdated = true;
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(bound, bound, bound));
    }
}
