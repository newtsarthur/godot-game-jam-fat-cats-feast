using Godot;
using System.Threading.Tasks;

public partial class Splashscreen : Control
{
    [Export]
    public PackedScene LoadScene { get; set; }

    [Export]
    public float InTime { get; set; } = 0.5f;

    [Export]
    public float FadeInTime { get; set; } = 1.5f;

    [Export]
    public float PauseTime { get; set; } = 1.5f;

    [Export]
    public float FadeOutTime { get; set; } = 1.5f;

    [Export]
    public float OutTime { get; set; } = 0.5f;

    [Export]
    public TextureRect SplashScreen { get; set; }

    public override void _Ready()
    {
        _ = Fade();
    }

    public async Task Fade()
    {
        SplashScreen.Modulate = new Color(SplashScreen.Modulate, a: 0.0f);

        var tween = CreateTween();

        tween.TweenInterval(InTime);
        tween.TweenProperty(SplashScreen, "modulate:a", 1.0f, FadeInTime);
        tween.TweenInterval(PauseTime);
        tween.TweenProperty(SplashScreen, "modulate:a", 0.0f, FadeOutTime);
        tween.TweenInterval(OutTime);

        await ToSignal(tween, Tween.SignalName.Finished);

         GetTree().ChangeSceneToFile("res://node_2d.tscn");
    }
}
