namespace GachaGame.Azure.Core.Interfaces;
public interface IBannerRoller<in TBanner, out TResult>
{
    public TResult RollBanner(TBanner banner);
}