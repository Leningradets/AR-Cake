using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class WebImageReference : WebResource
{
    public RawImage image;

    private ARTrackedImageManager _trackedImageManager;

    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();   
    }

    private async void Start()
    {
        await Initialize();
    }

    private async Task Initialize()
    {
        var bytes = await DownloadData(_url);
        await AddToLibrary(bytes);
    }

    private async Task AddToLibrary(byte[] bytes)
    {
        var imageTexture = new Texture2D(10, 10);
        imageTexture.LoadImage(bytes);

        var library = _trackedImageManager.referenceLibrary;

        if (library is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            try
            {
                var job = mutableLibrary.ScheduleAddImageWithValidationJob(imageTexture, "Image", 3f);

                Debug.Log("Adding image to library");

                while (!job.jobHandle.IsCompleted)
                {
                    await Task.Yield();
                }

                if (job.status == AddReferenceImageJobStatus.Success)
                {
                    Debug.Log("Image added to library");
                }
                else if (job.status == AddReferenceImageJobStatus.ErrorInvalidImage)
                {
                    Debug.LogError("Error InvalidImage");
                }
                else
                {
                    Debug.LogError("Error Unknown");
                }

            }
            catch (InvalidOperationException e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.Log("Reference Library is immutable");
        }
    }

    public override async void Reload()
    {
        await Initialize();
    }
}