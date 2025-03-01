using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScaleBasedOnDistance : MonoBehaviour
{
    Transform cam;
    float FixedSize = 0.00001f;
    [SerializeField] float minScale;    
    [SerializeField] float maxScale;
    [SerializeField] float _minDistance;
    [SerializeField] float _maxDistance;
    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Scale();
    }

    void Scale()
    {
        var distance = Vector3.Distance(cam.position, transform.position);
        var size = distance * FixedSize * Camera.main.fieldOfView;
        var scale = Mathf.Lerp(minScale, maxScale, Mathf.InverseLerp(_minDistance, _maxDistance, distance));
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
