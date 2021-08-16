using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class WebCamDeviceSelect : MonoBehaviour
{
    private GameObject sceneDirector;
    private WebCamTexture _front;
    private WebCamTexture _back;
    public int devicSelect = 0;
    public RawImage CanvasScreen;
    IEnumerator Start()
    {
        WebCamSetting();
        sceneDirector = GameObject.Find("SceneDirector");



#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitForSeconds(0.1f);
        }
#elif UNITY_IOS
    if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
      yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    }
#endif

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.LogWarning("Not permitted to use Camera");
            yield break;
        }
#elif UNITY_IOS
    if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
      Debug.LogWarning("Not permitted to use WebCam");
      yield break;
    }
#endif
        yield return new WaitForEndOfFrame();
    }

    //設定webcam
    void WebCamSetting()
    {
        _back = new WebCamTexture(WebCamTexture.devices[0].name, Screen.width, Screen.height, 30);
        _front = new WebCamTexture(WebCamTexture.devices[1].name, Screen.width, Screen.height, 30);
        CanvasScreen.texture = _front;
        _front.Play();

    }

    //button的功能
    public void ChangeWebcam()
    {
        if(devicSelect == 0)
        {
            _front.Stop();
            _back.Play();
            devicSelect = 1;
        }
        else if(devicSelect == 1)
        {
            _back.Stop();
            _front.Play();
        }

    }

}
