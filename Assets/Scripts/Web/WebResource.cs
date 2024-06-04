using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WebResource : MonoBehaviour
{
    public event Action<DownloadState> StateChanged;
    public DownloadState State => _state;

    [SerializeField] protected string _url;

    private DownloadState _state
    {
        set
        {
            _state_ = value;
            StateChanged?.Invoke(_state_);
        }
        get { return _state_; }
    }

    private DownloadState _state_;

    protected async Task<byte[]> DownloadData(string url)
    {
        _state = DownloadState.Downloading;
        UnityWebRequest request = UnityWebRequest.Get(url);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            _state = DownloadState.Downloaded;
            return request.downloadHandler.data;
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            _state = DownloadState.ConnectionError;
        }
        else
        {
            _state = DownloadState.Error;
        }

        return null;
    }

    public abstract void Reload();
}
