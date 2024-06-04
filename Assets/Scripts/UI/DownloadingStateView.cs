using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DownloadingStateView : MonoBehaviour
{
    private Dictionary<DownloadState, StatePanel> _statePanels = new Dictionary<DownloadState, StatePanel>();
    private StatePanel _currentStatePanel;
    private WebResource[] _webResources;

    private void Awake()
    {
        var panels = GetComponentsInChildren<StatePanel>(true);

        foreach (var panel in panels)
        {
            if (_statePanels.TryAdd(panel.State, panel) == false)
            {
                Debug.LogWarning($"Duplicate state panel: {panel.State}, new panel was not added");
            }
        }
    }

    private void OnEnable()
    {
        _webResources = FindObjectsOfType<WebResource>();
        
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
        if (state == DownloadState.Downloaded && !CheckAllWebResourcesDownloaded())
        {
            CloseCurrentPanel();
            return;
        }
        
        if (_statePanels.TryGetValue(state, out var panel))
        {
            OpenPanel(panel);
        }
    }

    private void OpenPanel(StatePanel panel)
    {
        if (_currentStatePanel != null)
        {
            _currentStatePanel.Close();
        }
        
        panel.Open();
        _currentStatePanel = panel;
    }
    
    private void CloseCurrentPanel()
    {
        if (_currentStatePanel != null)
        {
            _currentStatePanel.Close();
            _currentStatePanel = null;
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