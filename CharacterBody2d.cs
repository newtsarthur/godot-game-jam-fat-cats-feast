using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
    [Export] public PackedScene Clone;
    [Export] public int MaxClones = 1;

    [Export] public float MoveSpeed = 150f;
    [Export] public float JumpForce = 350f;
    [Export] public float Gravity = 1000f;
    [Export] public float MaxFallSpeed = 1000f;
    [Export] public float JumpCutMultiplier = 0.5f; // Soltar o pulo "corta" o impulso

    //Velocidade
    private Vector2 _velocity;

    //Contador atual
    private int _currentClones = 0;

    //Lista para gerenciar clones
    private List<Clone> _activeClones = new();

    public override void _PhysicsProcess(double delta)
    {
        // GD.Print("IsOnFloor: " + IsOnFloor() + " Velocity: " + _velocity);
        float dt = (float)delta;

        // MOVIMENTO HORIZONTAL
        float direction = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
        _velocity.X = direction * MoveSpeed;

        // Flipar o sprite dependendo da direção
        if (direction != 0)
        {
            // Pega o Sprite2D (ou AnimatedSprite2D)
            AnimatedSprite2D sprite = GetNode<AnimatedSprite2D>("Anim");
            sprite.FlipH = direction > 0;
        }

        // APLICAR GRAVIDADE
        if (!IsOnFloor())
        {
            _velocity.Y += Gravity * dt;

            // Limitar a velocidade de queda
            if (_velocity.Y > MaxFallSpeed)
                _velocity.Y = MaxFallSpeed;
        }
        else
        {
            // Resetar a velocidade de queda ao tocar o chão
            _velocity.Y = 0;

            // PULO
            if (Input.IsActionJustPressed("jump"))
            {
                _velocity.Y = -JumpForce;
            }

            //Verifica se o shift foi pressionado
            if(IsOnFloor() && Input.IsActionJustPressed("summon_clone"))
            {
                SummonClone();
            }
        }

        // CORTE DE PULO (estilo Hollow Knight)
        if (Input.IsActionJustReleased("jump") && _velocity.Y < 0)
        {
            _velocity.Y *= JumpCutMultiplier;
        }

        if (Input.IsActionJustPressed("reload"))
        {
            ReloadScene();
        }
        
        Velocity = _velocity;
        MoveAndSlide();
    }

    //Recarrega a cena atual
    private void ReloadScene()
    {
        string currentScenePath = GetTree().CurrentScene.SceneFilePath;

        GetTree().ChangeSceneToFile(currentScenePath);

        GD.Print("Cena recarregada");
    }

    //Summona um clone onde o player está.
    private void SummonClone()
    {
        //Verifica se pode criar mais clones
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

        // 1. Instancia corretamente mantendo o tipo original
        var newClone = Clone.Instantiate();
        if(newClone != null)
        {
            _currentClones++;
        }
        
        // 2. Verifica e converte o tipo adequadamente
        if (newClone is CharacterBody2D characterBody)
        {
            // Configuração do clone
            characterBody.Name = "PlayerClone";
            characterBody.AddToGroup("PlayerGroup");
            characterBody.GlobalPosition = GlobalPosition;
            
            // Copia o flip do sprite do jogador para o clone
            AnimatedSprite2D playerSprite = GetNode<AnimatedSprite2D>("Anim");
            if (characterBody.HasNode("Anim"))
            {
                AnimatedSprite2D cloneSprite = characterBody.GetNode<AnimatedSprite2D>("Anim");
                cloneSprite.FlipH = playerSprite.FlipH;
            }
            
            // Adiciona à cena
            GetParent().AddChild(characterBody);
            
            // Força atualização física
            characterBody.ForceUpdateTransform();
            
            GD.Print($"Clone CharacterBody2D criado: {characterBody.Name}, Flip: {playerSprite.FlipH}");
        }
        else if (newClone is Node2D node2d)
        {
            // Configuração do clone
            node2d.Name = "PlayerClone";
            node2d.AddToGroup("PlayerGroup");
            node2d.GlobalPosition = GlobalPosition;
            
            // Copia o flip do sprite do jogador para o clone
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
    private void ConfigureCloneCollision(Node2D clone)
    {
        // Garante que está no grupo correto
        clone.AddToGroup("PlayerGroup");
        
        // Debug para verificar
        GD.Print($"Clone instanciado - Pos: {clone.GlobalPosition}, Grupos: {string.Join(", ", clone.GetGroups())}");
    }
}
