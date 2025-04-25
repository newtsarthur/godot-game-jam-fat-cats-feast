using Godot;
using System;

public partial class Scene5 : Node2D
{
    [Export] public AnimationPlayer animated;
    [Export] public AnimationPlayer animatedtw;

    private bool isSkipping = false;


    [Signal]
    public delegate void CutsceneFinishedEventHandler();

    public override void _Ready()
    {
      if(PlayerData.Instance.Inicio == 0)
      {
        animatedtw.Play("initial1");
        var playerNode = GetNode<CharacterBody2D>("Player") as Player;
        if (playerNode != null)
        {
            playerNode.MoveSpeed = 0;
            playerNode.JumpForce = 0;
        }
        // Conecta o sinal de fim de animação
        animatedtw.AnimationFinished += OnAnimationFinished;  
        PlayerData.Instance.Inicio = 1;
      }
      else {
            animated.Play("light");
      }
        // Começa a animação assim que o jogo inicia

    }

    private void OnAnimationFinished(StringName animName)
    {
        GD.Print($"Animação '{animName}' terminou.");
        animatedtw.Stop();
        animated.Play("light");
        var playerNode = GetNode<CharacterBody2D>("Player") as Player;
        if (playerNode != null)
        {
            playerNode.MoveSpeed = 100;
            playerNode.JumpForce = 300;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("cancel")) // 'cancel' é o nome da ação no Input Map
        {
            if (!isSkipping)
            {
                isSkipping = true;

                GD.Print("Animação pulada!");

                // Parar as animações se necessário
                animatedtw.Stop();

                // Emite o sinal de término da cutscene
                EmitSignal(SignalName.CutsceneFinished);
                animated.Play("light");
                var playerNode = GetNode<CharacterBody2D>("Player") as Player;
                if (playerNode != null)
                {
                    playerNode.MoveSpeed = 100;
                    playerNode.JumpForce = 300;
                }
            }
        }
    }
}
