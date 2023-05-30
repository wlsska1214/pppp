using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public bool isDetected
    {
        get
        {
            if (_ignoreCoroutineCount > 0)
            {
                foreach (var ignoring in ignoringsBuffer)
                {
                    if (ignoring == current)
                        return false;
                }
            }

            return current;
        }
    }
    [SerializeField] private Vector2 _offset;
    [SerializeField] private Vector2 _size;
    [SerializeField] LayerMask _targetMask;
    public Collider2D current;
    public Collider2D latest;
    public Collider2D[] ignoringsBuffer = new Collider2D[10];
    private bool[] _triggersBuffer = new bool[10];
    private int _triggeredIgnoringIndex;
    private Collider2D _subject;
    private bool _isSubjectTriggered;
    private int _ignoreCoroutineCount;

    public bool IsGroundExistBelow()
    {
        if (isDetected == false)
            return false;

        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, _size, 0.0f, Vector2.down, 3.0f, _targetMask);

        if (hits.Length > 1)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider != current)
                    return true;
            }
        }
        return false;
    }

    public void IgnoreLatest()
    {
        _ignoreCoroutineCount++;
        StartCoroutine(E_IgnoreGround(latest));
    }

    IEnumerator E_IgnoreGround(Collider2D ground)
    {
        int index = FindIndexOfEmptyBuffer();
        if (index >= 0)
        {
            ignoringsBuffer[index] = ground;
            _triggersBuffer[index] = false;
            Physics2D.IgnoreCollision(_subject, ground, true);
            yield return new WaitUntil(() => _triggersBuffer[index]);
            yield return new WaitUntil(() => _triggersBuffer[index] == false);
            Physics2D.IgnoreCollision(_subject, ground, false);
            ignoringsBuffer[index] = null;
        }
        _ignoreCoroutineCount--;
    }

    private int FindIndexOfEmptyBuffer()
    {
        for (int i = 0; i < ignoringsBuffer.Length; i++)
        {
            if (ignoringsBuffer[i] == null)
                return i;
        }
        return -1;
    }

    private void Awake()
    {
        _subject = transform.GetChild(1).GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        current =  Physics2D.OverlapBox(transform.position + (Vector3)_offset, _size, 0.0f, _targetMask);

        if (current != latest &&
            current != null)
        {
            latest = current;
        }

        for (int i = 0; i < _triggersBuffer.Length; i++)
        {
            _triggersBuffer[i] = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + (Vector3)_offset, _size);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position + Vector3.down * 1.5f, new Vector3(_size.x, 3.0f, 0.0f));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & _targetMask) > 0)
        {
            for (int i = 0; i < ignoringsBuffer.Length; i++)
            {
                if (ignoringsBuffer[i] == collision)
                {
                    _triggersBuffer[i] = true;
                }
            }
        }
    }
}
