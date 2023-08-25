using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryDownloadButton : MonoBehaviour
{
    public void RetryDownload()
    {
        foreach (var resource in FindObjectsOfType<WebResource>())
        {
            if (resource.State != DownloadState.Downloaded) 
            { 
                resource.Reload(); 
            }
        }
    }
}
