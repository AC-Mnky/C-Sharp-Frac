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

    private static bool AreEqual(Frac a, Frac b)
    {
        return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
    }

    private static void TestReciprocal()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.reciprocal(), new Frac(2, 1), "reciprocal 1/2");

        var b = new Frac(-2, 3);
        AssertEqual(b.reciprocal(), new Frac(-3, 2), "reciprocal -2/3");

        var c = new Frac(5);
        AssertEqual(c.reciprocal(), new Frac(1, 5), "reciprocal 5");

        var zero = new Frac(0, 1);
        AssertEqual(zero.reciprocal(), new Frac(1, 0), "reciprocal 0 -> +inf");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.reciprocal(), new Frac(0, 1), "reciprocal +inf -> 0");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.reciprocal(), new Frac(0, 1), "reciprocal -inf -> 0");

        var nan = new Frac(0, 0);
        AssertEqual(nan.reciprocal(), new Frac(0, 0), "reciprocal nan -> nan");

        var d = new Frac(3, 4);
        AssertEqual(d.reciprocal().reciprocal(), d, "double reciprocal 3/4");

        var e = new Frac(-5, 7);
        AssertEqual(e.reciprocal().reciprocal(), e, "double reciprocal -5/7");

        var f = new Frac(10, -2);
        AssertEqual(f.reciprocal(), new Frac(-1, 5), "reciprocal 10/-2");
    }

    private static void TestNeg()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.neg(), new Frac(-1, 2), "neg 1/2");

        var b = new Frac(-3, 4);
        AssertEqual(b.neg(), new Frac(3, 4), "neg -3/4");

        var c = new Frac(0, 1);
        AssertEqual(c.neg(), new Frac(0, 1), "neg 0");

        var d = new Frac(5);
        AssertEqual(d.neg(), new Frac(-5, 1), "neg 5");

        var e = new Frac(1, 0);
        AssertEqual(e.neg(), new Frac(-1, 0), "neg +inf -> -inf");

        var f = new Frac(-1, 0);
        AssertEqual(f.neg(), new Frac(1, 0), "neg -inf -> +inf");

        var nan = new Frac(0, 0);
        AssertEqual(nan.neg(), new Frac(0, 0), "neg nan -> nan");

        var g = new Frac(2, 3);
        AssertEqual(g.neg().neg(), g, "double neg 2/3");

        var h = new Frac(-7, 5);
        AssertEqual(h.neg().neg(), h, "double neg -7/5");

        var i = new Frac(10, -2);
        AssertEqual(i.neg(), new Frac(5, 1), "neg 10/-2");
    }

    private static void TestMulFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(2, 3);
        AssertEqual(a.mul(b), new Frac(1, 3), "mul 1/2 * 2/3");

        var c = new Frac(-1, 2);
        var d = new Frac(2, 5);
        AssertEqual(c.mul(d), new Frac(-1, 5), "mul -1/2 * 2/5");

        var e = new Frac(3, 4);
        var f = new Frac(4, 3);
        AssertEqual(e.mul(f), new Frac(1, 1), "mul 3/4 * 4/3");

        var g = new Frac(0, 1);
        var h = new Frac(5, 7);
        AssertEqual(g.mul(h), new Frac(0, 1), "mul 0 * 5/7");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.mul(h), new Frac(1, 0), "mul +inf * positive");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.mul(h), new Frac(-1, 0), "mul -inf * positive");

        var i = new Frac(-3, 5);
        AssertEqual(posInf.mul(i), new Frac(-1, 0), "mul +inf * negative");

        AssertEqual(negInf.mul(i), new Frac(1, 0), "mul -inf * negative");

        AssertEqual(g.mul(posInf), new Frac(0, 0), "mul 0 * +inf -> nan");

        var nan = new Frac(0, 0);
        AssertEqual(nan.mul(h), new Frac(0, 0), "mul nan * 5/7 -> nan");
    }

    private static void TestDivFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(2, 3);
        AssertEqual(a.div(b), new Frac(3, 4), "div 1/2 / 2/3");

        var c = new Frac(-1, 2);
        var d = new Frac(2, 5);
        AssertEqual(c.div(d), new Frac(-5, 4), "div -1/2 / 2/5");

        var e = new Frac(3, 4);
        var f = new Frac(3, 4);
        AssertEqual(e.div(f), new Frac(1, 1), "div 3/4 / 3/4");

        var g = new Frac(0, 1);
        var h = new Frac(5, 7);
        AssertEqual(g.div(h), new Frac(0, 1), "div 0 / positive");

        AssertEqual(h.div(g), new Frac(1, 0), "div positive / 0 -> +inf");

        var i = new Frac(-5, 7);
        AssertEqual(i.div(g), new Frac(-1, 0), "div negative / 0 -> -inf");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.div(h), new Frac(1, 0), "div +inf / positive");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.div(h), new Frac(-1, 0), "div -inf / positive");

        var nan = new Frac(0, 0);
        AssertEqual(nan.div(h), new Frac(0, 0), "div nan / positive -> nan");

        AssertEqual(h.div(posInf), new Frac(0, 1), "div positive / +inf -> 0");
    }

    private static void TestAddFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(1, 3);
        AssertEqual(a.add(b), new Frac(5, 6), "add 1/2 + 1/3");

        var c = new Frac(-1, 2);
        var d = new Frac(1, 2);
        AssertEqual(c.add(d), new Frac(0, 1), "add -1/2 + 1/2");

        var e = new Frac(1, 2);
        var f = new Frac(1, 2);
        AssertEqual(e.add(f), new Frac(1, 1), "add 1/2 + 1/2");

        var g = new Frac(2, 3);
        var h = new Frac(4, 3);
        AssertEqual(g.add(h), new Frac(2, 1), "add 2/3 + 4/3");

        var i = new Frac(0, 1);
        var j = new Frac(5, 7);
        AssertEqual(i.add(j), j, "add 0 + 5/7");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.add(posInf), new Frac(1, 0), "add +inf + +inf");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.add(negInf), new Frac(-1, 0), "add -inf + -inf");

        AssertEqual(posInf.add(negInf), new Frac(0, 0), "add +inf + -inf -> nan");

        var nan = new Frac(0, 0);
        AssertEqual(nan.add(j), new Frac(0, 0), "add nan + 5/7 -> nan");

        var k = new Frac(-2, 5);
        var l = new Frac(7, 10);
        AssertEqual(k.add(l), new Frac(3, 10), "add -2/5 + 7/10");
    }

    private static void TestSubFrac()
    {
        var a = new Frac(1, 2);
        var b = new Frac(1, 3);
        AssertEqual(a.sub(b), new Frac(1, 6), "sub 1/2 - 1/3");

        var c = new Frac(1, 2);
        var d = new Frac(1, 2);
        AssertEqual(c.sub(d), new Frac(0, 1), "sub 1/2 - 1/2");

        var e = new Frac(-1, 2);
        var f = new Frac(1, 2);
        AssertEqual(e.sub(f), new Frac(-1, 1), "sub -1/2 - 1/2");

        var g = new Frac(2, 3);
        var h = new Frac(4, 3);
        AssertEqual(g.sub(h), new Frac(-2, 3), "sub 2/3 - 4/3");

        var i = new Frac(0, 1);
        var j = new Frac(5, 7);
        AssertEqual(i.sub(j), new Frac(-5, 7), "sub 0 - 5/7");

        var posInf = new Frac(1, 0);
        var negInf = new Frac(-1, 0);
        AssertEqual(posInf.sub(negInf), new Frac(1, 0), "sub +inf - -inf -> +inf");

        AssertEqual(negInf.sub(posInf), new Frac(-1, 0), "sub -inf - +inf -> -inf");

        AssertEqual(posInf.sub(posInf), new Frac(0, 0), "sub +inf - +inf -> nan");

        var nan = new Frac(0, 0);
        AssertEqual(nan.sub(j), new Frac(0, 0), "sub nan - 5/7 -> nan");

        var k = new Frac(7, 10);
        var l = new Frac(2, 5);
        AssertEqual(k.sub(l), new Frac(3, 10), "sub 7/10 - 2/5");
    }

    private static void TestAddBigInteger()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.add(1), new Frac(3, 2), "add BigInteger 1/2 + 1");

        var b = new Frac(-3, 2);
        AssertEqual(b.add(2), new Frac(1, 2), "add BigInteger -3/2 + 2");

        var c = new Frac(5, 3);
        AssertEqual(c.add(-1), new Frac(2, 3), "add BigInteger 5/3 + (-1)");

        var d = new Frac(0, 1);
        AssertEqual(d.add(5), new Frac(5, 1), "add BigInteger 0 + 5");

        var e = new Frac(7, 4);
        AssertEqual(e.add(0), e, "add BigInteger 7/4 + 0");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.add(5), new Frac(1, 0), "add BigInteger +inf + 5");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.add(-3), new Frac(-1, 0), "add BigInteger -inf + (-3)");

        var nan = new Frac(0, 0);
        AssertEqual(nan.add(1), new Frac(0, 0), "add BigInteger nan + 1");

        var f = new Frac(-7, 3);
        AssertEqual(f.add(3), new Frac(2, 3), "add BigInteger -7/3 + 3");

        var g = new Frac(10, 3);
        AssertEqual(g.add(-3), new Frac(1, 3), "add BigInteger 10/3 + (-3)");
    }

    private static void TestSubBigInteger()
    {
        var a = new Frac(3, 2);
        AssertEqual(a.sub(1), new Frac(1, 2), "sub BigInteger 3/2 - 1");

        var b = new Frac(-1, 2);
        AssertEqual(b.sub(1), new Frac(-3, 2), "sub BigInteger -1/2 - 1");

        var c = new Frac(5, 3);
        AssertEqual(c.sub(-1), new Frac(8, 3), "sub BigInteger 5/3 - (-1)");

        var d = new Frac(0, 1);
        AssertEqual(d.sub(5), new Frac(-5, 1), "sub BigInteger 0 - 5");

        var e = new Frac(7, 4);
        AssertEqual(e.sub(0), e, "sub BigInteger 7/4 - 0");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.sub(5), new Frac(1, 0), "sub BigInteger +inf - 5");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.sub(-3), new Frac(-1, 0), "sub BigInteger -inf - (-3)");

        var nan = new Frac(0, 0);
        AssertEqual(nan.sub(1), new Frac(0, 0), "sub BigInteger nan - 1");

        var f = new Frac(-7, 3);
        AssertEqual(f.sub(3), new Frac(-16, 3), "sub BigInteger -7/3 - 3");

        var g = new Frac(10, 3);
        AssertEqual(g.sub(-3), new Frac(19, 3), "sub BigInteger 10/3 - (-3)");
    }

    private static void TestMulBigInteger()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.mul(2), new Frac(1, 1), "mul BigInteger 1/2 * 2");

        var b = new Frac(-3, 4);
        AssertEqual(b.mul(2), new Frac(-3, 2), "mul BigInteger -3/4 * 2");

        var c = new Frac(5, 3);
        AssertEqual(c.mul(-3), new Frac(-5, 1), "mul BigInteger 5/3 * -3");

        var d = new Frac(0, 1);
        AssertEqual(d.mul(7), new Frac(0, 1), "mul BigInteger 0 * 7");

        var e = new Frac(7, 4);
        AssertEqual(e.mul(0), new Frac(0, 1), "mul BigInteger 7/4 * 0");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.mul(2), new Frac(1, 0), "mul BigInteger +inf * 2");

        AssertEqual(posInf.mul(-1), new Frac(-1, 0), "mul BigInteger +inf * -1");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.mul(2), new Frac(-1, 0), "mul BigInteger -inf * 2");

        AssertEqual(negInf.mul(-1), new Frac(1, 0), "mul BigInteger -inf * -1");

        var nan = new Frac(0, 0);
        AssertEqual(nan.mul(5), new Frac(0, 0), "mul BigInteger nan * 5");
    }

    private static void TestDivBigInteger()
    {
        var a = new Frac(1, 2);
        AssertEqual(a.div(2), new Frac(1, 4), "div BigInteger 1/2 / 2");

        var b = new Frac(-3, 4);
        AssertEqual(b.div(2), new Frac(-3, 8), "div BigInteger -3/4 / 2");

        var c = new Frac(5, 3);
        AssertEqual(c.div(-5), new Frac(-1, 3), "div BigInteger 5/3 / -5");

        var d = new Frac(0, 1);
        AssertEqual(d.div(7), new Frac(0, 1), "div BigInteger 0 / 7");

        var e = new Frac(7, 4);
        AssertEqual(e.div(-1), new Frac(-7, 4), "div BigInteger 7/4 / -1");

        var posInf = new Frac(1, 0);
        AssertEqual(posInf.div(2), new Frac(1, 0), "div BigInteger +inf / 2");

        AssertEqual(posInf.div(-1), new Frac(-1, 0), "div BigInteger +inf / -1");

        var negInf = new Frac(-1, 0);
        AssertEqual(negInf.div(2), new Frac(-1, 0), "div BigInteger -inf / 2");

        AssertEqual(negInf.div(-1), new Frac(1, 0), "div BigInteger -inf / -1");

        var nan = new Frac(0, 0);
        AssertEqual(nan.div(5), new Frac(0, 0), "div BigInteger nan / 5");
    }

    private static void TestIsLegal()
    {
        var a = new Frac(1, 2);
        AssertEqualBool(a.isLegal(), true, "isLegal 1/2");

        var b = new Frac(0, 1);
        AssertEqualBool(b.isLegal(), true, "isLegal 0");

        var c = new Frac(-5, 3);
        AssertEqualBool(c.isLegal(), true, "isLegal -5/3");

        var posInf = new Frac(1, 0);
        AssertEqualBool(posInf.isLegal(), false, "isLegal +inf");

        var negInf = new Frac(-1, 0);
        AssertEqualBool(negInf.isLegal(), false, "isLegal -inf");

        var nan = new Frac(0, 0);
        AssertEqualBool(nan.isLegal(), false, "isLegal nan");

        var d = new Frac(2, 4);
        AssertEqualBool(d.isLegal(), true, "isLegal 2/4");

        var e = new Frac(10, -2);
        AssertEqualBool(e.isLegal(), true, "isLegal 10/-2");

        var f = new Frac(BigInteger.Zero, BigInteger.One);
        AssertEqualBool(f.isLegal(), true, "isLegal BigInteger zero");

        var g = new Frac(BigInteger.One, BigInteger.Zero);
        AssertEqualBool(g.isLegal(), false, "isLegal BigInteger +inf");
    }

    private static void TestIsInteger()
    {
        var a = new Frac(2);
        AssertEqualBool(a.isInteger(), true, "isInteger 2");

        var b = new Frac(4, 2);
        AssertEqualBool(b.isInteger(), true, "isInteger 4/2");

        var c = new Frac(3, 2);
        AssertEqualBool(c.isInteger(), false, "isInteger 3/2");

        var d = new Frac(0, 1);
        AssertEqualBool(d.isInteger(), true, "isInteger 0");

        var e = new Frac(-5, 1);
        AssertEqualBool(e.isInteger(), true, "isInteger -5");

        var f = new Frac(-5, 3);
        AssertEqualBool(f.isInteger(), false, "isInteger -5/3");

        var posInf = new Frac(1, 0);
        AssertEqualBool(posInf.isInteger(), false, "isInteger +inf");

        var negInf = new Frac(-1, 0);
        AssertEqualBool(negInf.isInteger(), false, "isInteger -inf");

        var nan = new Frac(0, 0);
        AssertEqualBool(nan.isInteger(), false, "isInteger nan");

        var g = new Frac(6, 3);
        AssertEqualBool(g.isInteger(), true, "isInteger 6/3");
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
}

