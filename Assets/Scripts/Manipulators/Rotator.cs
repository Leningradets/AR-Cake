using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : WebPrefabObjectManipulator
{
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_input.IsRotating)
        {
            _objecTransform.Rotate(_camera.transform.up, -_input.Rotation.x, Space.World);
            _objecTransform.Rotate(_camera.transform.right, _input.Rotation.y, Space.World);
        }
    }
}
