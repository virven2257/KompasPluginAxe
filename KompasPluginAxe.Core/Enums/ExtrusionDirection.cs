using Kompas6Constants3D;

namespace KompasPluginAxe.Core.Enums
{
    /// <summary>
    /// Направление выдавливания
    /// </summary>
    public enum ExtrusionDirection
    {   
        Direct = Direction_Type.dtNormal,
        Reverse = Direction_Type.dtReverse,
        MiddlePlane = Direction_Type.dtMiddlePlane,
        Both = Direction_Type.dtBoth
        
    }
}