using System.Numerics;

public static class BigIntegerExtensions
{
    public static BigInteger GCD(BigInteger a, BigInteger b)
    {
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);
        
        if (a.IsZero) return b;
        if (b.IsZero) return a;
        if (a == b) return a;
        
        // 提取公因子2
        int shift = 0;
        while (a.IsEven && b.IsEven)
        {
            a >>= 1;
            b >>= 1;
            shift++;
        }
        
        // Stein算法主循环
        while (!a.IsZero && !b.IsZero)
        {
            while (a.IsEven) a >>= 1;
            while (b.IsEven) b >>= 1;
            
            if (a > b)
            {
                var temp = a;
                a = b;
                b = temp;
            }
            
            b -= a;
        }
        
        return (a.IsZero ? b : a) << shift;
    }
}