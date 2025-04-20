using Godot;
using System;

public partial class Balance : Sprite2D
{
    private Tween blinkTween;

    public override void _Ready()
    {
        StartBlinkIdle();
    }

    public void StartBlinkIdle()
    {

        blinkTween = GetTree().CreateTween();
        blinkTween.SetLoops(); // loop infinito

        // Pisca de 0 → 1
        blinkTween.TweenMethod(
            Callable.From<float>(SetShaderBlinkIntensity),
            0.0f,
            1.0f,
            0.3f
        ).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);

        // E volta de 1 → 0
        blinkTween.TweenMethod(
            Callable.From<float>(SetShaderBlinkIntensity),
            1.0f,
            0.0f,
            0.3f
        ).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
    }

    public void SetShaderBlinkIntensity(float value)
    {

        if (Material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("blink_intensity", value);
        }
    }
}
