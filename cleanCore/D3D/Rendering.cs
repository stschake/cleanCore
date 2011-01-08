using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D9;

namespace cleanCore.D3D
{

    [StructLayout(LayoutKind.Sequential)]
    public struct PositionColored
    {
        public static readonly VertexFormat FVF = VertexFormat.Position | VertexFormat.Diffuse;
        public static readonly int Stride = Vector3.SizeInBytes + sizeof(int);

        public Vector3 Position;
        public int Color;

        public PositionColored(Vector3 pos, int col)
        {
            Position = pos;
            Color = col;
        }
    }

    public static class Rendering
    {
        private static readonly List<IResource> _resources = new List<IResource>();
        private static IntPtr _usedDevicePointer = IntPtr.Zero;

        public static Device Device { get; private set; }

        public static void Initialize(IntPtr devicePointer)
        {
            if (_usedDevicePointer != devicePointer)
            {
                Debug.WriteLine("Rendering: Device initialized on " + devicePointer);
                Device = Device.FromPointer(devicePointer);
                _usedDevicePointer = devicePointer;
            }
        }

        public static void RegisterResource(IResource source)
        {
            _resources.Add(source);
        }
        
        public static void DrawLine(Location from, Location to, Color4 color)
        {
            var vertices = new PositionColored[2];
            vertices[0] = new PositionColored(from.ToVector3(), color.ToArgb());
            vertices[1] = new PositionColored(to.ToVector3(), color.ToArgb());
            Device.DrawUserPrimitives(PrimitiveType.LineList, 1, vertices);
        }

        public static void OnLostDevice()
        {
            foreach (var resource in _resources)
                resource.OnLostDevice();
        }

        public static void OnResetDevice()
        {
            foreach (var resource in _resources)
                resource.OnResetDevice();
        }

        public static void Pulse()
        {
            if (!IsInitialized)
                return;
        }

        public static bool IsInitialized
        {
            get
            {
                return Device != null;
            }
        }
    }

}