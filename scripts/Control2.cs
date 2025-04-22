using Godot;
using System;

public partial class Control2 : Node2D
{
    [Export] private Label meuLabel;  // Arraste o Label no editor
    private Player _player;

    public override void _Ready()
    {
        // Conecta ao novo sinal (vamos adicionar esse sinal no PlayerData já já)
        PlayerData.Instance.Connect("ItemsCollectedInLevelChanged", new Callable(this, nameof(OnItemsCollectedChanged)));
        
        UpdateClonesDisplay(); // Mostra o valor inicial
    }

    private void OnItemsCollectedChanged(int newValue)
    {
        UpdateClonesDisplay();
    }

    private void UpdateClonesDisplay()
    {
        if (meuLabel != null)
        {
            // Mostra apenas os coletados nesta fase
            var points = PlayerData.Instance.ItemsCollectedTotal + PlayerData.Instance.ItemsCollectedInLevel;
            meuLabel.Text = points.ToString();
        }
        else
        {
            GD.PrintErr("Label não atribuído no HUD!");
        }
    }
}
