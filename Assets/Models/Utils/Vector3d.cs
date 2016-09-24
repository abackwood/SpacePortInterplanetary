using UnityEngine;

public struct Vector3d
{
    public static Vector3d zero
    {
        get
        {
            return new Vector3d(0,0,0);
        }
    }

    public static Vector3d one
    {
        get
        {
            return new Vector3d(1,1,1);
        }
    }

    public double x { get; set; }
    public double y { get; set; }
    public double z { get; set; }

    public Vector3d(double x, double y, double z) : this()
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType().Equals(typeof(Vector3d)) == false)
        {
            return false;
        }
        else
        {
            Vector3d v = (Vector3d)obj;
            return x == v.x && y == v.y && z == v.z;
        }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(Vector3d a, Vector3d b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector3d a, Vector3d b)
    {
        return a.Equals(b) == false;
    }

    public static Vector3d operator +(Vector3d a, Vector3d b)
    {
        return new Vector3d(
            a.x + b.x,
            a.y + b.y,
            a.z + b.z
        );
    }

    public static Vector3d operator -(Vector3d a, Vector3d b)
    {
        return new Vector3d(
            a.x - b.x,
            a.y - b.y,
            a.z - b.z
        );
    }

    public static Vector3d operator *(Vector3d v, double s)
    {
        return new Vector3d(
            v.x * s,
            v.y * s,
            v.z * s
        );
    }

    public static Vector3d operator *(double s, Vector3d v)
    {
        return v * s;
    }

    public static Vector3d operator /(Vector3d v, double s)
    {
        return new Vector3d(
            v.x / s,
            v.y / s,
            v.z / s
        );
    }

    public static implicit operator Vector3(Vector3d v)
    {
        return new Vector3(
            (float)v.x,
            (float)v.y,
            (float)v.z
        );
    }
}