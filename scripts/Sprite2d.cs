using Godot;
using System;
public partial class Sprite2d : Sprite2D
{
    [Export] // Isso permite ajustar a velocidade no inspector
    public float Speed = 100f;

    public override void _Process(double delta)
    {
        // Obter entrada do teclado
        Vector2 input = Vector2.Zero;
        input.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        input.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        // Normalizar o vetor para movimento diagonal não ser mais rápido
        input = input.Normalized();

        // Mover o sprite
        Position += input * Speed * (float)delta;
    }
}