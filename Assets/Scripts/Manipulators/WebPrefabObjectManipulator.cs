using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UserInput), typeof(WebPrefab))]
public class WebPrefabObjectManipulator : MonoBehaviour
{
    protected UserInput _input;
    protected Transform _objecTransform;
    private WebPrefab _prefab;

    protected virtual void Awake()
    {
        _input = GetComponent<UserInput>();
        _prefab = GetComponent<WebPrefab>();

        if(_prefab.Object != null)
        {
            _objecTransform = _prefab.Object.transform;
        }
        else
        {
            _prefab.Initialized += OnInitialized;
        }
    }

    private void OnInitialized(GameObject prefabObject)
    {
        _prefab.Initialized -= OnInitialized;
        _objecTransform = prefabObject.transform;
    }
}
