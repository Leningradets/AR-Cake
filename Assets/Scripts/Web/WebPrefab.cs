using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebPrefab : MonoBehaviour
{
    public event Action<GameObject> Initialized;

    public GameObject Object => _object;
    private WebPrefabProvider _dataProvider;
    private GameObject _object;

    private void Awake()
    {
        _dataProvider = FindObjectOfType<WebPrefabProvider>();

        if (_dataProvider.PrefabIsReady)
        {
            Initialize(_dataProvider.Prefab);
        }
        else
        {
            _dataProvider.PrefabImported += Initialize;
        }
    }

    private void Initialize(GameObject prefabObject)
    {
        _object = prefabObject;
        _object.transform.parent = transform;
        _object.SetActive(true);
        _object.transform.localPosition = Vector3.zero;
        _object.transform.localRotation = Quaternion.identity;
        _dataProvider.PrefabImported -= Initialize;
        Initialized?.Invoke(_object);
    }
}
