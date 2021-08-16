using Mediapipe;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WebCamScreenController : MonoBehaviour
{
    private int Width = 640;
    private int Height = 480;
    float rate = 1.77f;
    private int FPS = 30;
//    [SerializeField] float FocalLengthPx = 2.0f; /// TODO: calculate it from webCamDevice info if possible.
    private const int TEXTURE_SIZE_THRESHOLD = 50;
    private const int MAX_FRAMES_TO_BE_INITIALIZED = 500;
    private WebCamDevice webCamDevice;
    private WebCamTexture webCamTexture;
    private Texture2D outputTexture;
    private Color32[] pixelData;

    private void Awake()
    {
        float _z = 0.6f * rate;
        this.transform.localScale = new Vector3(-0.6f, 1.0f, _z);
    }

    public bool isPlaying
    {
        get { return isWebCamTextureInitialized && webCamTexture.isPlaying; }
    }

    private bool isWebCamTextureInitialized
    {
        get
        {
            // Some cameras may take time to be initialized, so check the texture size.
            return webCamTexture != null && webCamTexture.width > TEXTURE_SIZE_THRESHOLD;
        }
    }

    private bool isWebCamReady
    {
        get
        {
            return isWebCamTextureInitialized && pixelData != null;
        }
    }

    public IEnumerator ResetScreen(WebCamDevice? device)
    {
        
        if (isPlaying)
        {
            webCamTexture.Stop();
            webCamTexture = null;
            pixelData = null;
        }
        
        if (device is WebCamDevice deviceValue)
        {
            webCamDevice = deviceValue;
        }
        else
        {
            yield break;
        }

        webCamTexture = new WebCamTexture(webCamDevice.name, Width, Height, FPS);
        WebCamTextureFramePool.Instance.SetDimension(Width, Height);
        
        try
        {
            webCamTexture.Play();
            Debug.Log($"WebCamTexture Graphics Format: {webCamTexture.graphicsFormat}");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
            yield break;
        }
        
        var waitFrame = MAX_FRAMES_TO_BE_INITIALIZED;

        yield return new WaitUntil(() => {
            return isWebCamTextureInitialized || --waitFrame < 0;
        });

        if (!isWebCamTextureInitialized)
        {
            Debug.LogError("Failed to initialize WebCamTexture");
            yield break;
        }

        outputTexture = new Texture2D( Height, Width, TextureFormat.RGBA32, false);
        pixelData = new Color32[Width * Height];
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = outputTexture;
    }

    public bool isOK
    {
        get { return outputTexture != null; }
    }


    private void Update()
    {
        if (isOK == true)
        {
            if (webCamTexture.width > 16 || webCamTexture.height > 16)
            {
                webCamTexture.GetPixels32(pixelData);
                Color32[] rotate_pixels = rotateTexture2D(pixelData, true);
                outputTexture.SetPixels32(rotate_pixels);
                outputTexture.Apply();
                GetComponent<Renderer>().material.mainTexture = outputTexture;
            }
            else if (webCamTexture.width <= 16 || webCamTexture.height <= 16)
            {
                Debug.Log("not yet!");
            }
        }
        else
        {
            Debug.Log("not yet!!!!!!!!!!");
        }
    }

    private Color32[] rotateTexture2D(Color32[] _datas, bool clockwise)
    {
        Color32[] original = _datas;
        Color32[] rotated = new Color32[original.Length];
       // int k = 0;

        for (int j = 0; j < Height; ++j)
        {
            for (int i = 0; i < Width; ++i)
            {
                // 90度
                rotated[original.Length - (Height * i + Height) + j] = original[original.Length - (Width * j + Width) + i];
                // 270度
                //rotated[original.Length - (hei * i + hei) + j] = original[k];
                //k++;
            }
        }

        return rotated;
    }
    

    /*
    public float GetFocalLengthPx()
    {
        Debug.Log("1");
        return isPlaying ? FocalLengthPx : 0;
    }
    
    public Color32[] GetPixels32()
    {
        Debug.Log("2");
        return isPlaying ? webCamTexture.GetPixels32(pixelData) : null;
    }

    public IntPtr GetNativeTexturePtr()
    {
        Debug.Log("3");
        return webCamTexture.GetNativeTexturePtr();
    }
    */

    public Texture2D GetScreen()
    {
        Debug.Log("get screen: " + "width:" + outputTexture.width + "height:" + outputTexture.height);
        return outputTexture;     
    }

    /*
    public void DrawScreen(Color32[] colors)
    {
        Debug.Log("5");
        if (!isWebCamReady)
        {
            return;
        }
        // TODO: size assertion
        outputTexture.SetPixels32(colors);
        outputTexture.Apply();
    }*/
    
    public void DrawScreen(TextureFrame src)
    {
        if (!isWebCamReady)
        {
            return;
        }
        // TODO: size assertion
        
        src.CopyTexture(outputTexture);
    }
    
    public void DrawScreen(ImageFrame imageFrame)
    {
        if (!isWebCamReady) { return; }
        outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
        outputTexture.Apply();
    }

    public void DrawScreen(GpuBuffer gpuBuffer)
    {
        if (!isWebCamReady)
        {
            return;
        }
#if (UNITY_STANDALONE_LINUX || UNITY_ANDROID) && !UNITY_EDITOR_OSX && !UNITY_EDITOR_WIN
    // TODO: create an external texture
    outputTexture.UpdateExternalTexture((IntPtr)gpuBuffer.GetGlTextureBuffer().Name());
#else
        throw new NotSupportedException();
#endif
    }
    
    public TextureFramePool.TextureFrameRequest RequestNextFrame()
    {
        return WebCamTextureFramePool.Instance.RequestNextTextureFrame((TextureFrame textureFrame) => {
            if (isPlaying)
            {
                //textureFrame.CopyTextureFrom(webCamTexture);
                textureFrame.CopyTextureFrom(webCamTexture);
            }
        });
    }
    
    private class WebCamTextureFramePool : TextureFramePool { }
}
