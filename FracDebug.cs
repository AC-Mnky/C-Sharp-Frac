using System;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class FracDebug : MonoBehaviour
{
    public int x = 1;
    public int y = 1;
    public Frac inspectorValue1 = new Frac(1);
    public Frac inspectorValue2 = new Frac(1);
    public Frac inspectorValue3 = new Frac(1);
    public Frac inspectorValue4 = new Frac(1);

    void Start()
    {
        RunAll();
        Debug.Log("FracDebug.RunAll finished without exceptions");
    }

    public static void RunAll()
    {
        TestReciprocal();
        TestNeg();
        TestMulFrac();
        TestDivFrac();
        TestAddFrac();
        TestSubFrac();
        TestAddBigInteger();
        TestSubBigInteger();
        TestMulBigInteger();
        TestDivBigInteger();
        TestIsLegal();
        TestIsInteger();
        TestToStringMethod();
        TestFromStringAndIsValidString();
        TestToStringFromStringRoundTrip();
        TestOperators();
        TestCompoundAssignments();
        TestComparisonOperators();
        TestToDoubleAndFloat();
        TestToFloorAndToCeil();
        TestImplicitConversions();
    }

    public void Update()
    {
        y = x;
        inspectorValue3 = inspectorValue1 + inspectorValue2 + x;
        inspectorValue4 = inspectorValue4 + x;
    }

    private static void AssertEqual(Frac actual, Frac expected, string message)
    {
        if (!AreEqual(actual, expected))
        {
            throw new Exception("FracDebug Frac not equal: " + message + ", expected " + expected + ", actual " + actual);
        }
    }

    private static void AssertEqualString(string actual, string expected, string message)
    {
        if (!string.Equals(actual, expected, StringComparison.Ordinal))
        {
            throw new Exception("FracDebug string not equal: " + message + ", expected " + expected + ", actual " + actual);
        }
    }

    private static void AssertEqualBool(bool actual, bool expected, string message)
    {
        if (actual != expected)
        {
            throw new Exception("FracDebug bool not equal: " + message + ", expected " + expected + ", actual " + actual);
        }
    }

    private static void AssertEqualDouble(double actual, double expected, string message)
    {
        if (double.IsNaN(expected))
        {
            if (!double.IsNaN(actual))
            {
                throw new Exception("FracDebug double not NaN as expected: " + message + ", actual " + actual);
            }
            return;
        }

        if (double.IsPositiveInfinity(expected))
        {
            if (!double.IsPositiveInfinity(actual))
            {
                throw new Exception("FracDebug double not +inf as expected: " + message + ", actual " + actual);
            }
            return;
        }

        if (double.IsNegativeInfinity(expected))
        {
            if (!double.IsNegativeInfinity(actual))
            {
                throw new Exception("FracDebug double not -inf as expected: " + message + ", actual " + actual);
            }
            return;
        }

        if (actual != expected)
        {
            throw new Exception("FracDebug double not equal: " + message + ", expected " + expected + ", actual " + actual);
        }
    }

    private static void AssertEqualFloat(float actual, float expected, string message)
    {
        if (float.IsNaN(expected))
        {
            if (!float.IsNaN(actual))
            {
                throw new Exception("FracDebug float not NaN as expected: " + message + ", actual " + actual);
            }
            return;
        }

        if (float.IsPositiveInfinity(expected))
        {
            if (!float.IsPositiveInfinity(actual))
            {
                throw new Exception("FracDebug float not +inf as expected: " + message + ", actual " + actual);
            }
            return;
        }

        if (float.IsNegativeInfinity(expected))
        {
            if (!float.IsNegativeInfinity(actual))
            {
                throw new Exception("FracDebug float not -inf as expected: " + message + ", actual " + actual);
            }
            return;
        }

        if (actual != expected)
        {
            throw new Exception("FracDebug float not equal: " + message + ", expected " + expected + ", actual " + actual);
        }
    }

    private static bool AreEqual(Frac a, Frac b)
    {
        return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
    }

    private static void TestReciprocal()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.Reciprocal(), new Frac(2, 1), "reciprocal 1/2");

        var b = new Frac(-2, 3);
        AssertEqual(b.Reciprocal(), new Frac(-3, 2), "reciprocal -2/3");

        var c = new Frac(5);
        AssertEqual(c.Reciprocal(), new Frac(1, 5), "reciprocal 5");

        var zero = new Frac(0, 1);
        AssertEqual(zero.Reciprocal(), new Frac(1, 0), "reciprocal 0 -> +inf");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Reciprocal(), new Frac(0, 1), "reciprocal +inf -> 0");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Reciprocal(), new Frac(0, 1), "reciprocal -inf -> 0");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Reciprocal(), new Frac(0, 0), "reciprocal nan -> nan");

        var d = new Frac(3, 4);
        AssertEqual(d.Reciprocal().Reciprocal(), d, "double reciprocal 3/4");

        var e = new Frac(-5, 7);
        AssertEqual(e.Reciprocal().Reciprocal(), e, "double reciprocal -5/7");

        var f = new Frac(10, -2);
        AssertEqual(f.Reciprocal(), new Frac(-1, 5), "reciprocal 10/-2");
    }

    private static void TestNeg()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.Neg(), new Frac(-1, 2), "neg 1/2");

        var b = new Frac(-3, 4);
        AssertEqual(b.Neg(), new Frac(3, 4), "neg -3/4");

        var c = new Frac(0, 1);
        AssertEqual(c.Neg(), new Frac(0, 1), "neg 0");

        var d = new Frac(5);
        AssertEqual(d.Neg(), new Frac(-5, 1), "neg 5");

        var e = new Frac(1, 0);
        AssertEqual(e.Neg(), new Frac(-1, 0), "neg +inf -> -inf");

        var f = new Frac(-1, 0);
        AssertEqual(f.Neg(), new Frac(1, 0), "neg -inf -> +inf");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Neg(), new Frac(0, 0), "neg nan -> nan");

        var g = new Frac(2, 3);
        AssertEqual(g.Neg().Neg(), g, "double neg 2/3");

        var h = new Frac(-7, 5);
        AssertEqual(h.Neg().Neg(), h, "double neg -7/5");

        var i = new Frac(10, -2);
        AssertEqual(i.Neg(), new Frac(5, 1), "neg 10/-2");
    }

    private static void TestMulFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(2, 3);
        AssertEqual(a.Mul(b), new Frac(1, 3), "mul 1/2 * 2/3");

        var c = new Frac(-1, 2);
        var d = new Frac(2, 5);
        AssertEqual(c.Mul(d), new Frac(-1, 5), "mul -1/2 * 2/5");

        var e = new Frac(3, 4);
        var f = new Frac(4, 3);
        AssertEqual(e.Mul(f), new Frac(1, 1), "mul 3/4 * 4/3");

        var g = new Frac(0, 1);
        var h = new Frac(5, 7);
        AssertEqual(g.Mul(h), new Frac(0, 1), "mul 0 * 5/7");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Mul(h), new Frac(1, 0), "mul +inf * positive");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Mul(h), new Frac(-1, 0), "mul -inf * positive");

        var i = new Frac(-3, 5);
        AssertEqual(posInf.Mul(i), new Frac(-1, 0), "mul +inf * negative");

        AssertEqual(negInf.Mul(i), new Frac(1, 0), "mul -inf * negative");

        AssertEqual(g.Mul(posInf), new Frac(0, 0), "mul 0 * +inf -> nan");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Mul(h), new Frac(0, 0), "mul nan * 5/7 -> nan");
    }

    private static void TestDivFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(2, 3);
        AssertEqual(a.Div(b), new Frac(3, 4), "div 1/2 / 2/3");

        var c = new Frac(-1, 2);
        var d = new Frac(2, 5);
        AssertEqual(c.Div(d), new Frac(-5, 4), "div -1/2 / 2/5");

        var e = new Frac(3, 4);
        var f = new Frac(3, 4);
        AssertEqual(e.Div(f), new Frac(1, 1), "div 3/4 / 3/4");

        var g = new Frac(0, 1);
        var h = new Frac(5, 7);
        AssertEqual(g.Div(h), new Frac(0, 1), "div 0 / positive");

        AssertEqual(h.Div(g), new Frac(1, 0), "div positive / 0 -> +inf");

        var i = new Frac(-5, 7);
        AssertEqual(i.Div(g), new Frac(-1, 0), "div negative / 0 -> -inf");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Div(h), new Frac(1, 0), "div +inf / positive");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Div(h), new Frac(-1, 0), "div -inf / positive");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Div(h), new Frac(0, 0), "div nan / positive -> nan");

        AssertEqual(h.Div(posInf), new Frac(0, 1), "div positive / +inf -> 0");
    }

    private static void TestAddFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(1, 3);
        AssertEqual(a.Add(b), new Frac(5, 6), "add 1/2 + 1/3");

        var c = new Frac(-1, 2);
        var d = new Frac(1, 2);
        AssertEqual(c.Add(d), new Frac(0, 1), "add -1/2 + 1/2");

        var e = new Frac(1, 2);
        var f = new Frac(1, 2);
        AssertEqual(e.Add(f), new Frac(1, 1), "add 1/2 + 1/2");

        var g = new Frac(2, 3);
        var h = new Frac(4, 3);
        AssertEqual(g.Add(h), new Frac(2, 1), "add 2/3 + 4/3");

        var i = new Frac(0, 1);
        var j = new Frac(5, 7);
        AssertEqual(i.Add(j), j, "add 0 + 5/7");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Add(posInf), new Frac(1, 0), "add +inf + +inf");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Add(negInf), new Frac(-1, 0), "add -inf + -inf");

        AssertEqual(posInf.Add(negInf), new Frac(0, 0), "add +inf + -inf -> nan");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Add(j), new Frac(0, 0), "add nan + 5/7 -> nan");

        var k = new Frac(-2, 5);
        var l = new Frac(7, 10);
        AssertEqual(k.Add(l), new Frac(3, 10), "add -2/5 + 7/10");
    }

    private static void TestSubFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(1, 3);
        AssertEqual(a.Sub(b), new Frac(1, 6), "sub 1/2 - 1/3");

        var c = new Frac(1, 2);
        var d = new Frac(1, 2);
        AssertEqual(c.Sub(d), new Frac(0, 1), "sub 1/2 - 1/2");

        var e = new Frac(-1, 2);
        var f = new Frac(1, 2);
        AssertEqual(e.Sub(f), new Frac(-1, 1), "sub -1/2 - 1/2");

        var g = new Frac(2, 3);
        var h = new Frac(4, 3);
        AssertEqual(g.Sub(h), new Frac(-2, 3), "sub 2/3 - 4/3");

        var i = new Frac(0, 1);
        var j = new Frac(5, 7);
        AssertEqual(i.Sub(j), new Frac(-5, 7), "sub 0 - 5/7");

        var posInf = new Frac(1, 0);
        var negInf = new Frac(-1, 0);
        AssertEqual(posInf.Sub(negInf), new Frac(1, 0), "sub +inf - -inf -> +inf");

        AssertEqual(negInf.Sub(posInf), new Frac(-1, 0), "sub -inf - +inf -> -inf");

        AssertEqual(posInf.Sub(posInf), new Frac(0, 0), "sub +inf - +inf -> nan");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Sub(j), new Frac(0, 0), "sub nan - 5/7 -> nan");

        var k = new Frac(7, 10);
        var l = new Frac(2, 5);
        AssertEqual(k.Sub(l), new Frac(3, 10), "sub 7/10 - 2/5");
    }

    private static void TestAddBigInteger()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.Add(1), new Frac(3, 2), "add BigInteger 1/2 + 1");

        var b = new Frac(-3, 2);
        AssertEqual(b.Add(2), new Frac(1, 2), "add BigInteger -3/2 + 2");

        var c = new Frac(5, 3);
        AssertEqual(c.Add(-1), new Frac(2, 3), "add BigInteger 5/3 + (-1)");

        var d = new Frac(0, 1);
        AssertEqual(d.Add(5), new Frac(5, 1), "add BigInteger 0 + 5");

        var e = new Frac(7, 4);
        AssertEqual(e.Add(0), e, "add BigInteger 7/4 + 0");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Add(5), new Frac(1, 0), "add BigInteger +inf + 5");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Add(-3), new Frac(-1, 0), "add BigInteger -inf + (-3)");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Add(1), new Frac(0, 0), "add BigInteger nan + 1");

        var f = new Frac(-7, 3);
        AssertEqual(f.Add(3), new Frac(2, 3), "add BigInteger -7/3 + 3");

        var g = new Frac(10, 3);
        AssertEqual(g.Add(-3), new Frac(1, 3), "add BigInteger 10/3 + (-3)");
    }

    private static void TestSubBigInteger()
    {
        var a = new Frac(3, 2);
        AssertEqual(a.Sub(1), new Frac(1, 2), "sub BigInteger 3/2 - 1");

        var b = new Frac(-1, 2);
        AssertEqual(b.Sub(1), new Frac(-3, 2), "sub BigInteger -1/2 - 1");

        var c = new Frac(5, 3);
        AssertEqual(c.Sub(-1), new Frac(8, 3), "sub BigInteger 5/3 - (-1)");

        var d = new Frac(0, 1);
        AssertEqual(d.Sub(5), new Frac(-5, 1), "sub BigInteger 0 - 5");

        var e = new Frac(7, 4);
        AssertEqual(e.Sub(0), e, "sub BigInteger 7/4 - 0");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Sub(5), new Frac(1, 0), "sub BigInteger +inf - 5");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Sub(-3), new Frac(-1, 0), "sub BigInteger -inf - (-3)");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Sub(1), new Frac(0, 0), "sub BigInteger nan - 1");

        var f = new Frac(-7, 3);
        AssertEqual(f.Sub(3), new Frac(-16, 3), "sub BigInteger -7/3 - 3");

        var g = new Frac(10, 3);
        AssertEqual(g.Sub(-3), new Frac(19, 3), "sub BigInteger 10/3 - (-3)");
    }

    private static void TestMulBigInteger()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.Mul(2), new Frac(1, 1), "mul BigInteger 1/2 * 2");

        var b = new Frac(-3, 4);
        AssertEqual(b.Mul(2), new Frac(-3, 2), "mul BigInteger -3/4 * 2");

        var c = new Frac(5, 3);
        AssertEqual(c.Mul(-3), new Frac(-5, 1), "mul BigInteger 5/3 * -3");

        var d = new Frac(0, 1);
        AssertEqual(d.Mul(7), new Frac(0, 1), "mul BigInteger 0 * 7");

        var e = new Frac(7, 4);
        AssertEqual(e.Mul(0), new Frac(0, 1), "mul BigInteger 7/4 * 0");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Mul(2), new Frac(1, 0), "mul BigInteger +inf * 2");

        AssertEqual(posInf.Mul(-1), new Frac(-1, 0), "mul BigInteger +inf * -1");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Mul(2), new Frac(-1, 0), "mul BigInteger -inf * 2");

        AssertEqual(negInf.Mul(-1), new Frac(1, 0), "mul BigInteger -inf * -1");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Mul(5), new Frac(0, 0), "mul BigInteger nan * 5");
    }

    private static void TestDivBigInteger()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.Div(2), new Frac(1, 4), "div BigInteger 1/2 / 2");

        var b = new Frac(-3, 4);
        AssertEqual(b.Div(2), new Frac(-3, 8), "div BigInteger -3/4 / 2");

        var c = new Frac(5, 3);
        AssertEqual(c.Div(-5), new Frac(-1, 3), "div BigInteger 5/3 / -5");

        var d = new Frac(0, 1);
        AssertEqual(d.Div(7), new Frac(0, 1), "div BigInteger 0 / 7");

        var e = new Frac(7, 4);
        AssertEqual(e.Div(-1), new Frac(-7, 4), "div BigInteger 7/4 / -1");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.Div(2), new Frac(1, 0), "div BigInteger +inf / 2");

        AssertEqual(posInf.Div(-1), new Frac(-1, 0), "div BigInteger +inf / -1");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.Div(2), new Frac(-1, 0), "div BigInteger -inf / 2");

        AssertEqual(negInf.Div(-1), new Frac(1, 0), "div BigInteger -inf / -1");

        var nan = new Frac(0, 0);
        AssertEqual(nan.Div(5), new Frac(0, 0), "div BigInteger nan / 5");
    }

    private static void TestIsLegal()
    {
        var a = new Frac(1, 2);
        AssertEqualBool(a.IsLegal(), true, "isLegal 1/2");

        var b = new Frac(0, 1);
        AssertEqualBool(b.IsLegal(), true, "isLegal 0");

        var c = new Frac(-5, 3);
        AssertEqualBool(c.IsLegal(), true, "isLegal -5/3");

        var posInf = new Frac(1, 0);
        AssertEqualBool(posInf.IsLegal(), false, "isLegal +inf");

        var negInf = new Frac(-1, 0);
        AssertEqualBool(negInf.IsLegal(), false, "isLegal -inf");

        var nan = new Frac(0, 0);
        AssertEqualBool(nan.IsLegal(), false, "isLegal nan");

        var d = new Frac(2, 4);
        AssertEqualBool(d.IsLegal(), true, "isLegal 2/4");

        var e = new Frac(10, -2);
        AssertEqualBool(e.IsLegal(), true, "isLegal 10/-2");

        var f = new Frac(BigInteger.Zero, BigInteger.One);
        AssertEqualBool(f.IsLegal(), true, "isLegal BigInteger zero");

        var g = new Frac(BigInteger.One, BigInteger.Zero);
        AssertEqualBool(g.IsLegal(), false, "isLegal BigInteger +inf");
    }

    private static void TestIsInteger()
    {
        var a = new Frac(2);
        AssertEqualBool(a.IsInteger(), true, "isInteger 2");

        var b = new Frac(4, 2);
        AssertEqualBool(b.IsInteger(), true, "isInteger 4/2");

        var c = new Frac(3, 2);
        AssertEqualBool(c.IsInteger(), false, "isInteger 3/2");

        var d = new Frac(0, 1);
        AssertEqualBool(d.IsInteger(), true, "isInteger 0");

        var e = new Frac(-5, 1);
        AssertEqualBool(e.IsInteger(), true, "isInteger -5");

        var f = new Frac(-5, 3);
        AssertEqualBool(f.IsInteger(), false, "isInteger -5/3");

        var posInf = new Frac(1, 0);
        AssertEqualBool(posInf.IsInteger(), false, "isInteger +inf");

        var negInf = new Frac(-1, 0);
        AssertEqualBool(negInf.IsInteger(), false, "isInteger -inf");

        var nan = new Frac(0, 0);
        AssertEqualBool(nan.IsInteger(), false, "isInteger nan");

        var g = new Frac(6, 3);
        AssertEqualBool(g.IsInteger(), true, "isInteger 6/3");
    }

    private static void TestToStringMethod()
    {
        var a = new Frac(1, 2);
        AssertEqualString(a.ToString(), "1/2", "ToString 1/2");

        var b = new Frac(3, 1);
        AssertEqualString(b.ToString(), "3", "ToString 3");

        var c = new Frac(-5, 2);
        AssertEqualString(c.ToString(), "-5/2", "ToString -5/2");

        var d = new Frac(0, 1);
        AssertEqualString(d.ToString(), "0", "ToString 0");

        var posInf = new Frac(1, 0);
        AssertEqualString(posInf.ToString(), "+inf", "ToString +inf");

        var negInf = new Frac(-1, 0);
        AssertEqualString(negInf.ToString(), "-inf", "ToString -inf");

        var nan = new Frac(0, 0);
        AssertEqualString(nan.ToString(), "nan", "ToString nan");

        var e = new Frac(2, 4);
        AssertEqualString(e.ToString(), "1/2", "ToString 2/4 simplified");

        var f = new Frac(-4, 2);
        AssertEqualString(f.ToString(), "-2", "ToString -4/2");

        var g = new Frac(10, -2);
        AssertEqualString(g.ToString(), "-5", "ToString 10/-2");
    }

    private static void TestFromStringAndIsValidString()
    {
        AssertEqualBool(Frac.IsValidString("1/2"), true, "IsValidString 1/2");
        AssertEqualBool(Frac.IsValidString("  1 /  2  "), true, "IsValidString spaces in fraction");
        AssertEqualBool(Frac.IsValidString("3"), true, "IsValidString integer");
        AssertEqualBool(Frac.IsValidString("-5"), true, "IsValidString negative integer");
        AssertEqualBool(Frac.IsValidString("0.5"), true, "IsValidString decimal 0.5");
        AssertEqualBool(Frac.IsValidString("-2.25"), true, "IsValidString decimal -2.25");
        AssertEqualBool(Frac.IsValidString(" .5 "), true, "IsValidString decimal .5 with spaces");
        AssertEqualBool(Frac.IsValidString("nan"), true, "IsValidString nan");
        AssertEqualBool(Frac.IsValidString("+inf"), true, "IsValidString +inf");
        AssertEqualBool(Frac.IsValidString("-inf"), true, "IsValidString -inf");

        AssertEqualBool(Frac.IsValidString(""), false, "IsValidString empty string");
        AssertEqualBool(Frac.IsValidString("   "), false, "IsValidString only spaces");
        AssertEqualBool(Frac.IsValidString("abc"), false, "IsValidString letters");
        AssertEqualBool(Frac.IsValidString("1//2"), false, "IsValidString double slash");
        AssertEqualBool(Frac.IsValidString("/2"), false, "IsValidString missing numerator");
        AssertEqualBool(Frac.IsValidString("1/"), false, "IsValidString missing denominator");
        AssertEqualBool(Frac.IsValidString("1.2.3"), false, "IsValidString multiple dots");

        AssertEqual(Frac.FromString("1/2"), new Frac(1, 2), "FromString 1/2");
        AssertEqual(Frac.FromString("  1 /  2  "), new Frac(1, 2), "FromString spaces in fraction");
        AssertEqual(Frac.FromString("3"), new Frac(3, 1), "FromString integer 3");
        AssertEqual(Frac.FromString("-5"), new Frac(-5, 1), "FromString integer -5");
        AssertEqual(Frac.FromString("0.5"), new Frac(1, 2), "FromString decimal 0.5");
        AssertEqual(Frac.FromString("-2.25"), new Frac(-9, 4), "FromString decimal -2.25");
        AssertEqual(Frac.FromString(".5"), new Frac(1, 2), "FromString decimal .5");
        AssertEqual(Frac.FromString("nan"), new Frac(0, 0), "FromString nan");
        AssertEqual(Frac.FromString("+inf"), new Frac(1, 0), "FromString +inf");
        AssertEqual(Frac.FromString("-inf"), new Frac(-1, 0), "FromString -inf");

        bool threwOnInvalid = false;
        try
        {
            Frac.FromString("invalid");
        }
        catch
        {
            threwOnInvalid = true;
        }
        AssertEqualBool(threwOnInvalid, true, "FromString invalid string throws");
    }

    private static void TestToStringFromStringRoundTrip()
    {
        var values = new[]
        {
            new Frac(0, 1),
            new Frac(1, 2),
            new Frac(-5, 3),
            new Frac(1, 0),
            new Frac(-1, 0),
            new Frac(0, 0)
        };

        for (int i = 0; i < values.Length; i++)
        {
            var original = values[i];
            var s = original.ToString();
            var parsed = Frac.FromString(s);
            AssertEqual(parsed, original, "ToString/FromString roundtrip " + s);
        }
    }

    private static void TestOperators()
    {
        var a = new Frac(1, 2);
        var b = new Frac(1, 3);
        AssertEqual(a + b, new Frac(5, 6), "operator + frac+frac");

        var c = new Frac(1, 2);
        AssertEqual(c + (BigInteger)1, new Frac(3, 2), "operator + frac+bigint");

        AssertEqual((BigInteger)1 + c, new Frac(3, 2), "operator + bigint+frac");

        var d = new Frac(3, 2);
        var e = new Frac(1, 2);
        AssertEqual(d - e, new Frac(1, 1), "operator - frac-frac");

        var f = new Frac(3, 2);
        AssertEqual(f - (BigInteger)1, new Frac(1, 2), "operator - frac-bigint");

        AssertEqual((BigInteger)1 - e, new Frac(1, 2), "operator - bigint-frac");

        var g = new Frac(2, 3);
        var h = new Frac(3, 4);
        AssertEqual(g * h, new Frac(1, 2), "operator * frac*frac");

        var i = new Frac(1, 2);
        AssertEqual(i * (BigInteger)2, new Frac(1, 1), "operator * frac*bigint");

        AssertEqual((BigInteger)2 * i, new Frac(1, 1), "operator * bigint*frac");

        var posInf = new Frac(1, 0);
        var zero = new Frac(0, 1);
        AssertEqual(zero * posInf, new Frac(0, 0), "operator * 0*+inf -> nan");
    }

    private static void TestCompoundAssignments()
    {
        var a = new Frac(1, 2);
        var b = new Frac(1, 3);
        a += b;
        AssertEqual(a, new Frac(5, 6), "compound += frac+frac");

        var c = new Frac(1, 2);
        c += (BigInteger)1;
        AssertEqual(c, new Frac(3, 2), "compound += frac+bigint");

        var d = new Frac(3, 2);
        var e = new Frac(1, 2);
        d -= e;
        AssertEqual(d, new Frac(1, 1), "compound -= frac-frac");

        var f = new Frac(3, 2);
        f -= (BigInteger)1;
        AssertEqual(f, new Frac(1, 2), "compound -= frac-bigint");

        var g = new Frac(2, 3);
        var h = new Frac(3, 4);
        g *= h;
        AssertEqual(g, new Frac(1, 2), "compound *= frac*frac");

        var i = new Frac(1, 2);
        i *= (BigInteger)2;
        AssertEqual(i, new Frac(1, 1), "compound *= frac*bigint");
    }

    private static void TestComparisonOperators()
    {
        var a = new Frac(1, 2);
        var b = new Frac(2, 4);
        AssertEqualBool(a == b, true, "operator == equal rationals");
        AssertEqualBool(a != b, false, "operator != equal rationals");

        var c = new Frac(3, 4);
        AssertEqualBool(c > a, true, "operator > larger rational");
        AssertEqualBool(a < c, true, "operator < smaller rational");
        AssertEqualBool(a >= new Frac(1, 2), true, "operator >= equal");
        AssertEqualBool(a <= new Frac(1, 2), true, "operator <= equal");

        var d = new Frac(3, 2);
        AssertEqualBool(d > (BigInteger)1, true, "operator > frac>bigint");
        AssertEqualBool((BigInteger)2 > d, true, "operator > bigint>frac");
        AssertEqualBool((BigInteger)1 < d, true, "operator < bigint<frac");
        AssertEqualBool(d == (BigInteger)3, false, "operator == frac!=bigint");
        AssertEqualBool(d == (BigInteger)1, false, "operator == frac!=bigint smaller");

        var posInf = new Frac(1, 0);
        var negInf = new Frac(-1, 0);
        var posInf2 = new Frac(1, 0);
        var negInf2 = new Frac(-1, 0);
        var zero = new Frac(0, 1);
        AssertEqualBool(posInf > zero, true, "operator > +inf>0");
        AssertEqualBool(negInf < zero, true, "operator < -inf<0");
        AssertEqualBool(posInf > negInf, true, "operator > +inf>-inf");
        AssertEqualBool(posInf == posInf2, true, "operator == +inf==+inf");
        AssertEqualBool(negInf == negInf2, true, "operator == -inf==-inf");
        AssertEqualBool(posInf != negInf, true, "operator != +inf!=-inf");

        var nan1 = new Frac(0, 0);
        var nan2 = new Frac(0, 0);
        AssertEqualBool(nan1 == nan2, false, "operator == nan==nan is false");
        AssertEqualBool(nan1 != nan2, true, "operator != nan!=nan is true");
        AssertEqualBool(nan1 > zero, false, "operator > nan>0 is false");
        AssertEqualBool(nan1 < zero, false, "operator < nan<0 is false");
        AssertEqualBool(nan1 >= zero, false, "operator >= nan>=0 is false");
        AssertEqualBool(nan1 <= zero, false, "operator <= nan<=0 is false");
    }

    private static void TestToDoubleAndFloat()
    {
        var a = new Frac(1, 2);
        AssertEqualDouble(a.ToDouble(), 0.5, "ToDouble 1/2");
        AssertEqualFloat(a.ToFloat(), 0.5f, "ToFloat 1/2");

        var b = new Frac(3, 1);
        AssertEqualDouble(b.ToDouble(), 3.0, "ToDouble 3");
        AssertEqualFloat(b.ToFloat(), 3.0f, "ToFloat 3");

        var c = new Frac(-5, 2);
        AssertEqualDouble(c.ToDouble(), -2.5, "ToDouble -5/2");
        AssertEqualFloat(c.ToFloat(), -2.5f, "ToFloat -5/2");

        var zero = new Frac(0, 1);
        AssertEqualDouble(zero.ToDouble(), 0.0, "ToDouble 0");
        AssertEqualFloat(zero.ToFloat(), 0.0f, "ToFloat 0");

        var posInf = new Frac(1, 0);
        AssertEqualDouble(posInf.ToDouble(), double.PositiveInfinity, "ToDouble +inf");
        AssertEqualFloat(posInf.ToFloat(), float.PositiveInfinity, "ToFloat +inf");

        var negInf = new Frac(-1, 0);
        AssertEqualDouble(negInf.ToDouble(), double.NegativeInfinity, "ToDouble -inf");
        AssertEqualFloat(negInf.ToFloat(), float.NegativeInfinity, "ToFloat -inf");

        var nan = new Frac(0, 0);
        AssertEqualDouble(nan.ToDouble(), double.NaN, "ToDouble nan");
        AssertEqualFloat(nan.ToFloat(), float.NaN, "ToFloat nan");
    }

    private static void AssertEqualBigInteger(BigInteger actual, BigInteger expected, string message)
    {
        if (actual != expected)
        {
            throw new Exception("FracDebug BigInteger not equal: " + message + ", expected " + expected + ", actual " + actual);
        }
    }

    private static void TestToFloorAndToCeil()
    {
        // ToFloor
        AssertEqualBigInteger(new Frac(5).ToFloor(), 5, "ToFloor integer 5");
        AssertEqualBigInteger(new Frac(5, 3).ToFloor(), 1, "ToFloor 5/3 -> 1");
        AssertEqualBigInteger(new Frac(-5, 3).ToFloor(), -2, "ToFloor -5/3 -> -2");
        AssertEqualBigInteger(new Frac(1, 2).ToFloor(), 0, "ToFloor 1/2 -> 0");
        AssertEqualBigInteger(new Frac(-1, 2).ToFloor(), -1, "ToFloor -1/2 -> -1");
        AssertEqualBigInteger(new Frac(0, 1).ToFloor(), 0, "ToFloor 0 -> 0");
        AssertEqualBigInteger(new Frac(4, 2).ToFloor(), 2, "ToFloor 4/2 -> 2");
        AssertEqualBigInteger(new Frac(-4, 2).ToFloor(), -2, "ToFloor -4/2 -> -2");
        AssertEqualBigInteger(new Frac(7, 3).ToFloor(), 2, "ToFloor 7/3 -> 2");
        AssertEqualBigInteger(new Frac(-7, 3).ToFloor(), -3, "ToFloor -7/3 -> -3");

        // ToCeil
        AssertEqualBigInteger(new Frac(5).ToCeil(), 5, "ToCeil integer 5");
        AssertEqualBigInteger(new Frac(5, 3).ToCeil(), 2, "ToCeil 5/3 -> 2");
        AssertEqualBigInteger(new Frac(-5, 3).ToCeil(), -1, "ToCeil -5/3 -> -1");
        AssertEqualBigInteger(new Frac(1, 2).ToCeil(), 1, "ToCeil 1/2 -> 1");
        AssertEqualBigInteger(new Frac(-1, 2).ToCeil(), 0, "ToCeil -1/2 -> 0");
        AssertEqualBigInteger(new Frac(0, 1).ToCeil(), 0, "ToCeil 0 -> 0");
        AssertEqualBigInteger(new Frac(4, 2).ToCeil(), 2, "ToCeil 4/2 -> 2");
        AssertEqualBigInteger(new Frac(-4, 2).ToCeil(), -2, "ToCeil -4/2 -> -2");
        AssertEqualBigInteger(new Frac(7, 3).ToCeil(), 3, "ToCeil 7/3 -> 3");
        AssertEqualBigInteger(new Frac(-7, 3).ToCeil(), -2, "ToCeil -7/3 -> -2");

        // ToFloor/ToCeil on non-finite values should throw
        var posInf = new Frac(1, 0);
        var negInf = new Frac(-1, 0);
        var nan = new Frac(0, 0);

        var threwFloorPosInf = false;
        try { posInf.ToFloor(); } catch { threwFloorPosInf = true; }
        AssertEqualBool(threwFloorPosInf, true, "ToFloor +inf throws");

        var threwFloorNegInf = false;
        try { negInf.ToFloor(); } catch { threwFloorNegInf = true; }
        AssertEqualBool(threwFloorNegInf, true, "ToFloor -inf throws");

        var threwFloorNan = false;
        try { nan.ToFloor(); } catch { threwFloorNan = true; }
        AssertEqualBool(threwFloorNan, true, "ToFloor nan throws");

        var threwCeilPosInf = false;
        try { posInf.ToCeil(); } catch { threwCeilPosInf = true; }
        AssertEqualBool(threwCeilPosInf, true, "ToCeil +inf throws");

        var threwCeilNegInf = false;
        try { negInf.ToCeil(); } catch { threwCeilNegInf = true; }
        AssertEqualBool(threwCeilNegInf, true, "ToCeil -inf throws");

        var threwCeilNan = false;
        try { nan.ToCeil(); } catch { threwCeilNan = true; }
        AssertEqualBool(threwCeilNan, true, "ToCeil nan throws");
    }

    private static void TestImplicitConversions()
    {
        Frac fromInt = 42;
        AssertEqual(fromInt, new Frac(42), "implicit int -> Frac");

        Frac fromNegInt = -7;
        AssertEqual(fromNegInt, new Frac(-7), "implicit negative int -> Frac");

        Frac fromZeroInt = 0;
        AssertEqual(fromZeroInt, new Frac(0), "implicit int 0 -> Frac");

        Frac fromBigInt = (BigInteger)123456789;
        AssertEqual(fromBigInt, new Frac(123456789), "implicit BigInteger -> Frac");

        Frac a = new Frac(1, 2);
        AssertEqual(a + 1, new Frac(3, 2), "Frac + implicit int");
        AssertEqual(1 + a, new Frac(3, 2), "implicit int + Frac");
        AssertEqual(a - 1, new Frac(-1, 2), "Frac - implicit int");
        AssertEqual(1 - a, new Frac(1, 2), "implicit int - Frac");
        AssertEqual(a * 3, new Frac(3, 2), "Frac * implicit int");
        AssertEqual(3 * a, new Frac(3, 2), "implicit int * Frac");

        AssertEqualBool(new Frac(5) == 5, true, "Frac == implicit int");
        AssertEqualBool(new Frac(3, 2) > 1, true, "Frac > implicit int");
        AssertEqualBool(0 < new Frac(1, 2), true, "implicit int < Frac");

        Frac fromStr1 = "1/2";
        AssertEqual(fromStr1, new Frac(1, 2), "implicit string \"1/2\" -> Frac");

        Frac fromStr2 = "-5/3";
        AssertEqual(fromStr2, new Frac(-5, 3), "implicit string \"-5/3\" -> Frac");

        Frac fromStr3 = "42";
        AssertEqual(fromStr3, new Frac(42), "implicit string \"42\" -> Frac");

        Frac fromStr4 = "0.5";
        AssertEqual(fromStr4, new Frac(1, 2), "implicit string \"0.5\" -> Frac");

        Frac fromStr5 = "-2.25";
        AssertEqual(fromStr5, new Frac(-9, 4), "implicit string \"-2.25\" -> Frac");

        Frac fromStr6 = "+inf";
        AssertEqual(fromStr6, new Frac(1, 0), "implicit string \"+inf\" -> Frac");

        Frac fromStr7 = "-inf";
        AssertEqual(fromStr7, new Frac(-1, 0), "implicit string \"-inf\" -> Frac");

        Frac fromStr8 = "nan";
        AssertEqualString(fromStr8.ToString(), "nan", "implicit string \"nan\" -> Frac");

        bool threwOnInvalidStr = false;
        try
        {
            Frac invalid = "abc";
        }
        catch
        {
            threwOnInvalidStr = true;
        }
        AssertEqualBool(threwOnInvalidStr, true, "implicit string invalid throws");

        string toStr1 = new Frac(1, 2);
        AssertEqualString(toStr1, "1/2", "implicit Frac -> string 1/2");

        string toStr2 = new Frac(3);
        AssertEqualString(toStr2, "3", "implicit Frac -> string 3");

        string toStr3 = new Frac(-5, 3);
        AssertEqualString(toStr3, "-5/3", "implicit Frac -> string -5/3");

        string toStr4 = new Frac(1, 0);
        AssertEqualString(toStr4, "+inf", "implicit Frac -> string +inf");

        string toStr5 = new Frac(-1, 0);
        AssertEqualString(toStr5, "-inf", "implicit Frac -> string -inf");

        string toStr6 = new Frac(0, 0);
        AssertEqualString(toStr6, "nan", "implicit Frac -> string nan");
    }
}

