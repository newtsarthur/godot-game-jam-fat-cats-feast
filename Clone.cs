using Godot;
using System;

public partial class Clone : CharacterBody2D
{
  public override void _PhysicsProcess(double delta)
  {
    Velocity = Vector2.Zero;
  }
  public override void _Ready()
  {
      // Garante que está no grupo certo
      AddToGroup("PlayerGroup");
      
      // Debug para verificar
      GD.Print($"Clone pronto: {Name} | Grupo: {IsInGroup("PlayerGroup")}");
      
      // Força atualização da física
      ForceUpdateTransform();
      GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
  }
}
