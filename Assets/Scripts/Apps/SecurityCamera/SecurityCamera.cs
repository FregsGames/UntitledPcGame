using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 maxRightForward;
    [SerializeField]
    private Vector3 maxLeftForward;
    [SerializeField]
    private Vector3 maxUpForward;
    [SerializeField]
    private Vector3 maxDownForward;
    [SerializeField]
    private Vector3 originalForward;

    [SerializeField]
    private float cameraSpeed;

    public void MoveHorizontal(bool right)
    {
        if (right)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, new Vector3(maxRightForward.x, transform.forward.y, transform.forward.z), cameraSpeed * Time.deltaTime);
        }
        else
        {
            transform.forward = Vector3.MoveTowards(transform.forward, new Vector3(maxLeftForward.x, transform.forward.y, transform.forward.z), cameraSpeed * Time.deltaTime);
        }
    }

    public void MoveVertical(bool up)
    {
        if (up)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, new Vector3(transform.forward.x, maxUpForward.y, transform.forward.z), cameraSpeed * Time.deltaTime);

        }
        else
        {
            transform.forward = Vector3.MoveTowards(transform.forward, new Vector3(transform.forward.x, maxDownForward.y, transform.forward.z), cameraSpeed * Time.deltaTime);
        }
    }
    [HorizontalGroup("Original")]
    [Button]
    public void SaveOriginal()
    {
        originalForward = transform.forward;
    }
    [HorizontalGroup("Original")]
    [Button]
    public void ToOriginal()
    {
        transform.forward = originalForward;
    }
    [HorizontalGroup("Right")]
    [Button]
    public void TestRight()
    {
        transform.forward = maxRightForward;
    }
    [HorizontalGroup("Right")]
    [Button]
    public void SaveRight()
    {
        maxRightForward = transform.forward;
    }
    [HorizontalGroup("Left")]
    [Button]
    public void TestLeft()
    {
        transform.forward = maxLeftForward;
    }
    [HorizontalGroup("Left")]
    [Button]
    public void SaveLeft()
    {
        maxLeftForward = transform.forward;
    }
    [HorizontalGroup("Up")]
    [Button]
    public void TestUp()
    {
        transform.forward = maxUpForward;
    }
    [HorizontalGroup("Up")]
    [Button]
    public void SaveUp()
    {
        maxUpForward = transform.forward;
    }
    [HorizontalGroup("Down")]
    [Button]
    public void TestDown()
    {
        transform.forward = maxDownForward;
    }
    [HorizontalGroup("Down")]
    [Button]
    public void SaveDown()
    {
        maxDownForward = transform.forward;
    }

    [Button]
    public void MoveUp()
    {
        MoveVertical(true);
    }
    [HorizontalGroup("MoveHorizontal")]
    [Button]
    public void MoveLeft()
    {
        MoveHorizontal(false);
    }
    [HorizontalGroup("MoveHorizontal")]
    [Button]
    public void MoveRight()
    {
        MoveHorizontal(true);
    }
    [Button]
    public void MoveDown()
    {
        MoveVertical(false);
    }
}
