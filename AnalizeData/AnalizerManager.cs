namespace AnalizeData
{
    public class AnalizerManager : IAnalizerManager
    {  
        public double PrintSpeed(double seconds, int dataCount)
            => dataCount / seconds;
    }

    public interface IAnalizerManager
    {
        double PrintSpeed(double instantSecond, int dataCount);
    }
}