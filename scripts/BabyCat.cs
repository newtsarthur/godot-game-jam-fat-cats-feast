using Godot;
using System;

public partial class BabyCat : CharacterBody2D
{
    [Export] public TileMapLayer Grid;
    [Export] public float MoveSpeed = 25f;
    [Export] public float Gravity = 0f;
    [Export] public float RayLength = 30f;

    private int _direction = 1;
    private RayCast2D _floorDetector;
    private RayCast2D _wallDetector;
    private Sprite2D _sprite;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        // Configuração dos RayCast2D
        _floorDetector = GetNodeOrNull<RayCast2D>("FloorDetector");
        _wallDetector = GetNodeOrNull<RayCast2D>("WallDetector");
        _sprite = GetNode<Sprite2D>("Sprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        // Configura as camadas de colisão para ignorar PlayerGroup e BabyCatGroup
        SetupCollisionLayers();

        // Garantir que os RayCasts sejam inicializados
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

        // Ignora objetos dos grupos especificados nos RayCasts
        foreach (Node node in GetTree().GetNodesInGroup("PlayerGroup"))
        {
            if (node is CollisionObject2D collisionObj)
            {
                _floorDetector.AddException(collisionObj);
                _wallDetector.AddException(collisionObj);
            }
        }

        foreach (Node node in GetTree().GetNodesInGroup("BabyCatGroup"))
        {
            if (node is CollisionObject2D collisionObj)
            {
                _floorDetector.AddException(collisionObj);
                _wallDetector.AddException(collisionObj);
            }
        }
    }

    private void SetupCollisionLayers()
    {
        // Obtém o parent CollisionObject2D (pode ser o próprio CharacterBody2D ou um filho)
        CollisionObject2D collisionObject = this; // Ou GetNode<CollisionObject2D>("...");

        // Configura as layers e masks para ignorar PlayerGroup e BabyCatGroup
        // Você precisará ajustar os números das camadas de acordo com seu projeto
        uint playerLayer = 1 << 0; // Supondo que PlayerGroup está na layer 1
        uint babyCatLayer = 1 << 1; // Supondo que BabyCatGroup está na layer 2

        // Desativa a detecção com essas camadas
        collisionObject.CollisionMask &= ~playerLayer;
        collisionObject.CollisionMask &= ~babyCatLayer;
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

        // Atualiza o sprite
        if (_sprite != null)
        {
            _sprite.FlipH = _direction > 0;
        }

        // Aplica a velocidade e movimento
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