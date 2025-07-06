using Microsoft.Xna.Framework;

namespace SWEN_Game._Interfaces
{
    public interface IPlayer
    {
        Vector2 RealPos { get; }
        float Speed { get; set; }
    }
}