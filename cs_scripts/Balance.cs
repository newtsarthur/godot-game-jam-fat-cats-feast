using Godot;
using System;

public partial class Balance : Area2D
{
    private Tween blinkTween;
    private Sprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D"); // ou o nome exato do nó
        StartBlinkIdle();
        BodyEntered += OnBodyEntered;
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
        if (sprite.Material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("blink_intensity", value);
        }
    }

    private void OnBodyEntered(Node body)
    {
        if(body.IsInGroup("PlayerGroup"))
        {
            AddPointPlayer();
            QueueFree();
        }
    }
    public void AddPointPlayer() 
    {
        PlayerData.Instance.ItemsCollectedInLevel++;
    }
}
