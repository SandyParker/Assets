
using UnityEngine;

public class PointInTime
{
    public Vector2 Position;
    public Quaternion Rotation;
    public float HP;
    public bool Running;
    public bool Jumping;
    public bool Double;
    public bool Falling;
    public bool Wall;
    public bool Climb;
    public bool Attack1;
    public bool Attack2;
    public bool Attack3;
    public bool Dash;

    public PointInTime(Vector2 position, Quaternion rotation, float hp, bool running, bool jumping, bool doble, bool falling, bool wall, bool climb, bool attack1, bool attack2, bool attack3, bool dash)
    {
        Position = position;
        Rotation = rotation;
        HP = hp;
        Running = running;
        Jumping = jumping;
        Double = doble;
        Falling = falling;
        Wall = wall;
        Climb = climb;
        Attack1 = attack1;
        Attack2 = attack2;
        Attack3 = attack3;
        Dash = dash;
    }
}
