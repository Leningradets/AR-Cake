using System.Collections;
using System.Collections.Generic;
using Siccity.GLTFUtility;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class WebPrefabProvider : WebResource
{
    public event Action<GameObject> PrefabImported;

    public bool PrefabIsReady => _prefabIsReady;

    public GameObject Prefab => _prefab;

    private bool _prefabIsReady = false;
    private GameObject _prefab;

    private async void Awake()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        var data = await DownloadData(_url);
        ImportGLBAsync(data);
    }

    void ImportGLBAsync(byte[] data)
    {
        Importer.ImportGLBAsync(data, new ImportSettings(), OnFinishAsync);
        
        Debug.Log("Importing prefab");
    }

    private void OnFinishAsync(GameObject gameObject, AnimationClip[] animations)
    {
        _prefab = gameObject;
        _prefab.SetActive(false);
        _prefabIsReady = true;
        PrefabImported?.Invoke(gameObject);
        Debug.Log("Prefab imported");
    }

    public async override void Reload()
    {
        await Initialize();
    }
}
