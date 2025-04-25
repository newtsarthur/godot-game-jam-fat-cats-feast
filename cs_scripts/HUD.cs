using Godot;

public partial class HUD : Node2D  // Use CanvasLayer se quiser que a UI fique fixa na tela
{
    [Export] private Label meuLabel;  // Arraste o Label no editor
    private Player _player;

    public override void _Ready()
    {
        // Só mostra "0" até o SetPlayer ser chamado
        meuLabel.Text = "0";
    }

    public void SetPlayer(Player player)
    {
        _player = player;

        if (_player != null)
        {
            _player.PontuacaoAtualizada += OnPontuacaoAtualizada;
            _player.ClonesAtualizados += OnClonesAtualizados;

            UpdateClonesDisplay();
            GD.Print($"ClonesCurrent: {_player.ClonesCurrent}, ClonesMax: {_player.ClonesMax}");
        }
        else
        {
            GD.PrintErr("SetPlayer recebeu null!");
        }
    }

    private void OnPontuacaoAtualizada(int novaPontuacao)
    {
        // Se for exibir pontuação depois, ativa isso:
        // meuLabel.Text = novaPontuacao.ToString();
    }

    private void OnClonesAtualizados(int clonesDisponiveis)
    {
        UpdateClonesDisplay();
    }

    private void UpdateClonesDisplay()
    {
        if (_player != null && meuLabel != null)
        {
            GD.Print("MaxClones: " + _player.MaxClones);
            GD.Print("ClonesCurrent: " + _player._currentClones);

            int clonesRestantes = _player.MaxClones - _player._currentClones;
            meuLabel.Text = clonesRestantes.ToString();
        }
    }

    public override void _ExitTree()
    {
        if (_player != null)
        {
            _player.PontuacaoAtualizada -= OnPontuacaoAtualizada;
            _player.ClonesAtualizados -= OnClonesAtualizados;
        }
    }
}
