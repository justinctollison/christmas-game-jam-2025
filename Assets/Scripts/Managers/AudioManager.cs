using UnityEngine;

public enum SFXType
{
    Coin,
    LightSwitch,
    Bell,
    Jump,
    Traversal,
    Door,
    Footstep
}

public enum BGMType
{
    MainMenu,
    Background
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _bgmSource;

    [Header("SFX Clips")]
    [SerializeField] private AudioClip _coinClip;
    [SerializeField] private AudioClip _lightClip;
    [SerializeField] private AudioClip _bellClip;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _traversalClip;
    [SerializeField] private AudioClip _footstepClip;

    [Header("BGM Clips")]
    [SerializeField] private AudioClip _mainMenuClip;
    [SerializeField] private AudioClip _backgroundClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(SFXType type)
    {
        AudioClip clip = type switch
        {
            SFXType.Coin => _coinClip,
            SFXType.LightSwitch => _lightClip,
            SFXType.Bell => _bellClip,
            SFXType.Jump => _jumpClip,
            SFXType.Traversal => _traversalClip,
            SFXType.Footstep => _footstepClip,
            _ => null
        };

        if (clip != null)
            _sfxSource.PlayOneShot(clip);
    }

    public void PlayBGM(BGMType type)
    {
        AudioClip clip = type switch
        {
            BGMType.Background => _backgroundClip,
            BGMType.MainMenu => _mainMenuClip,
            _ => null
        };

        _bgmSource.clip = clip;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }
}
