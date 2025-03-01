using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCHeadTurn : MonoBehaviour
{
    [Header("NPCHeadTurn Experimental")]
    [SerializeField] Transform _headTransform;
    [SerializeField] Transform _headForward;
    [SerializeField] bool _isLooking = false;
    [SerializeField] float _limitYaw = 50f;
    [SerializeField] float _limitPitch = 10f;
    [SerializeField] float _limitRoll = 1f;
    [SerializeField] float _distance = 4f;
    [SerializeField] float _turnSpeed = 4.5f;
    float _headResetTimer = .75f;
    Quaternion _lastRot;
    Transform camTransform;
    Transform GetHeadTransform() =>
        GetComponentsInChildren<Transform>()
        .Where(item => item.name.ToLower().Contains("head"))
        .FirstOrDefault();

    private void Start()
    {
        camTransform = Camera.main.gameObject.transform;
        if (_headTransform == null)
            GetHeadTransform();
        if (_headForward == null)
        {
            _headForward = new GameObject("HeadForward").transform;
            _headForward.transform.position = _headTransform.position;
            _headForward.transform.rotation = _headTransform.rotation;
            _headForward.transform.localScale = Vector3.one;
            _headForward.transform.parent= _headTransform.parent;
        }
    }

    private void LateUpdate()
    {
        Vector3 Direction = (camTransform.position - _headTransform.position).normalized;
        float angle = Vector3.SignedAngle(Direction, _headForward.forward, _headForward.up);
        if (angle < _limitYaw && angle > -_limitYaw && Vector3.Distance(camTransform.position, _headTransform.position) < _distance)
        {
            if (!_isLooking)
            {
                _isLooking = true;
                _lastRot = _headTransform.rotation;
            }

            Quaternion targetRot = Quaternion.LookRotation(camTransform.position - _headTransform.position);
            _lastRot = Quaternion.Slerp(_lastRot, targetRot, _turnSpeed * Time.deltaTime);
            _headTransform.rotation = _lastRot;
            _headResetTimer = .75f;
        }
        else if (_isLooking)
        {
            _lastRot = Quaternion.Slerp(_lastRot, _headForward.rotation, _turnSpeed * Time.deltaTime);
            _headTransform.rotation = _lastRot;
            _headResetTimer -= Time.deltaTime;
            if (_headResetTimer <= 0)
            {
                _headTransform.rotation = _headForward.rotation;
                _isLooking = false; 
            }
        }
    }
}
