Frac 类型文档
========================

概述
----

Frac 是一个基于 `System.Numerics.BigInteger` 的有理数类型，支持无穷大和 NaN 等扩展值。

- 内部表示：分子 `x`、分母 `y`（均为 `BigInteger`）
- 约定：所有 **public 方法** 返回的 Frac 对象满足
  - 分母 `y >= 0`
  - `gcd(|x|, y) == 1`（既约分数）
  - 特殊值使用分母 `y == 0`
- 典型用途：实现可精确表示有理数的数学运算逻辑。

项目目录
--------

- `Frac.cs`：Frac 类型的实现，包含构造、属性、基本运算方法等。使用时只需要这一个文件就够了。
- `FracDebug.cs`：测试脚本。在 Unity 中挂载到任意 Empty 对象上以使用。
- `Readme.md`：本文档，详细介绍 Frac 类型的设计、使用方法和实现细节。

内部约简规则
------------

内部私有方法 `simplify()` 负责：

- 计算 `gcd(x, y)`，若 gcd 为 0（通常是 `(0,0)`），则不做任何操作
- 将 `(x, y)` 同时除以 gcd，得到既约分数
- 保证分母非负：若 `y < 0`，则令 `x = -x, y = -y`

构造与属性
----------

- `public Frac(BigInteger x)`  
  - 表示整数 `x/1`。
- `public Frac(BigInteger x, BigInteger y)`  
  - 表示分数 `x/y`，构造时会自动 `simplify()`。
  - 当 `y == 0` 时，用于表示无穷和 NaN（见“特殊值”）。
- `private Frac(BigInteger x, BigInteger y, bool simplify)`  
  - 内部使用的构造函数：
    - `simplify == true`：等同于公开的两参数构造，会做约分和符号标准化。
    - `simplify == false`：只保证分母非负，不做约分（用于高效构造中间结果）。

属性：

- `Numerator`：分子（`BigInteger`），设置后会自动调用 `simplify()`。
- `Denominator`：分母（`BigInteger`），设置后会自动调用 `simplify()`。

特殊值约定
----------

特殊值全部通过 `y == 0` 来表示，只在 `ToString()` 中被区分：

- `x == 0, y == 0`：`"nan"`（非数值）
- `x == 1, y == 0`：`"+inf"`（正无穷）
- `x == -1, y == 0`：`"-inf"`（负无穷）
- 其它 `(x != 0, y == 0)`：视为非法内部状态，`ToString()` 会抛出异常（说明 `simplify()` 或构造调用错误）。

典型构造方式：

- `new Frac(0, 0)`：得到 NaN
- `new Frac(1, 0)`：得到正无穷
- `new Frac(-1, 0)`：得到负无穷

基本运算方法
------------

倒数与相反数：

- `Frac Reciprocal()`  
  - 返回 `y/x`，使用内部构造 `new Frac(y, x, false)`。
  - 对于普通有理数 `a/b`，得到 `b/a`。
  - 对于 `0`（如 `0/1`），得到 `+inf`（`1/0`）。
  - 对于 `+inf`（`1/0`），得到 `0`（`0/1`）。
  - 对于 NaN（`0/0`），结果仍然是 NaN（`0/0`）。

- `Frac Neg()`  
  - 返回 `-x/y`，使用 `new Frac(-x, y, false)`。
  - 无论普通数还是特殊值，都仅改变符号。

乘法与除法（Frac 与 Frac）
--------------------------

- `Frac Mul(Frac other)`  
  - 乘法前会做一次“交叉约分”，避免数值过大：
    - `gcd1 = GCD(x, other.y)`
    - `gcd2 = GCD(other.x, y)`
  - 若 `gcd1 == 0` 或 `gcd2 == 0`，说明出现了零和特殊值相乘，直接返回 `new Frac(0, 0, false)`（NaN）。
  - 否则按
    - 分子：`(x / gcd1) * (other.x / gcd2)`
    - 分母：`(y / gcd2) * (other.y / gcd1)`
    - 构造：`new Frac(..., ..., false)`（约分靠之前 GCD）。

- `Frac Div(Frac other)`  
  - 定义为 `Mul(other.Reciprocal())`。
  - 结合 `Reciprocal()` 和 `Mul()` 的逻辑，可以表示：
    - `a / 0` → ±∞ 或 NaN（根据符号和后续运算）。
    - `0 / 0` → NaN。

加法与减法（Frac 与 Frac）
-------------------------

- `Frac Add(Frac other)`  
  - 计算 `gcd = GCD(y, other.y)`。
  - 若 `gcd == 0`（两者分母都为 0）：
    - 若 `x == 1` 且 `other.x == 1` → `+inf`（`new Frac(1, 0, false)`）
    - 若 `x == -1` 且 `other.x == -1` → `-inf`（`new Frac(-1, 0, false)`）
    - 其它情况 → NaN（`new Frac(0, 0, false)`）
  - 若 `gcd != 0`：
    - `m1 = y / gcd`
    - `m2 = other.y / gcd`
    - `numerator = x * m2 + other.x * m1`
    - `denominator = (y / gcd) * other.y`
    - 使用 `new Frac(numerator, denominator, true)`，必须约分以处理如 `1/2 + 1/2 = 1`。

- `Frac Sub(Frac other)`  
  - 定义为 `Add(other.Neg())`。

与 BigInteger 的运算
--------------------

这些方法提供 Frac 与整数之间的便捷操作：

- `Frac Add(BigInteger other)`  
  - 计算 `x + other * y`，分母保持 `y`。
- `Frac Sub(BigInteger other)`  
  - 计算 `x - other * y`。
- `Frac Mul(BigInteger other)`  
  - 调用 `Mul(new Frac(other))`。
- `Frac Div(BigInteger other)`  
  - 调用 `Div(new Frac(other))`。

合法性、整数与 NaN 判断
----------------------

- `bool IsLegal()`  
  - 返回 `!y.IsZero`，即分母非 0 时为合法有理数。
  - 对于 NaN 和 ±∞（分母为 0），返回 `false`。

- `bool IsInteger()`  
  - 返回 `y.IsOne`，即分母为 1 时为整数。
  - 对于分母 0 的特殊值，返回 `false`。

- `bool IsNaN()`  
  - 返回 `x == 0 && y == 0`，用于判断当前值是否为 NaN。

转换与取整
----------

- `double ToDouble()`  
  - 若为普通有理数（`IsLegal() == true`），返回 `((double)x) / ((double)y)` 的值。
  - 若为 `+inf`（`1/0`），返回 `double.PositiveInfinity`。
  - 若为 `-inf`（`-1/0`），返回 `double.NegativeInfinity`。
  - 若为 NaN（`0/0`），返回 `double.NaN`。

- `float ToFloat()`  
  - 等价于对 `ToDouble()` 的结果再做一次 `(float)` 强制转换。
  - 同样会将 `+inf/-inf/nan` 映射到对应的 `float` 特殊值。

- `BigInteger ToBigInteger()`  
  - 仅对有限值定义（`IsLegal() == true`），否则抛出异常。
  - 若 `IsInteger() == true`，直接返回分子 `x`。
  - 若不是整数，则返回向负无穷取整后的结果：
    - 例如 `5/3` → `1`，`-5/3` → `-2`，`-1/2` → `-1`。

字符串表示
----------

- `override string ToString()`：
  - 若 `y == 0`：
    - `x == 0` → `"nan"`
    - `x == 1` → `"+inf"`
    - `x == -1` → `"-inf"`
    - 其它 → 抛出异常（内部状态错误）。
  - 若 `y == 1`：返回整数形式 `x.ToString()`。
  - 其它：返回 `"x/y"` 形式（如 `"3/4"`）。

操作符重载
----------

Frac 提供若干操作符重载，全部委托给前面定义的实例方法：

- 加法：
  - `static Frac operator +(Frac a, Frac b)` → `a.Add(b)`
  - `static Frac operator +(Frac a, BigInteger b)` → `a.Add(b)`
  - `static Frac operator +(BigInteger a, Frac b)` → `b.Add(a)`

- 减法：
  - `static Frac operator -(Frac a, Frac b)` → `a.Sub(b)`
  - `static Frac operator -(Frac a, BigInteger b)` → `a.Sub(b)`
  - `static Frac operator -(BigInteger a, Frac b)` → `b.Sub(a).Neg()`

- 乘法：
  - `static Frac operator *(Frac a, Frac b)` → `a.Mul(b)`
  - `static Frac operator *(Frac a, BigInteger b)` → `a.Mul(b)`
  - `static Frac operator *(BigInteger a, Frac b)` → `b.Mul(a)`

- 相等与比较：
  - `static bool operator ==(Frac a, Frac b)`：按数值判断有理数是否相等；NaN 与任何值都不相等。
  - `static bool operator !=(Frac a, Frac b)`：等价于 `!(a == b)`。
  - `static bool operator >(Frac a, Frac b)` / `<` / `>=` / `<=`：按有理数大小比较，支持 ±∞。
  - 同时提供 `Frac` 与 `BigInteger` 之间的 `==, !=, >, <, >=, <=` 运算符，方便写出 `frac > 1`、`2 <= frac` 等表达式。

在 C# 中，`+=`、`-=`、`*=` 等复合赋值运算会自动使用上面的 `+`、`-`、`*` 运算符，因此本类型无需额外定义 `operator +=` 等函数即可直接使用，例如 `a += b`、`a *= (BigInteger)2`。注意本类型没有支持除法复合赋值 `/=`。

示例
----

普通有理数：

- `new Frac(2, 4)` → 自动化简为 `1/2`
- `new Frac(-2, -4)` → 自动化简为 `1/2`
- `new Frac(1, -2)` → 标准化为 `-1/2`

基本运算：

- `new Frac(1, 2) + new Frac(1, 3)` → `5/6`
- `new Frac(1, 2) * new Frac(2, 3)` → `1/3`
- `new Frac(3, 2).IsInteger()` → `false`
- `new Frac(4, 2).IsInteger()` → `true`

复合赋值与比较运算示例：

- `var a = new Frac(1, 2); var b = new Frac(1, 3); a += b;` → `a == 5/6`
- `var c = new Frac(1, 2); c *= (BigInteger)2;` → `c == 1`
- `new Frac(1, 2) > new Frac(1, 3)` → `true`
- `new Frac(1, 2) == new Frac(2, 4)` → `true`
- `new Frac(1, 0) > new Frac(0, 1)` → `true`（`+inf` 大于任意有限数）
- `var nan = new Frac(0, 0);` 中，`nan == nan` 为 `false`，`nan != nan` 为 `true`，`nan.IsNaN()` 为 `true`，且所有 `<, >, <=, >=` 比较都为 `false`。

特殊值示例：

- `new Frac(1, 0).ToString()` → `"+inf"`
- `new Frac(-1, 0).ToString()` → `"-inf"`
- `new Frac(0, 0).ToString()` → `"nan"`

