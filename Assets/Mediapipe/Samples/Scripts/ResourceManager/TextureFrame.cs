using Mediapipe;
using System;
using Unity.Collections;
using UnityEngine;

public class TextureFrame {
  private Texture2D texture;
  private IntPtr nativeTexturePtr = IntPtr.Zero;

  public int width { get; private set; }
  public int height { get; private set; }

  public GlTextureBuffer.DeletionCallback OnRelease;
    
  public TextureFrame(int width, int height, GlTextureBuffer.DeletionCallback OnRelease) {
    texture = new Texture2D( width, height, TextureFormat.RGBA32, false);
    this.width = width;
    this.height = height;
    this.OnRelease = OnRelease;
  }

  public void CopyTexture(Texture dst) {
    Graphics.CopyTexture(texture, dst);
  }

  public void CopyTextureFrom(WebCamTexture src) {
        // TODO: Convert format on GPU
       Color32[] original = src.GetPixels32();
        Color32[] rotated;
       rotated = Rotate(original);
       texture.SetPixels32(rotated);
       texture.Apply();
    
  }

    private Color32[] Rotate(Color32[] _datas)
    {
        Color32[] original = _datas;
        Color32[] rotated = new Color32[original.Length];
        int Width = 640;
        int Height = 480;

        for (int j = 0; j < Height; ++j)
        {
            for (int i = 0; i < Width; ++i)
            {
                // 90度
                rotated[original.Length - (Height * i + Height) + j] = original[original.Length - (Width * j + Width) + i];     
            }
        }
        return rotated;
    }

  public Color32[] GetPixels32() {
    return texture.GetPixels32();
  }

  // TODO: implement generic method
  public NativeArray<byte> GetRawNativeByteArray() {
    return texture.GetRawTextureData<byte>();
  }

  public IntPtr GetNativeTexturePtr(bool update = true) {
    if (update || nativeTexturePtr == IntPtr.Zero) {
      nativeTexturePtr = texture.GetNativeTexturePtr();
    }

    return nativeTexturePtr;
  }

  public GpuBufferFormat gpuBufferformat {
    get {
      return GpuBufferFormat.kBGRA32;
    }
  }

  public void Release() {
    OnRelease((UInt64)GetNativeTexturePtr(false), IntPtr.Zero);
  }
}
