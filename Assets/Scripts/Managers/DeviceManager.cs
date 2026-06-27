using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DeviceManager : MonoBehaviour
{
    [SerializeField] private Volume postProcessVolume;
    public bool IsMobile()
    {
#if UNITY_ANDROID || UNITY_IOS
        return true;
#else
        return false;
#endif
    }

    public void InitializeScreen()
    {
        if (IsMobile())
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    public void SetPostProcessing()
    {
        if (postProcessVolume != null &&
            postProcessVolume.profile.TryGet<LensDistortion>(out var lensDistortion))
        {
            if (IsMobile())
            {
                lensDistortion.intensity.value = 0.12f;
            }
            else
            {
                lensDistortion.intensity.value = 0.25f;
            }
        }
        else
        {
            Log.Warning("LensDistortion could not be found in the post-processing profile.");
        }
    }
}
