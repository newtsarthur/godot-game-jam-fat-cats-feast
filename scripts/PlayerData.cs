using Godot;
using System;

public partial class PlayerData : Node
{
    public static PlayerData Instance { get; private set; }

    public int ItemsCollectedTotal = 0;
    private int _itemsCollectedInLevel = 0;
    public int DeathCount = 0;
    public int Inicio = 0;

    [Signal]
    public delegate void ItemsCollectedInLevelChangedEventHandler(int newValue);

    public override void _Ready()
    {
        Instance = this;
    }

    public int ItemsCollectedInLevel
    {
        get => _itemsCollectedInLevel;
        set
        {
            _itemsCollectedInLevel = value;
            EmitSignal(nameof(ItemsCollectedInLevelChanged), _itemsCollectedInLevel);
        }
    }

    public void ResetLevelProgress()
    {
        ItemsCollectedInLevel = 0;
        Inicio = 1;
    }

    public void CommitLevelProgress()
    {
        ItemsCollectedTotal += ItemsCollectedInLevel;
        ItemsCollectedInLevel = 0;
    }
}
