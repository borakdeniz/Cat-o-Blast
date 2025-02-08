using UnityEngine;
using System.Collections;

[System.Serializable]
public class GemColorData
{
    public int gemColorId;
    public Sprite defaultSprite;
    public Sprite firstSprite;
    public Sprite secondSprite;
    public Sprite thirdSprite;
}

public class Gem : MonoBehaviour
{
    public int gemColorId;
    private SpriteRenderer gemRenderer;
    private CollapseManager collapseManager;
    public bool isMarkedForDestruction = false;

    private void Awake()
    {
        gemRenderer = GetComponent<SpriteRenderer>();
        collapseManager = FindObjectOfType<CollapseManager>();
    }

    private void Start()
    {
        UpdateGemSprite(1);
    }

    private void OnMouseDown()
    {
        if (isMarkedForDestruction) return;
        collapseManager.HandleGemClick(this);
    }

    public void UpdateGemSprite(int adjacentGemCount)
    {
        GemColorData colorData = GemColorManager.Instance.GetGemColorData(gemColorId);

        if (adjacentGemCount > CollapseManager.conditionC)
            gemRenderer.sprite = colorData.thirdSprite;
        else if (adjacentGemCount > CollapseManager.conditionB)
            gemRenderer.sprite = colorData.secondSprite;
        else if (adjacentGemCount > CollapseManager.conditionA)
            gemRenderer.sprite = colorData.firstSprite;
        else
            gemRenderer.sprite = colorData.defaultSprite;
    }

    public IEnumerator AnimateGemDestruction()
    {
        isMarkedForDestruction = true;
        float animationDuration = 0.3f;
        float elapsedTime = 0f;
        Vector3 initialScale = transform.localScale;

        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
