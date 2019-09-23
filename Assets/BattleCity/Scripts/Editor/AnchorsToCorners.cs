using UnityEditor;
using UnityEngine;

public class AnchorsToCorners : MonoBehaviour
{
    /// <summary>
    /// Anchors to corners.
    /// </summary>
    [MenuItem("uGUI/Anchors to Corners %[")]
    static void SetAnchorsToCorners()
    {
        // for all the selected gameobject in hierarchy
        foreach (Transform transform in Selection.transforms)
        {
            RectTransform gameObjectTransform = transform as RectTransform;
            RectTransform parentTransform = Selection.activeTransform.parent as RectTransform;

            if (gameObjectTransform == null || parentTransform == null) return;

            Vector2 newAnchorsMin = new Vector2(gameObjectTransform.anchorMin.x + gameObjectTransform.offsetMin.x / parentTransform.rect.width,
                                                gameObjectTransform.anchorMin.y + gameObjectTransform.offsetMin.y / parentTransform.rect.height);
            Vector2 newAnchorsMax = new Vector2(gameObjectTransform.anchorMax.x + gameObjectTransform.offsetMax.x / parentTransform.rect.width,
                                                gameObjectTransform.anchorMax.y + gameObjectTransform.offsetMax.y / parentTransform.rect.height);

            gameObjectTransform.anchorMin = newAnchorsMin;
            gameObjectTransform.anchorMax = newAnchorsMax;
            gameObjectTransform.offsetMin = gameObjectTransform.offsetMax = new Vector2(0, 0);
        }
    }
}