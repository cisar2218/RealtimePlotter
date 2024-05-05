using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIValuesDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpMax;
    [SerializeField] TextMeshProUGUI tmpMid;
    [SerializeField] TextMeshProUGUI tmpMin;


    private void UpdateText(float valMin, float valMid, float valMax)
    {
        tmpMin.text = valMin.ToString();
        tmpMax.text = valMax.ToString();
        tmpMid.text = valMid.ToString();
    }


    void Awake()
    {
        VoxelRenderer.onMaxValueChangedEvent += UpdateText;
    }

    private void OnDestroy()
    {
        VoxelRenderer.onMaxValueChangedEvent -= UpdateText;
    }
}
