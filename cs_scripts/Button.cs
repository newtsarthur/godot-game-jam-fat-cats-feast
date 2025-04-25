using Godot;
using System;

public partial class Button : Area2D
{
    [Export] public TileMapLayer Chao; // chão que será ativado
    [Export] public TileMapLayer Dica1;
    [Export] public TileMapLayer Reload;

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
        }
        if( Dica1 != null)
        {
            Dica1.Visible = false;
        }
        if( Reload != null )
        {
            Reload.Visible = false;
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("PlayerGroup"))
        {
            _playersInArea++;
            GD.Print(CanHelp);
            AtivarBotao();
            GD.Print($"Entrou: {body.Name} | Total na área: {_playersInArea}");
            CanHelp++;
            var player = GetNode<Player>("../Player");
            if( player.ClonesCurrent != 0 && Chao.Enabled == true && _playersInArea == 1)
            {
                Dica1.Visible = false;
            }else if( CanHelp == 2 && player.ClonesCurrent == 0 )
            {
                GD.Print("Posso ajudalo");
                Reload.Visible = false;
                Dica1.Visible = true;
            }
            if ( player.ClonesCurrent == player.ClonesMax)
            {
                Dica1.Visible = false;
            }
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
                if ( player.ClonesCurrent == player.ClonesMax && Chao.Enabled == false && _playersInArea == 0)
                {
                    Reload.Visible = true;
                }
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
            //GD.Print("Funciona");
    }

    //Desativa o chão invisível
    private void DesativarBotao() 
    {
        // animação de pressionar o botão
        sprite.Play("ButtonDefault"); 

        if (Chao != null)
            Chao.Enabled = false;
            //GD.Print("Desativado");
    }
}
