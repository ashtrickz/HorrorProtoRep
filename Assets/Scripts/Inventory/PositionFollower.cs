using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    public Vector3 Offset;
    
    [SerializeField]
    private Transform target;

    public void Update() => transform.position = target.transform.position + Offset;
}