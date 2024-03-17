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
    public int maxPoints = 54000;
    public int pointPerSec = 20;
    public float voxelScale = 0.1f;
    public float updatesPerSec = 3f;
    public float startLifetime = 10f;

    private float cumulativeTime = 0f;

    public Transform dronTransform;


    public float bound = 10.0f;

    void Awake()
    {
        system = GetComponent<ParticleSystem>();
    }

    

    private void AddPoints(int howMany, Color color)
    {
        var initVoxelsCount = voxelsCount;
        while (voxelsCount < initVoxelsCount + howMany)
        {
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

    private IEnumerator UpdateLoop()
    {
        var whiteVec = new Color(1.0f, 1.0f, 1.0f);
        var redVec = new Color(1.0f, 0.0f, 0.0f);

        while (voxelsCount < maxPoints)
        {
            var color = Color.Lerp(whiteVec, redVec, Random.value);
            
            AddPoints(pointPerSec, color);
            yield return new WaitForSeconds(1/updatesPerSec);
            voxelsUpdated = true;
        }
        voxelsUpdated = true;
    }

    private void Start()
    {
        voxels = new ParticleSystem.Particle[maxPoints];
        StartCoroutine(UpdateLoop());


        //for (int i = 0; i < voxels.Length / 2; i++)
        //{
        //    voxels[i].startColor = new Color(1.0f, 0.0f, 0.0f);
        //    voxels[i].startSize = voxelScale;
        //    voxels[i].position = new Vector3(
        //        Random.Range(-bound, bound), Random.Range(-bound / 2, bound / 2), Random.Range(-bound, bound)
        //        );
        //}
    }

    void Update()
    {
        if (voxelsUpdated)
        {
            system.SetParticles(voxels, voxels.Length);

            Debug.Log("Update called");
            Debug.Log("Start:"+ voxels[0].startLifetime);
            Debug.Log("Remaining:"+voxels[0].remainingLifetime);
            voxelsUpdated = false;
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            for (int i = 0; i < voxelsCount; i++)
            {
                voxels[i].startColor = new Color(0.0f, 0.0f, 1.0f);
                voxels[i].startSize = voxelScale;
                voxels[voxelsCount].startLifetime = startLifetime;
                voxels[voxelsCount].remainingLifetime = startLifetime;
                voxels[i].position = new Vector3(
                    Random.Range(-bound, bound), Random.Range(-bound / 2, bound / 2), Random.Range(-bound, bound)
                    );
            }
            voxelsUpdated = true;
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
