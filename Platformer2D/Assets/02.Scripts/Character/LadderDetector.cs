using Mono.Cecil.Cil;
using UnityEngine;

public class LadderDetector : MonoBehaviour
{
    public bool isUpDetected => _upLadder != null;
    public bool isDownDetected => _downLadder != null;
    public bool doEscapeUp => Physics2D.OverlapCircle((Vector2)transform.position + Vector2.up * upEscapeOffset, 0.01f, _targetMask) == null &&
                              Physics2D.OverlapCircle((Vector2)transform.position, 0.01f, _targetMask) != null;
    public bool doEscapeDown => Physics2D.OverlapCircle((Vector2)transform.position + Vector2.up * downEscapeOffset, 0.01f, _targetMask) == null &&
                                Physics2D.OverlapCircle((Vector2)transform.position + Vector2.up * _col.size.y, 0.01f, _targetMask) != null;

    public float upEscapeOffset;
    public float downEscapeOffset;
    public float upClimbOffset;
    public float downClimbOffset;
    [SerializeField] private LayerMask _targetMask;

    private BoxCollider2D _upLadder;
    private BoxCollider2D _downLadder;
    public Vector2 latestUpLadderTopPos;
    public Vector2 latestDownLadderTopPos;

    private CapsuleCollider2D _col;

    public Vector2 GetUpLadderTopPos()
    {
        if (_upLadder == null)
            return Vector2.zero;

        return new Vector2(_upLadder.transform.position.x,
                           _upLadder.transform.position.y + _upLadder.offset.y + _upLadder.size.y / 2.0f);
    }

    public Vector2 GetUpLadderBottomPos()
    {
        if (_upLadder == null)
            return Vector2.zero;

        return new Vector2(_upLadder.transform.position.x,
                           _upLadder.transform.position.y + _upLadder.offset.y - _upLadder.size.y / 2.0f);
    }

    public Vector2 GetDownLadderTopPos()
    {
        if (_downLadder == null)
            return Vector2.zero;

        return new Vector2(_downLadder.transform.position.x,
                           _downLadder.transform.position.y + _downLadder.offset.y + _downLadder.size.y / 2.0f);
    }

    public Vector2 GetDownLadderBottomPos()
    {
        if (_downLadder == null)
            return Vector2.zero;

        return new Vector2(_downLadder.transform.position.x,
                           _downLadder.transform.position.y + _downLadder.offset.y - _downLadder.size.y / 2.0f);
    }

    public Vector2 GetClimbUpStartPos()
    {
        if (_upLadder == null)
            return Vector2.zero;

        float startPosY = _upLadder.transform.position.y + _upLadder.offset.y - _upLadder.size.y / 2.0f + upEscapeOffset;
        startPosY = startPosY > transform.position.y ? startPosY : transform.position.y;
        return new Vector2(_upLadder.transform.position.x + _upLadder.offset.x, startPosY);
    }
    public Vector2 GetClimbDownStartPos()
    {
        if (_downLadder == null)
            return Vector2.zero;

        float startPosY = _downLadder.transform.position.y + _downLadder.offset.y + _downLadder.size.y / 2.0f - downEscapeOffset;
        Debug.Log($"{startPosY}, {transform.position.y} ");
        //startPosY = startPosY < transform.position.y ? startPosY : transform.position.y;
        return new Vector2(_downLadder.transform.position.x + _downLadder.offset.x, startPosY);
    }

    private void Awake()
    {
        _col = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        _upLadder = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.up * upClimbOffset, 0.01f, _targetMask) as BoxCollider2D;
        _downLadder = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.up * downClimbOffset, 0.01f, _targetMask) as BoxCollider2D;
        
        if (_upLadder != null)
            latestUpLadderTopPos = GetUpLadderTopPos();

        if (_downLadder != null)
            latestDownLadderTopPos = GetDownLadderTopPos();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * upEscapeOffset + Vector3.left * 0.1f,
                        transform.position + Vector3.up * upEscapeOffset + Vector3.right * 0.1f);
        Gizmos.DrawLine(transform.position + Vector3.up * downEscapeOffset + Vector3.left * 0.1f,
                        transform.position + Vector3.up * downEscapeOffset + Vector3.right * 0.1f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.up * upClimbOffset + Vector3.left * 0.1f,
                        transform.position + Vector3.up * upClimbOffset + Vector3.right * 0.1f);
        Gizmos.DrawLine(transform.position + Vector3.up * downClimbOffset + Vector3.left * 0.1f,
                        transform.position + Vector3.up * downClimbOffset + Vector3.right * 0.1f);
    }
}
