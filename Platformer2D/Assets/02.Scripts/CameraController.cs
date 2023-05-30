using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField] private BoxCollider2D _boundShape;
    public BoxCollider2D boundShape
    {
        get => _boundShape;
        set
        {
            _boundShape = value;
            _boundShapeXmin = value.transform.position.x + value.offset.x - value.size.x / 2.0f;
            _boundShapeXmax = value.transform.position.x + value.offset.x + value.size.x / 2.0f;
            _boundShapeYmin = value.transform.position.y + value.offset.y - value.size.y / 2.0f;
            _boundShapeYmax = value.transform.position.y + value.offset.y + value.size.y / 2.0f;
        }
    }

    private float _boundShapeXmin;
    private float _boundShapeXmax;
    private float _boundShapeYmin;
    private float _boundShapeYmax;

    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _offset;
    [Range(1.0f, 10.0f)]
    [SerializeField] private float _smoothness;
    private Camera _cam;

    private void Awake()
    {
        instance = this;
        _cam = Camera.main;
        boundShape = _boundShape;
    }

    private void LateUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 targetPos = new Vector3(_target.transform.position.x,
                                        _target.transform.position.y,
                                        transform.position.z)
                            + (Vector3)_offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, _smoothness * Time.deltaTime);

        Vector3 leftBottom = _cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, _cam.nearClipPlane));
        Vector3 rightTop = _cam.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, _cam.nearClipPlane));
        Vector3 size = new Vector3(rightTop.x - leftBottom.x, rightTop.y - leftBottom.y, 0.0f);

        // x 최소경계
        if (smoothPos.x < _boundShapeXmin + size.x / 2.0f)
            smoothPos.x = _boundShapeXmin + size.x / 2.0f;
        // x 최대경계
        else if (smoothPos.x > _boundShapeXmax - size.x / 2.0f)
                 smoothPos.x = _boundShapeXmax - size.x / 2.0f;

        // y 최소경계
        if (smoothPos.y < _boundShapeYmin + size.y / 2.0f)
            smoothPos.y = _boundShapeYmin + size.y / 2.0f;
        // y 최대경계
        else if (smoothPos.y > _boundShapeYmax - size.y / 2.0f)
                 smoothPos.y = _boundShapeYmax - size.y / 2.0f;

        transform.position = smoothPos;                            
    }

    private void OnDrawGizmosSelected()
    {
        Camera cam = Camera.main;

        Vector3 leftBottom = cam.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, cam.nearClipPlane));
        Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, cam.nearClipPlane));
        Vector3 center = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, rightTop - leftBottom);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boundShape.transform.position + (Vector3)_boundShape.offset, _boundShape.size);

    }
}
