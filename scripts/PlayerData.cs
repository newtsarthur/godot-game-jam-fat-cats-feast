using Godot;
using System;

public partial class PlayerData : Node
{
    public static PlayerData Instance { get; private set; }

    public int DeathCount = 0;
    public int ItemsCollected = 0;

    public override void _Ready()
    {
        Instance = this;
    }
}
