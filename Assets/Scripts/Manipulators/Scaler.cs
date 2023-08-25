using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : WebPrefabObjectManipulator
{
    private void Update()
    {
        if (_input.IsPinched)
        {
            _objecTransform.localScale *= _input.Pinch;
        }
    }
}
