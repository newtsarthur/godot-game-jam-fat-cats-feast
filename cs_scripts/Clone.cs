using Godot;
using System;

public partial class Clone : CharacterBody2D
{
    [Export] public float MoveSpeed = 150f; // Mesma velocidade do player
    private Vector2 _velocity;
    
    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;
        
        // Atualiza a velocidade baseada no movimento atual
        _velocity = Velocity;
        
        // MOVIMENTO HORIZONTAL
        float direction = Mathf.Sign(_velocity.X); // Pega apenas a direção (-1, 0 ou 1)
        
        // Flipar o sprite dependendo da direção
        if (Mathf.Abs(direction) > 0.1f) // Pequena tolerância
        {
            AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("Anim");
            sprite.FlipH = direction > 0;
            GD.Print($"Clone flip: {sprite.FlipH}"); // Debug
        }
        
        // Aplica a velocidade e move
        Velocity = _velocity;
        MoveAndSlide();
    }

    public override void _Ready()
    {
        // Garante que está no grupo certo
        AddToGroup("PlayerGroup");
        
        // Debug para verificar
        GD.Print($"Clone pronto: {Name} | Grupo: {IsInGroup("PlayerGroup")}");
        
        // Inicializa o flip igual ao do player
        var players = GetTree().GetNodesInGroup("PlayerGroup");
        foreach (var player in players)
        {
            if (player is Player mainPlayer && player != this)
            {
                AnimatedSprite2D playerSprite = mainPlayer.GetNode<AnimatedSprite2D>("Anim");
                GetNode<AnimatedSprite2D>("Anim").FlipH = playerSprite.FlipH;
                break;
            }
        }
        
        // Força atualização da física
        ForceUpdateTransform();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
}