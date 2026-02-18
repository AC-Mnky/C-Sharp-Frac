using System.Numerics;
using System;

public class Frac
{
    // 所有public方法输出的Frac对象都保证分母非负且与分子互素
    private BigInteger x;
    private BigInteger y;

    private void simplify()
    {
        var gcd = BigIntegerExtensions.GCD(x, y);
        if (gcd.IsZero)
        {
            return;
        }

        x /= gcd;
        y /= gcd;

        if (y.Sign < 0)
        {
            x = -x;
            y = -y;
        }
    }

    public BigInteger Numerator
    {
        get => x;
        set
        {
            x = value;
            simplify();
        }
    }

    public BigInteger Denominator
    {
        get => y;
        set
        {
            y = value;
            simplify();
        }
    }

    public Frac(BigInteger x)
    {
        this.x = x;
        this.y = 1;
    }

    public Frac(BigInteger x, BigInteger y)
    {
        this.x = x;
        this.y = y;
        simplify();
    }

    private Frac(BigInteger x, BigInteger y, bool should_simplify) // 如果 should_simplify 为 false，则只保证分母非负，不保证与分子互素
    {
        this.x = x;
        this.y = y;
        if (should_simplify)
        {
            simplify();
        }
        else if (y.Sign < 0)
        {
            this.x = -x;
            this.y = -y;
        }
    }

    public Frac reciprocal()
    {
        return new Frac(y, x, false);
    }

    public Frac neg()
    {
        return new Frac(-x, y, false);
    }

    public Frac mul(Frac other)
    {
        var gcd1 = BigIntegerExtensions.GCD(x, other.y);
        var gcd2 = BigIntegerExtensions.GCD(other.x, y);
        if (gcd1.IsZero || gcd2.IsZero)
        {
            return new Frac(0, 0, false);
        }
        return new Frac((x / gcd1) * (other.x / gcd2), (y / gcd2) * (other.y / gcd1), false);
    }

    public Frac div(Frac other)
    {
        return mul(other.reciprocal());
    }

    public Frac add(Frac other)
    {
        var gcd = BigIntegerExtensions.GCD(y, other.y);
        if (gcd.IsZero)
        {
            if (x.IsOne && other.x.IsOne)
                return new Frac(1, 0, false);
            else if (x == -1 && other.x == -1)
                return new Frac(-1, 0, false);
            else
                return new Frac(0, 0, false);
        }

        var m1 = y / gcd;
        var m2 = other.y / gcd;
        var numerator = x * m2 + other.x * m1;
        var denominator = (y / gcd) * other.y;
        return new Frac(numerator, denominator, true); // 必须simplify，因为显然会有 1/2 + 1/2 = 1 这种情况
    }

    public Frac sub(Frac other)
    {
        return add(other.neg());
    }

    public Frac add(BigInteger other)
    {
        return new Frac(x + other * y, y, false);
    }

    public Frac sub(BigInteger other)
    {
        return new Frac(x - other * y, y, false);
    }

    public Frac mul(BigInteger other)
    {
        return mul(new Frac(other));
    }

    public Frac div(BigInteger other)
    {
        return div(new Frac(other));
    }

    public bool isLegal()
    {
        return !y.IsZero;
    }

    public bool isInteger()
    {
        return y.IsOne;
    }

    public override string ToString()
    {
        if (y.IsZero)
        {
            if (x.IsZero) return "nan";
            if (x.IsOne) return "+inf";
            if (x == -1) return "-inf";
            throw new Exception("Frac.cs internal error, check simplify()");
        }

        if (y.IsOne)
        {
            return x.ToString();
        }

        return x.ToString() + "/" + y.ToString();
    }

    public static Frac operator +(Frac a, Frac b)
    {
        return a.add(b);
    }

    public static Frac operator +(Frac a, BigInteger b)
    {
        return a.add(b);
    }

    public static Frac operator +(BigInteger a, Frac b)
    {
        return b.add(a);
    }

    public static Frac operator -(Frac a, Frac b)
    {
        return a.sub(b);
    }

    public static Frac operator -(Frac a, BigInteger b)
    {
        return a.sub(b);
    }

    public static Frac operator -(BigInteger a, Frac b)
    {
        return b.sub(a).neg();
    }

    public static Frac operator *(Frac a, Frac b)
    {
        return a.mul(b);
    }

    public static Frac operator *(Frac a, BigInteger b)
    {
        return a.mul(b);
    }

    public static Frac operator *(BigInteger a, Frac b)
    {
        return b.mul(a);
    }
}
