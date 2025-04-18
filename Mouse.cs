using Godot;
using System;

public partial class Mouse : CharacterBody2D
{
    [Export] public float MoveSpeed = 30f;
    [Export] public float Gravity = 980f;
    [Export] public float RayLength = 30f;
    
    private int _direction = 1;
    private RayCast2D _floorDetector;
    private RayCast2D _wallDetector;
    private AnimatedSprite2D _sprite;
    
    public override void _Ready()
    {
        _floorDetector = GetNodeOrNull<RayCast2D>("FloorDetector");
        _wallDetector = GetNodeOrNull<RayCast2D>("WallDetector");
        _sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        
        if (_floorDetector == null)
        {
            _floorDetector = new RayCast2D();
            _floorDetector.Name = "FloorDetector";
            AddChild(_floorDetector);
            _floorDetector.TargetPosition = new Vector2(RayLength * _direction, 20);
            _floorDetector.Enabled = true;
        }
        
        if (_wallDetector == null)
        {
            _wallDetector = new RayCast2D();
            _wallDetector.Name = "WallDetector";
            AddChild(_wallDetector);
            _wallDetector.TargetPosition = new Vector2(RayLength * _direction, 0);
            _wallDetector.Enabled = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        
        // Aplica gravidade
        if (!IsOnFloor())
        {
            velocity.Y += Gravity * (float)delta;
        }
        else
        {
            velocity.Y = 0;
        }

        // Movimento horizontal
        velocity.X = MoveSpeed * _direction;
        
        // Verifica colisões
        UpdateRaycasts();
        CheckForTurn();
        
        // Atualiza sprite
        if (_sprite != null)
        {
            _sprite.FlipH = _direction > 0; // Invertido para direção correta
            _sprite.Play("walk");
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void UpdateRaycasts()
    {
        if (_floorDetector != null)
        {
            _floorDetector.TargetPosition = new Vector2(RayLength * _direction, 20);
            _floorDetector.ForceRaycastUpdate();
        }
        
        if (_wallDetector != null)
        {
            _wallDetector.TargetPosition = new Vector2(RayLength * _direction, 0);
            _wallDetector.ForceRaycastUpdate();
        }
    }

    private void CheckForTurn()
    {
        bool shouldTurn = false;
        
        if (_floorDetector != null && !_floorDetector.IsColliding())
        {
            shouldTurn = true;
        }
        
        if (_wallDetector != null && _wallDetector.IsColliding())
        {
            shouldTurn = true;
        }
        
        if (shouldTurn)
        {
            _direction *= -1;
            UpdateRaycasts();
        }
    }
}