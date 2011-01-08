using System.Diagnostics;
using SlimDX;
using SlimDX.Direct3D9;

namespace cleanCore.D3D
{
    
    public class Font : IResource
    {
        private SlimDX.Direct3D9.Font _font;

        public Font(int height, int width, string font)
        {
            _font = new SlimDX.Direct3D9.Font(Rendering.Device, height, width, FontWeight.Normal, 1, false,
                                              CharacterSet.Default, Precision.Default, FontQuality.Antialiased,
                                              PitchAndFamily.Default, font);

            Rendering.RegisterResource(this);
        }

        public void OnLostDevice()
        {
            if (_font.OnLostDevice() != ResultCode.Success)
                Debugger.Break();
        }

        public void OnResetDevice()
        {
            if (_font.OnResetDevice() != ResultCode.Success)
                Debugger.Break();
        }

        public void Release()
        {
            _font.Dispose();
            _font = null;
        }

        public void Print(int x, int y, string text, Color4 color)
        {
            if (_font != null)
                _font.DrawString(null, text, x, y, color);
        }
    }

}