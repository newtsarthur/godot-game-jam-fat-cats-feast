using Godot;
using System;

public partial class Fade : CanvasLayer
{
    public override void _Ready()
    {
        var fade = GetNode<ColorRect>("ColorRect");
        
        // Certifique-se que o ColorRect cobre toda a tela
        fade.Size = GetViewport().GetVisibleRect().Size;
        
        // Inicialmente, o ColorRect deve ser transparente
        fade.Modulate = new Color(33f / 255f, 30f / 255f, 51f / 255f, 0); // Totalmente transparente, com a cor desejada
    }

    public void StartDeathFade(Action callback = null)
    {
        var fade = GetNode<ColorRect>("ColorRect");
        fade.Visible = true;

        // Cria o tween
        var tween = GetTree().CreateTween();
        
        // Anima a transparência de 0 para 1 (transparente → opaco)
        tween.TweenProperty(
            fade, "modulate", new Color(33f / 255f, 30f / 255f, 51f / 255f, 1), // Valor final (opaco) com a cor desejada
            0.5f // Duração
        );

        // Quando a animação terminar, chama o callback e recarrega a cena
        tween.TweenCallback(Callable.From(() =>
        {
            GD.Print("Tela escurecida. Recarregando cena...");
            callback?.Invoke();
            GetTree().ReloadCurrentScene();
        }));
    }

    public void StartFade(Action callback = null)
    {
        var fade = GetNode<ColorRect>("ColorRect");
        fade.Visible = true;

        // Cria o tween
        var tween = GetTree().CreateTween();
        
        // Anima a transparência de 0 para 1 (transparente → opaco)
        tween.TweenProperty(
            fade, "modulate", new Color(33f / 255f, 30f / 255f, 51f / 255f, 1), // Valor final (opaco) com a cor desejada
            0.5f // Duração
        );

        // Quando a animação terminar, chama o callback
        tween.TweenCallback(Callable.From(() =>
        {
            GD.Print("Tela escurecida.");
            callback?.Invoke();
            fade.Modulate = new Color(33f / 255f, 30f / 255f, 51f / 255f, 0); // Totalmente transparente, com a cor desejada
        }));
    }
}
