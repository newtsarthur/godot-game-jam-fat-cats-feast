using Godot;
using System;

public partial class MovingPlatform : CharacterBody2D
{
    [Export]
    public float minY = 48.0f;

    [Export]
    public float maxY = 263.0f;

    [Export]
    public float speed = 50f;

    private bool movingDown = true;

    public override void _Ready()
    {
        Position = new Vector2(Position.X, 189.0f);
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print("Movendo..."); // debug
        float moveDistance = speed * (float)delta;
        Vector2 pos = Position;

        if (movingDown)
        {
            pos.Y += moveDistance;
            if (pos.Y >= maxY)
            {
                pos.Y = maxY;
                movingDown = false;
            }
        }
        else
        {
            pos.Y -= moveDistance;
            if (pos.Y <= minY)
            {
                pos.Y = minY;
                movingDown = true;
            }
        }

        Position = pos;
    }
}
