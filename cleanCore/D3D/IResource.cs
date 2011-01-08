namespace cleanCore.D3D
{

    public interface IResource
    {
        void OnLostDevice();
        void OnResetDevice();
    }

}