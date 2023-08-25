using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DownloadingStateView : MonoBehaviour
{
    [SerializeField] private StatePanel _downloadingStatePanel;
    [SerializeField] private StatePanel _connectionErrorStatePanel;
    [SerializeField] private StatePanel _unknownErrorStatePanel;

    private WebResource[] _webResources;

    private void Awake()
    {
        _webResources = FindObjectsOfType<WebResource>();
    }

    private void OnEnable()
    {
        foreach (var resource in _webResources)
        {
            resource.StateChanged += OnStateChanged;
        }
    }

    private void OnDisable()
    {
        foreach (var resource in _webResources)
        {
            resource.StateChanged -= OnStateChanged;
        }
    }

    private void OnStateChanged(DownloadState state)
    {
        switch (state)
        {
            case DownloadState.Downloading:
                _connectionErrorStatePanel.Close();
                _downloadingStatePanel.Open();
                break;
            case DownloadState.Downloaded:
                if (CheckAllWebResourcesDownloaded())
                {
                    _downloadingStatePanel.Close();
                }
                break;
            case DownloadState.ConnectionError:
                _downloadingStatePanel.Close();
                _connectionErrorStatePanel.Open();
                break;
            case DownloadState.Error:
                _unknownErrorStatePanel.Open();
                break;
        }

    }

    private bool CheckAllWebResourcesDownloaded()
    {
        foreach (var resource in _webResources)
        {
            if(resource.State != DownloadState.Downloaded)
            {
                return false;
            }
        }

        return true;
    }
}