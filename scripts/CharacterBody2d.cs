using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    [Export] public PackedScene Clone;
    [Export] public int MaxClones = 1;

    [Export] public float MoveSpeed { get; set; } = 150f;
    [Export] public float JumpForce = 350f;
    [Export] public float Gravity = 1000f;
    [Export] public float MaxFallSpeed = 1000f;
    [Export] public float JumpCutMultiplier = 0.5f;

    private Vector2 _velocity;
    private int _currentClones = 0;
    public int ClonesCurrent { get; private set; } = 0;
    public int ClonesMax { get; private set; } = 0;

    private List<Clone> _activeClones = new();

    private bool _isDead = false;

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        // MOVIMENTO HORIZONTAL
        float direction = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        _velocity.X = direction * MoveSpeed;

        // Flipar o sprite dependendo da direção
        if (direction != 0)
        {
            AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("Anim");
            sprite.FlipH = direction > 0;
        }

        // APLICAR GRAVIDADE
        if (!IsOnFloor())
        {
            _velocity.Y += Gravity * dt;

            if (_velocity.Y > MaxFallSpeed)
                _velocity.Y = MaxFallSpeed;
        }
        else
        {
            _velocity.Y = 0;

            // PULO
            if (Input.IsActionJustPressed("jump"))
            {
                _velocity.Y = -JumpForce;
            }

            // Verifica se o shift foi pressionado
            if (IsOnFloor() && Input.IsActionJustPressed("summon_clone"))
            {
                SummonClone();
            }
        }

        // CORTE DE PULO
        if (Input.IsActionJustReleased("jump") && _velocity.Y < 0)
        {
            _velocity.Y *= JumpCutMultiplier;
        }

        if (Input.IsActionJustPressed("reload"))
        {
            GetNode<Fade>("../Fade").StartDeathFade(OnDeathFadeComplete);
        }

        Velocity = _velocity;
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            if (_isDead) break; // já morreu, evita múltiplos triggers

            var collision = GetSlideCollision(i);
            if (collision.GetCollider() is Node body && body.IsInGroup("EnemyGroup"))
            {
                _isDead = true;
                PlayerData.Instance.DeathCount++;
                GD.Print("Você morreu " + PlayerData.Instance.DeathCount + " vezes!");
                GD.Print("Player morreu");

                GetNode<Fade>("../Fade").StartDeathFade(OnDeathFadeComplete);
            }
        }
        ClonesCurrent = _currentClones;
        ClonesMax = MaxClones;
    }

    // Callback quando o fade terminar
    private void OnDeathFadeComplete()
    {
        GD.Print("Fade completo. Recarregando cena...");
        ReloadScene();
    }

    private void ReloadScene()
    {
        string currentScenePath = GetTree().CurrentScene.SceneFilePath;
        GetTree().ChangeSceneToFile(currentScenePath);
        GD.Print("Cena recarregada");
    }

    private void SummonClone()
    {
        if (_currentClones >= MaxClones)
        {
            GD.Print("Limite de clones atingido");
            return;
        }

        if (Clone == null)
        {
            GD.PrintErr("Cena do clone não atribuída!");
            return;
        }

        var newClone = Clone.Instantiate();
        if (newClone != null)
        {
            _currentClones++;
        }

        if (newClone is CharacterBody2D characterBody)
        {
            characterBody.Name = "PlayerClone";
            characterBody.AddToGroup("PlayerGroup");
            characterBody.GlobalPosition = GlobalPosition;

            AnimatedSprite2D playerSprite = GetNode<AnimatedSprite2D>("Anim");
            if (characterBody.HasNode("Anim"))
            {
                AnimatedSprite2D cloneSprite = characterBody.GetNode<AnimatedSprite2D>("Anim");
                cloneSprite.FlipH = playerSprite.FlipH;
            }

            GetParent().AddChild(characterBody);
            characterBody.ForceUpdateTransform();
            GD.Print($"Clone CharacterBody2D criado: {characterBody.Name}, Flip: {playerSprite.FlipH}");
        }
        else if (newClone is Node2D node2d)
        {
            node2d.Name = "PlayerClone";
            node2d.AddToGroup("PlayerGroup");
            node2d.GlobalPosition = GlobalPosition;

            AnimatedSprite2D playerSprite = GetNode<AnimatedSprite2D>("Anim");
            if (node2d.HasNode("Anim"))
            {
                AnimatedSprite2D cloneSprite = node2d.GetNode<AnimatedSprite2D>("Anim");
                cloneSprite.FlipH = playerSprite.FlipH;
            }

            GetParent().AddChild(node2d);
            node2d.ForceUpdateTransform();
            GD.Print($"Clone Node2D criado: {node2d.Name}, Flip: {playerSprite.FlipH}");
        }
        else
        {
            GD.PrintErr($"Tipo de clone não suportado: {newClone.GetType()}");
            return;
        }
    }
}
