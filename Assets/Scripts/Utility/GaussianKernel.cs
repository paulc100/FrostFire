using System;

public class GaussianKernel
{
    public static float[] Calculate(double sigma, int size)
    {
        float[] ret = new float[size]; 
        double sum = 0;
        int half = size / 2;
        for (int i = 0; i < size; i++)
        {
            ret[i] = (float) (1 / (Math.Sqrt(2 * Math.PI) * sigma) * Math.Exp(-(i - half) * (i - half) / (2 * sigma * sigma)));
            sum += ret[i];
        }
        return ret;
    }
}
