using Godot;
using System;

public partial class Cutscene : Node2D
{
    private AnimationPlayer animationPlayer;

    [Export] public string animationName;
    [Export] public bool autoplay = false;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        if (autoplay)
        {
            PlayCutscene();
        }
    }

    public void PlayCutscene()
    {
        if (animationPlayer != null && animationPlayer.HasAnimation(animationName))
        {
            animationPlayer.AnimationFinished += OnAnimationFinished;
            animationPlayer.Play(animationName);
        }
        else
        {
            GD.PrintErr("AnimationPlayer não encontrado ou animação inválida.");
        }
    }
    private void OnAnimationFinished(StringName name)
{
    if (name == animationName)
    {
        GD.Print("Animação terminou, emitindo sinal...");
        EmitSignal(SignalName.CutsceneFinished);
    }
}
    [Signal]
    public delegate void CutsceneFinishedEventHandler();

}
