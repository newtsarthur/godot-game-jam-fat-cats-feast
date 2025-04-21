using Godot;
using System;
using System.Threading.Tasks;

public partial class Fade : CanvasLayer
{
    public override void _Ready()
    {
        var fade = GetNode<ColorRect>("ColorRect");
        fade.Size = GetViewport().GetVisibleRect().Size;
        fade.Modulate = new Color(33f / 255f, 30f / 255f, 51f / 255f, 0); // Transparente
    }

    public void StartDeathFade(Action callback = null)
    {
        var fade = GetNode<ColorRect>("ColorRect");
        fade.Visible = true;

        var tween = GetTree().CreateTween();

        tween.TweenProperty(
            fade, "modulate",
            new Color(33f / 255f, 30f / 255f, 51f / 255f, 1),
            0.5f
        );

        tween.TweenCallback(Callable.From(() =>
        {
            GD.Print("Tela escurecida. Recarregando cena...");
            callback?.Invoke();
            GetTree().ReloadCurrentScene();
        }));
    }

    // 🔁 Versão awaitable do fade
    public async Task StartFade()
    {
        var fade = GetNode<ColorRect>("ColorRect");
        fade.Visible = true;

        var tcs = new TaskCompletionSource();

        var tween = GetTree().CreateTween();

        tween.TweenProperty(
            fade, "modulate",
            new Color(33f / 255f, 30f / 255f, 51f / 255f, 1),
            0.5f
        );

        tween.TweenCallback(Callable.From(() =>
        {
            GD.Print("Tela escurecida.");
            fade.Modulate = new Color(33f / 255f, 30f / 255f, 51f / 255f, 0);
            tcs.SetResult(); // finaliza a Task
        }));

        await tcs.Task;
    }
}
