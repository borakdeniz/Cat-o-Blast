using UnityEngine;
using System.Collections.Generic;

public class GemColorManager : MonoBehaviour
{
    public static GemColorManager Instance;
    public List<GemColorData> colorDataList;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public GemColorData GetGemColorData(int gemColorId)
    {
        return colorDataList.Find(data => data.gemColorId == gemColorId);
    }
}
