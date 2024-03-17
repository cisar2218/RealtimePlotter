using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class VoxelRenderer2 : MonoBehaviour
{
    ParticleSystem system;
    ParticleSystem.Particle[] voxels;
    float[] values;
    bool voxelsUpdated = false;

    public int voxelsCount = 0;
    public int maxPoints = 54000;
    public int pointPerSec = 20;
    public float voxelScale = 0.1f;
    public float updatesPerSec = 3f;
    public float startLifetime = 10f;

    private float cumulativeTime = 0f;

    private Color minColor = Color.white;
    private Color maxColor = Color.red;

    public float valueCurrentMax = 0f;
    float valueMin = 0f;
    float valueSealing = 20f;
    public float bound = 10.0f;

    public Transform dronTransform;

    public delegate void OnRendererValueChange(float newMax);
    public static OnRendererValueChange onMaxValueChangedEvent;


    void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    private void AddPoints(int howMany)
    {
        var initVoxelsCount = voxelsCount;
        while (voxelsCount < initVoxelsCount + howMany)
        {
            values[voxelsCount] = Random.Range(valueMin, valueSealing);
            if (values[voxelsCount] > valueCurrentMax)
            {
                valueCurrentMax = values[voxelsCount];
            }
            
            var color = Color.Lerp(minColor, maxColor, values[voxelsCount] / valueCurrentMax);

            voxels[voxelsCount].startColor = color;
            voxels[voxelsCount].startSize = voxelScale;
            voxels[voxelsCount].startLifetime = startLifetime;
            voxels[voxelsCount].remainingLifetime = startLifetime;

            var posX = dronTransform.position.x + Random.Range(-bound, bound);
            var posY = dronTransform.position.y + Random.Range(-bound, bound) / 2;
            var posZ = dronTransform.position.z + Random.Range(-bound, bound);
            voxels[voxelsCount].position = new Vector3(
                posX, posY, posZ
                );
            voxelsCount += 1;
        }
    }

    private void UpdateColors(int howMany)
    {
        for (int i = 0; i < howMany; i++) {
            var color = Color.Lerp(minColor, maxColor, values[i] / valueCurrentMax);
            voxels[i].startColor = color;
        }
    }

    void ScaleMaxSeiling(float scaleFactor)
    {
        valueSealing = valueCurrentMax * scaleFactor;
        onMaxValueChangedEvent?.Invoke(valueSealing);
    }

    private IEnumerator UpdateLoop()
    {
        while (voxelsCount < maxPoints)
        {
            AddPoints(pointPerSec);
            UpdateColors(voxelsCount-pointPerSec);
            yield return new WaitForSeconds(1/updatesPerSec);
            // extend max 
            ScaleMaxSeiling(1.10f);
            
            voxelsUpdated = true;
        }
        voxelsUpdated = true;
    }

    private void Start()
    {
        voxels = new ParticleSystem.Particle[maxPoints];
        values = new float[maxPoints];
    }

    void Update()
    {
        if (voxelsUpdated)
        {
            system.SetParticles(voxels, voxels.Length);
            voxelsUpdated = false;
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(UpdateLoop());
        }

        UpdateVoxelsLifetime();
    }

    private void UpdateVoxelsLifetime()
    {
        for (int i = 0; i < voxels.Length; i++)
        {
            if (voxels[i].remainingLifetime > 0)
            {
                voxels[i].remainingLifetime -= Time.deltaTime;
            }
        }
        cumulativeTime += Time.deltaTime;
        if (cumulativeTime > 1.0f)
        {
            cumulativeTime = 0.0f;
        }
        voxelsUpdated = true;
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(bound, bound, bound));
    }
}
