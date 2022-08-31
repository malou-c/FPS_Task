using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AudioActiveStatus
{
    ON,
    OFF
}

public class AudioSwitcher : MonoBehaviour
{
    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _deactiveSprite;
    private Image _image;
    private AudioActiveStatus _audioActive;
    // Start is called before the first frame update
    void Start()
    {
        // Для первого запуска
        if (!PlayerPrefs.HasKey("AUDIO_ACTIVE"))
            PlayerPrefs.SetInt("AUDIO_ACTIVE", 0);

        _image = GetComponent<Image>();
        _audioActive = (AudioActiveStatus)PlayerPrefs.GetInt("AUDIO_ACTIVE");
        SwitchImage();
    }

    public void SwitchActive()
    {
        _audioActive = _audioActive == AudioActiveStatus.ON? AudioActiveStatus.OFF : AudioActiveStatus.ON;
        PlayerPrefs.SetInt("AUDIO_ACTIVE", (int)_audioActive);
        SwitchImage();
    }

    public void SwitchImage()
    {
        switch (_audioActive)
        {
            case AudioActiveStatus.ON:
                _image.sprite = _activeSprite;
                break;
            case AudioActiveStatus.OFF:
                _image.sprite = _deactiveSprite;
                break;
            default:
                break;
        }
    }
}
