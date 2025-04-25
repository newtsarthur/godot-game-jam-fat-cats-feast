using Godot;
using System;

public partial class Cutscene : Node2D  // Certifique-se de herdar de Node2D
{
    private AnimationPlayer animationPlayer;
    private bool isSkipping = false;

    [Export] public string animationName;
    [Export] public bool autoplay = false;

    [Signal]
    public delegate void CutsceneFinishedEventHandler();

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

    // Aqui está o método _Process correto, para detectar as ações de teclas
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("cancel")) // 'ui_cancel' corresponde à tecla 'TAB' por padrão
        {
            if (!isSkipping)
            {
                isSkipping = true; // Impede múltiplos saltos consecutivos
                GD.Print("Animação pulada!");

                // Emite o sinal de término da cutscene
                EmitSignal(SignalName.CutsceneFinished);
            }
        }
    }
}
