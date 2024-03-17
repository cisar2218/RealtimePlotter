using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMaxDisplay : MonoBehaviour
{
    TextMeshProUGUI tmp;
    string basePhrase = "Current Max Value:";

    void UpdateText(float value) {
        tmp.text = $"{basePhrase} {value}";
    }

    // Start is called before the first frame update
    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        VoxelRenderer2.onMaxValueChangedEvent += UpdateText;
    }

    private void OnDestroy()
    {
        VoxelRenderer2.onMaxValueChangedEvent -= UpdateText;
    }
}
