using UnityEngine.UI;
using UnityEngine;

public class BlockPanel : MonoBehaviour
{
    [SerializeField]
    private Image blockPanel;

    public void EnableBlock(bool state)
    {
        blockPanel.enabled = state;
    }
}
