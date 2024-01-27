using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPlotter : MonoBehaviour
{
    public GameObject pointPrefab;
    public int numPoints = 100;
    public float bound = 7.0f;

    private List<GameObject> points;

    private void Awake()
    {
        points = new List<GameObject>();
    }

    void Start()
    {
        for (int i = 0; i < numPoints;  i++)
        {
            var position = new Vector3(
                Random.Range(-bound, bound), Random.Range(-bound, bound), Random.Range(-bound, bound)
                );
            points.Add(Instantiate(pointPrefab, position, Quaternion.identity));
        }
    }

    private void Update()
    {
        //foreach (var point in points)
        //{
        //    point.transform.position += new Vector3(
        //        Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)
        //        );
        //}
    }

    void OnDrawGizmos()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(7, 7, 7));
    }
}
