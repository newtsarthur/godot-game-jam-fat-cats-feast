using Godot;

public partial class Thorn : Area2D
{
    [Export] public float FallSpeed = 300f;
    private bool _shouldFall = false;
    private Vector2 _originalPosition;
    private int touchGround = 0;
    public override void _Ready()
    {
        _originalPosition = Position;
        // Começa com colisão desativada
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
        
        if(touchGround == 0)
        {
          // Conecta o sinal de colisão
          BodyEntered += OnBodyEntered;
        }

    }

    public override void _PhysicsProcess(double delta)
    {
        if (_shouldFall)
        {
            Position += new Vector2(0, FallSpeed * (float)delta);
        }
    }

    public void StartFalling()
    {
        _shouldFall = true;
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);
        touchGround = 1;
    }

    public void ResetThorn()
    {
        _shouldFall = false;
        Position = _originalPosition;
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }

    private void OnBodyEntered(Node2D body)
    {
        // Para de cair quando colide com qualquer coisa (incluindo TileMap com collisor)
        if (body is TileMap || body.IsInGroup("Ground") || body.IsInGroup("Platform"))
        {
            _shouldFall = false;
            GD.Print("Thorn atingiu o chão/tilemap");
            touchGround = 1;
        }
    }
}