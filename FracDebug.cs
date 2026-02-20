using System;
using System.Numerics;
using UnityEngine;

public class FracDebug : MonoBehaviour
{
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
        TestOperators();
        TestCompoundAssignments();
        TestComparisonOperators();
        TestToDoubleAndFloat();
        TestToBigIntegerConversion();
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

    private static void TestToBigIntegerConversion()
    {
        var a = new Frac(5);
        if (a.ToBigInteger() != new BigInteger(5))
        {
            throw new Exception("ToBigInteger integer 5");
        }

        var b = new Frac(5, 3);
        if (b.ToBigInteger() != new BigInteger(1))
        {
            throw new Exception("ToBigInteger 5/3 -> 1");
        }

        var c = new Frac(-5, 3);
        if (c.ToBigInteger() != new BigInteger(-2))
        {
            throw new Exception("ToBigInteger -5/3 -> -2");
        }

        var d = new Frac(1, 2);
        if (d.ToBigInteger() != new BigInteger(0))
        {
            throw new Exception("ToBigInteger 1/2 -> 0");
        }

        var e = new Frac(-1, 2);
        if (e.ToBigInteger() != new BigInteger(-1))
        {
            throw new Exception("ToBigInteger -1/2 -> -1");
        }

        var zero = new Frac(0, 1);
        if (zero.ToBigInteger() != BigInteger.Zero)
        {
            throw new Exception("ToBigInteger 0 -> 0");
        }

        var posInf = new Frac(1, 0);
        var negInf = new Frac(-1, 0);
        var nan = new Frac(0, 0);

        var threwPosInf = false;
        try
        {
            posInf.ToBigInteger();
        }
        catch
        {
            threwPosInf = true;
        }
        AssertEqualBool(threwPosInf, true, "ToBigInteger +inf throws");

        var threwNegInf = false;
        try
        {
            negInf.ToBigInteger();
        }
        catch
        {
            threwNegInf = true;
        }
        AssertEqualBool(threwNegInf, true, "ToBigInteger -inf throws");

        var threwNan = false;
        try
        {
            nan.ToBigInteger();
        }
        catch
        {
            threwNan = true;
        }
        AssertEqualBool(threwNan, true, "ToBigInteger nan throws");
    }
}

