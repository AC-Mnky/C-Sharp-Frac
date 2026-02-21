using System;
using System.Globalization;
using System.Numerics;
using System.Text;

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

public struct Frac
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

    private Frac(BigInteger x, BigInteger y, bool shouldSimplify) // 如果 shouldSimplify 为 false，则只保证分母非负，不保证与分子互素
    {
        this.x = x;
        this.y = y;
        if (shouldSimplify)
        {
            simplify();
        }
        else if (y.Sign < 0)
        {
            this.x = -x;
            this.y = -y;
        }
    }

    public Frac Reciprocal()
    {
        return new Frac(y, x, false);
    }

    public Frac Neg()
    {
        return new Frac(-x, y, false);
    }

    public Frac Mul(Frac other)
    {
        var gcd1 = BigIntegerExtensions.GCD(x, other.y);
        var gcd2 = BigIntegerExtensions.GCD(other.x, y);
        if (gcd1.IsZero || gcd2.IsZero)
        {
            return new Frac(0, 0, false);
        }
        return new Frac((x / gcd1) * (other.x / gcd2), (y / gcd2) * (other.y / gcd1), false);
    }

    public Frac Div(Frac other)
    {
        return Mul(other.Reciprocal());
    }

    public Frac Add(Frac other)
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

    public Frac Sub(Frac other)
    {
        return Add(other.Neg());
    }

    public Frac Add(BigInteger other)
    {
        return new Frac(x + other * y, y, false);
    }

    public Frac Sub(BigInteger other)
    {
        return new Frac(x - other * y, y, false);
    }

    public Frac Mul(BigInteger other)
    {
        return Mul(new Frac(other));
    }

    public Frac Div(BigInteger other)
    {
        return Div(new Frac(other));
    }

    public bool IsLegal()
    {
        return !y.IsZero;
    }

    public bool IsInteger()
    {
        return y.IsOne;
    }

    public bool IsNaN()
    {
        return y.IsZero && x.IsZero;
    }

    public double ToDouble()
    {
        if (y.IsZero)
        {
            if (x.IsZero) return double.NaN;
            if (x.Sign > 0) return double.PositiveInfinity;
            return double.NegativeInfinity;
        }

        return (double)x / (double)y;
    }

    public float ToFloat()
    {
        return (float)ToDouble();
    }

    public BigInteger ToBigInteger()
    {
        if (!IsLegal())
        {
            throw new Exception("Frac.ToBigInteger called on non-finite value");
        }

        if (IsInteger())
        {
            return x;
        }

        var quotient = BigInteger.DivRem(x, y, out var remainder);
        if (remainder.IsZero)
        {
            return quotient;
        }

        if (x.Sign < 0)
        {
            return quotient - BigInteger.One;
        }

        return quotient;
    }

    private static bool equalsCore(Frac a, Frac b)
    {
        bool aNaN = a.y.IsZero && a.x.IsZero;
        bool bNaN = b.y.IsZero && b.x.IsZero;
        if (aNaN || bNaN) return false;
        return a.x == b.x && a.y == b.y;
    }

    private static int compareNonNaN(Frac a, Frac b)
    {
        bool aInf = a.y.IsZero;
        bool bInf = b.y.IsZero;
        if (aInf || bInf)
        {
            if (aInf && bInf)
            {
                if (a.x == b.x) return 0;
                return a.x > b.x ? 1 : -1;
            }
            if (aInf) return a.x > 0 ? 1 : -1;
            return b.x > 0 ? -1 : 1;
        }
        var left = a.x * b.y;
        var right = b.x * a.y;
        return left.CompareTo(right);
    }

    public override bool Equals(object obj)
    {
        if (obj is Frac other) return equalsCore(this, other);
        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + x.GetHashCode();
            hash = hash * 31 + y.GetHashCode();
            return hash;
        }
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

    private static string normalizeInputString(string s)
    {
        if (s == null) return null;
        var builder = new StringBuilder(s.Length);
        for (int i = 0; i < s.Length; i++)
        {
            var ch = s[i];
            if (!char.IsWhiteSpace(ch))
            {
                builder.Append(ch);
            }
        }
        return builder.ToString();
    }

    private static bool allDigits(string s)
    {
        if (string.IsNullOrEmpty(s)) return false;
        for (int i = 0; i < s.Length; i++)
        {
            var ch = s[i];
            if (ch < '0' || ch > '9') return false;
        }
        return true;
    }

    private static bool tryParseCore(string s, out Frac result)
    {
        result = default;
        if (s == null) return false;

        var normalized = normalizeInputString(s);
        if (string.IsNullOrEmpty(normalized)) return false;

        if (normalized == "nan")
        {
            result = new Frac(0, 0, false);
            return true;
        }

        if (normalized == "+inf")
        {
            result = new Frac(1, 0, false);
            return true;
        }

        if (normalized == "-inf")
        {
            result = new Frac(-1, 0, false);
            return true;
        }

        int slashIndex = normalized.IndexOf('/');
        if (slashIndex >= 0)
        {
            if (normalized.IndexOf('/', slashIndex + 1) >= 0) return false;
            var numeratorString = normalized.Substring(0, slashIndex);
            var denominatorString = normalized.Substring(slashIndex + 1);
            if (numeratorString.Length == 0 || denominatorString.Length == 0) return false;
            try
            {
                var numerator = BigInteger.Parse(numeratorString, CultureInfo.InvariantCulture);
                var denominator = BigInteger.Parse(denominatorString, CultureInfo.InvariantCulture);
                result = new Frac(numerator, denominator);
                return true;
            }
            catch
            {
                return false;
            }
        }

        int dotIndex = normalized.IndexOf('.');
        if (dotIndex >= 0)
        {
            if (normalized.IndexOf('.', dotIndex + 1) >= 0) return false;

            bool isNegative = false;
            string number = normalized;
            if (number[0] == '+' || number[0] == '-')
            {
                isNegative = number[0] == '-';
                number = number.Substring(1);
            }

            dotIndex = number.IndexOf('.');
            if (dotIndex < 0) return false;

            var integerPartString = number.Substring(0, dotIndex);
            var fractionalPartString = number.Substring(dotIndex + 1);

            if (integerPartString.Length == 0 && fractionalPartString.Length == 0) return false;
            if (integerPartString.Length == 0) integerPartString = "0";
            if (fractionalPartString.Length == 0) fractionalPartString = "0";

            if (!allDigits(integerPartString) || !allDigits(fractionalPartString)) return false;

            try
            {
                var integerPart = BigInteger.Parse(integerPartString, CultureInfo.InvariantCulture);
                var fractionalPart = BigInteger.Parse(fractionalPartString, CultureInfo.InvariantCulture);
                var denominator = BigInteger.Pow(new BigInteger(10), fractionalPartString.Length);
                var numerator = integerPart * denominator + fractionalPart;
                if (isNegative) numerator = -numerator;
                result = new Frac(numerator, denominator);
                return true;
            }
            catch
            {
                return false;
            }
        }

        try
        {
            var integer = BigInteger.Parse(normalized, CultureInfo.InvariantCulture);
            result = new Frac(integer);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidString(string s)
    {
        return tryParseCore(s, out _);
    }

    public static Frac FromString(string s)
    {
        if (tryParseCore(s, out var result))
        {
            return result;
        }
        throw new Exception("Frac.FromString invalid string");
    }

    public static bool operator ==(Frac a, Frac b)
    {
        return equalsCore(a, b);
    }

    public static bool operator !=(Frac a, Frac b)
    {
        return !equalsCore(a, b);
    }

    public static bool operator ==(Frac a, BigInteger b)
    {
        return equalsCore(a, new Frac(b));
    }

    public static bool operator ==(BigInteger a, Frac b)
    {
        return equalsCore(new Frac(a), b);
    }

    public static bool operator !=(Frac a, BigInteger b)
    {
        return !(a == b);
    }

    public static bool operator !=(BigInteger a, Frac b)
    {
        return !(a == b);
    }

    public static bool operator >(Frac a, Frac b)
    {
        bool aNaN = a.y.IsZero && a.x.IsZero;
        bool bNaN = b.y.IsZero && b.x.IsZero;
        if (aNaN || bNaN) return false;
        return compareNonNaN(a, b) > 0;
    }

    public static bool operator <(Frac a, Frac b)
    {
        bool aNaN = a.y.IsZero && a.x.IsZero;
        bool bNaN = b.y.IsZero && b.x.IsZero;
        if (aNaN || bNaN) return false;
        return compareNonNaN(a, b) < 0;
    }

    public static bool operator >=(Frac a, Frac b)
    {
        bool aNaN = a.y.IsZero && a.x.IsZero;
        bool bNaN = b.y.IsZero && b.x.IsZero;
        if (aNaN || bNaN) return false;
        if (equalsCore(a, b)) return true;
        return compareNonNaN(a, b) > 0;
    }

    public static bool operator <=(Frac a, Frac b)
    {
        bool aNaN = a.y.IsZero && a.x.IsZero;
        bool bNaN = b.y.IsZero && b.x.IsZero;
        if (aNaN || bNaN) return false;
        if (equalsCore(a, b)) return true;
        return compareNonNaN(a, b) < 0;
    }

    public static bool operator >(Frac a, BigInteger b)
    {
        return a > new Frac(b);
    }

    public static bool operator >(BigInteger a, Frac b)
    {
        return new Frac(a) > b;
    }

    public static bool operator <(Frac a, BigInteger b)
    {
        return a < new Frac(b);
    }

    public static bool operator <(BigInteger a, Frac b)
    {
        return new Frac(a) < b;
    }

    public static bool operator >=(Frac a, BigInteger b)
    {
        return a >= new Frac(b);
    }

    public static bool operator >=(BigInteger a, Frac b)
    {
        return new Frac(a) >= b;
    }

    public static bool operator <=(Frac a, BigInteger b)
    {
        return a <= new Frac(b);
    }

    public static bool operator <=(BigInteger a, Frac b)
    {
        return new Frac(a) <= b;
    }

    public static Frac operator +(Frac a, Frac b)
    {
        return a.Add(b);
    }

    public static Frac operator +(Frac a, BigInteger b)
    {
        return a.Add(b);
    }

    public static Frac operator +(BigInteger a, Frac b)
    {
        return b.Add(a);
    }

    public static Frac operator -(Frac a, Frac b)
    {
        return a.Sub(b);
    }

    public static Frac operator -(Frac a, BigInteger b)
    {
        return a.Sub(b);
    }

    public static Frac operator -(BigInteger a, Frac b)
    {
        return b.Sub(a).Neg();
    }

    public static Frac operator *(Frac a, Frac b)
    {
        return a.Mul(b);
    }

    public static Frac operator *(Frac a, BigInteger b)
    {
        return a.Mul(b);
    }

    public static Frac operator *(BigInteger a, Frac b)
    {
        return b.Mul(a);
    }
}
