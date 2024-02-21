using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VoxelRenderer : MonoBehaviour
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

    public UpdateMode mode;

    Vector3[] lastPositions;
    Color[] lastColors;

    public enum UpdateMode
    {
        UpdateWhole,
        IncremenetalAdding
    }

    void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private IEnumerator UpdateLoop()
    {
        bool isUpdating = true;
        while (isUpdating)
        {
            switch (mode)
            {
                case UpdateMode.UpdateWhole:
                    Init();
                    yield return new WaitForSeconds(updateDelay);
                    break;
                case UpdateMode.IncremenetalAdding:
                    voxelsCount += pointPerSec;
                    if (voxelsCount > numPoints)
                    {
                        isUpdating = false;
                        break;
                    }
                    Debug.Log(voxelsCount);
                    Init(voxelsCount);
                    yield return new WaitForSeconds(1.0f);
                    break;
                default:
                    Debug.LogWarning("This {nameof(RenderMode)} not handled");
                    isUpdating = false;
                    break;
            }
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
                Random.Range(-bound, bound), Random.Range(-bound / 2, bound / 2), Random.Range(-bound, bound)
                );
            colors[i] = new Color(Random.value, Random.value, Random.value);
        }

        SetVoxels(positions, colors);
    }

    void Init(int howMany)
    {
        float bound = 10;
        Vector3[] positions = new Vector3[howMany];
        Color[] colors = new Color[howMany];

        for (int i = 0; i < howMany; i++)
        {
            if (lastPositions != null && i < lastPositions.Length)
            {
                positions[i] = lastPositions[i];
                colors[i] = lastColors[i];
            }
            else
            {
                positions[i] = new Vector3(
                    Random.Range(-bound, bound), Random.Range(-bound/2, bound/2), Random.Range(-bound, bound)
                    );
                float intensity = Mathf.Clamp((float)voxelsCount/(float)numPoints, 0.0f,1.0f);
                Debug.Log("Red val:" + intensity);
                colors[i] = new Color(intensity, 0.0f, 0.0f);
            }
        }

        lastPositions = positions; lastColors = colors;

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
