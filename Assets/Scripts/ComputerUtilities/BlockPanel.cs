using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlockPanel : MonoBehaviour
{
    [SerializeField]
    private Image blockPanel;
    [SerializeField]
    private Image hourglassImage;
    [SerializeField]
    private float rotationSpeed = 50f;

    public void EnableBlock(bool state)
    {
        blockPanel.enabled = state;
        hourglassImage.enabled = state;

        if (state)
        {
            StartCoroutine(AnimateImage());
        }
    }

    private IEnumerator AnimateImage()
    {
        while(hourglassImage.enabled)
        {
            hourglassImage.rectTransform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
            yield return null;
        }

        hourglassImage.rectTransform.rotation = Quaternion.identity;
    }
}
