using Snek.AudioManager;
using Snek.SingletonManager;
using UnityEngine;

public abstract class UIPopupWithSFX : UIPopup
{
    protected SnekSFXManager _sfxManager;

    [SerializeField] protected AudioClip _showSound;

    protected override void Initialize()
    {
        base.Initialize();

        _sfxManager = SnekSingletonManager.GetSingleton<SnekSFXManager>();
    }

    protected override void Validate()
    {
        base.Validate();

        if (!_sfxManager)
            FailValidation("Cannot find SFX manager singleton.");

        if (!_showSound)
            FailValidation("Show sound not assigned.");
    }

    protected virtual void OnEnable()
    {
        _sfxManager.PlaySound(_showSound);
    }
}
