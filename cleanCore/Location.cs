using System;
using System.Runtime.InteropServices;

namespace cleanCore
{
    
    [StructLayout(LayoutKind.Sequential)]
    public struct Location
    {
        public float X;
        public float Y;
        public float Z;

        public Location(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double DistanceTo(Location loc)
        {
            return Math.Sqrt(Math.Pow(X - loc.X, 2) + Math.Pow(Y - loc.Y, 2) + Math.Pow(Z - loc.Z, 2));
        }

        public double Distance2D(Location loc)
        {
            return Math.Sqrt(Math.Pow(X - loc.X, 2) + Math.Pow(Y - loc.Y, 2));
        }

        public double Length
        {
            get { return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2)); }
        }

        public Location Normalize()
        {
            var len = Length;
            return new Location((float)(X / len), (float)(Y / len), (float)(Z / len));
        }

        public float Angle
        {
            get
            {
                return (float)Math.Atan2(Y, X);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var loc = (Location) obj;
            if (loc.X != X || loc.Y != Y || loc.Z != Z)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() | Y.GetHashCode() | Z.GetHashCode();
        }

        public override string ToString()
        {
            return "[" + (int) X + ", " + (int) Y + ", " + (int) Z + "]";
        }
    }

}