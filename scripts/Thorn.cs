using Godot;

public partial class Thorn : Area2D
{
    [Export] public float FallSpeed = 300f;
    [Export] public TileMapLayer Reload;

    private bool _shouldFall = false;
    private Vector2 _originalPosition;
    // private int touchGround = 0;
    public bool HasTouchedGround { get; private set; } = false;
    public override void _Ready()
    {
        _originalPosition = Position;
        // Começa com colisão desativada
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
        
        if(!HasTouchedGround)
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
        HasTouchedGround = false;
    }

    public void ResetThorn()
    {
        _shouldFall = false;
        Position = _originalPosition;
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is TileMap || body.IsInGroup("Ground") || body.IsInGroup("Platform"))
        {
            _shouldFall = false;
            GD.Print("Thorn atingiu o chão/tilemap");
            HasTouchedGround = true;
            var buttonTwo = GetNode<Button2>("../Button2");
            if(buttonTwo.MouseDeath != 0)
            {
                Node cutsceneNode = GetNode("../Level Design/Cutscene");

                if (cutsceneNode is Cutscene cutscene)
                {
                    GetNode<Fade>("../Fade").StartFade();
                    cutscene.PlayCutscene();
                    GD.Print("Acabou o jogo");
                    GetNode<Fade>("../Fade").StartFade();
                }
                else
                {
                    GD.PrintErr("Falha ao encontrar ou fazer cast para Cutscene");
                    GD.PrintErr("Tipo real do nó: ", cutsceneNode?.GetType().ToString() ?? "null");
                }
            }else {
                GD.Print("Não acabou o jogo");
                Reload.Visible = true;
            }
        }
    }

}