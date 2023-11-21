using GrandDreams.Common.Unity.Utilities;
using GrandDreams.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour
{

    private void Awake()
    {
        //ConfigData.Initialize();
    }

    private IEnumerator Start()
    {
        DontDestroyOnLoad(GrandDreams.Common.Unity.Utilities.SoundManager.SharedInstance.gameObject);
        if (SoundManager.SharedInstance != null)
        {
            SoundManager.SharedInstance.PlaySong("BGGame");
            SoundManager.SharedInstance.SetSongVolume(0.4f);
        }

        Debug.Log(AlwaysOnTop.Instance == null);
        Debug.Log("b");

        yield return new WaitForEndOfFrame();

        AlwaysOnTop.Instance.KeepGameObjectOnLoad = true;
        Debug.Log("c");

        if (ConfigData.Data.AllowToSwitchScreen)
        {
            Debug.Log("d");

            AlwaysOnTop.Instance.Initialize(ConfigData.Data.WindowName, ConfigData.Data.PreferScreenPosX, ConfigData.Data.PreferScreenPosY, ConfigData.Data.PreferScreenSizeWidth, ConfigData.Data.PreferScreenSizeHeight);
        }

        yield return new WaitForEndOfFrame();

        SceneManager.LoadScene("MainScenes");
    }

}
