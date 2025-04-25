using Godot;
using System;

public partial class Button5 : Area2D
{
    [Export] public TileMapLayer Chao; // chão que será ativado
    [Export] public Thorn3 Thorn;
    private AnimatedSprite2D sprite;
    private int _playersInArea = 0;

    public int CanHelp { get; private set; } = 0;
    
    public override void _Ready()
    {
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

        //Conecta sinais de entrada e saída
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;

        if (Chao != null)
        {
            Chao.Enabled = false;
            Thorn.Visible = false;
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("PlayerGroup"))
        {
            _playersInArea++;
            AtivarBotao();
            GD.Print($"Entrou: {body.Name} | Total na área: {_playersInArea}");
            var player = GetNode<Player>("../Player");
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body.IsInGroup("PlayerGroup"))
        {
            _playersInArea--;
            
            if (_playersInArea <= 0)
            {
                // Garante que não fique negativo
                _playersInArea = 0; 
                DesativarBotao();
                GD.Print($"Saiu: {body.Name} | Botão desativado (0 players na área)");
                var player = GetNode<Player>("../Player");

            }
            else
            {
                GD.Print($"Saiu: {body.Name} | Ainda há {_playersInArea} players na área");
            }
        }
    }
    //Ativa o chão invisível
    private void AtivarBotao()
    { 
        // animação de pressionar o botão
        sprite.Play("ButtonClicked");
        if (Chao != null)
            Chao.Enabled = true;
            Thorn.Visible = true;
            //GD.Print("Funciona");
    }

    //Desativa o chão invisível
    private void DesativarBotao() 
    {
        // animação de pressionar o botão
        sprite.Play("ButtonDefault"); 

        if (Chao != null)
            Chao.Enabled = false;
            Thorn.Visible = false;
            //GD.Print("Desativado");
    }
}
