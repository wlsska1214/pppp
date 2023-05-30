using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public bool isDetected
    {
        get
        {
            return _isDetected;
        }
        set
        {
            if (_isDetected == value)
                return;

            _isDetected = value;
            posDetected = (Vector2)transform.position;
        }
    }
    private bool _isDetected;
    public Vector2 posDetected;
    public bool isTopOn;
    public bool isBottomOn;
    [SerializeField] private Vector2 _topCenter;
    [SerializeField] private Vector2 _topSize;
    [SerializeField] private Vector2 _bottomCenter;
    [SerializeField] private Vector2 _bottomSize;
    [SerializeField] private LayerMask _targetMask;

    private void FixedUpdate()
    {
        int dir = transform.eulerAngles.y == 0.0 ? Constants.DIRECTION_RIGHT : Constants.DIRECTION_LEFT;
        isTopOn = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(_topCenter.x * dir, _topCenter.y), _topSize, 0.0f, _targetMask);    
        isBottomOn = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(_bottomCenter.x * dir, _bottomCenter.y), _bottomSize , 0.0f, _targetMask);

        isDetected = isTopOn == false && isBottomOn == true;
    }


    private void OnDrawGizmos()
    {
        int dir = transform.eulerAngles.y == 0.0 ? Constants.DIRECTION_RIGHT : Constants.DIRECTION_LEFT;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(_topCenter.x * dir, _topCenter.y, 0.0f), _topSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(_bottomCenter.x * dir, _bottomCenter.y, 0.0f), _topSize);
    }
}
