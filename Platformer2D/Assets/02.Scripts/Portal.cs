using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static bool IsBusy;
    [SerializeField] private Portal _destination;
    [SerializeField] private Map _map;
    [SerializeField] private LayerMask _playerMask;

    private void MoveToDestination(Transform target)
    {
        //target.gameObject.SetActive(false);
        _map.gameObject.SetActive(false);
        target.position = _destination.transform.position;
        CameraController.instance.boundShape = _destination._map.boundShape;
        _destination._map.gameObject.SetActive(true);
        //target.gameObject.SetActive(true);
        Invoke("Ready", 1.0f);
    }

    private void Ready()
    {
        IsBusy = false;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsBusy == false &&
            Input.GetKey(KeyCode.UpArrow) &&
            ((1 << collision.gameObject.layer & _playerMask) >= 0))
        {
            IsBusy = true;
            MoveToDestination(collision.transform);
        }
    }
}
