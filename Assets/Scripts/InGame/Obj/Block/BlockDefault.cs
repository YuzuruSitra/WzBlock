
namespace InGame.Obj.Block
{
    public class BlockDefault : BlockBase
    {
        protected override void HitBall()
        {
            base.HitBall();
            SoundHandler.PlaySe(_hitSound);
        }
    }
}