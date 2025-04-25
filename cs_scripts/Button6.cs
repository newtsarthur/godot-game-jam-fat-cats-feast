using Godot;
using System;

public partial class Button6 : Area2D
{
    [Export] public Thorn2 Thorn;
    private AnimatedSprite2D _sprite;
    private int _playersInArea = 0;
    public int MouseDeath {get; set;} = 0;
    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("PlayerGroup"))
        {
            _playersInArea++;
            if (_playersInArea == 1) AtivarBotao();
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body.IsInGroup("PlayerGroup"))
        {
            _playersInArea = Mathf.Max(0, _playersInArea - 1);
            if (_playersInArea == 0) DesativarBotao();
        }
    }

    private void AtivarBotao()
    {
        _sprite.Play("ButtonClicked");
        Thorn?.StartFalling();
    }

    private void DesativarBotao()
    {
        _sprite.Play("ButtonDefault");
        // Não faz nada com o thorn aqui
    }
}