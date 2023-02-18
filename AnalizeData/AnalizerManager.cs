namespace AnalizeData
{
    public class AnalizerManager : IAnalizerManager
    {  
        public double PrintSpeed(double instantSecond, int dataCount)
            => dataCount / instantSecond;
    }

    public interface IAnalizerManager
    {
        double PrintSpeed(double instantSecond, int dataCount);
    }
}