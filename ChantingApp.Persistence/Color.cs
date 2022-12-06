using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ChantingApp.Persistence;

public struct Color : IParsable<Color>,
                      IEquatable<Color>,
                      IEqualityOperators<Color, Color, bool>
{
    private readonly int _r, _g, _b;

    public Color() : this(0, 0, 0)
    {
    }

    public Color(int r, int g, int b)
    {
        if (r is > 255 or < 0) throw new Exception($"Invalid value {r}");
        if (g is > 255 or < 0) throw new Exception($"Invalid value {g}");
        if (b is > 255 or < 0) throw new Exception($"Invalid value {b}");

        (_r, _g, _b) = (r, g, b);
    }

    public override string ToString()
    {
        return $"#{_r:X2}{_g:X2}{_b:X2}";
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_r, _g, _b);
    }

    public override bool Equals(object? obj)
    {
        return obj is Color c && c == this;
    }

    public static Color Parse(string s, IFormatProvider? provider)
    {
        var parseResult = ColorParser.ParseColor(s);
        if (parseResult.Succeeded)
            return parseResult.Result;
        throw new Exception($"Cannot parse value <<<{s}>>> into a valid color");
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out Color result)
    {
        var parseResult = ColorParser.ParseColor(s);
        result = parseResult.Succeeded ? parseResult.Result : default;
        return parseResult.Succeeded;
    }

    public bool Equals(Color other)
    {
        return other._r == _r &&
               other._g == _g &&
               other._b == _b;
    }

    public static bool operator ==(Color left, Color right)
    {
        return left._r == right._r &&
               left._g == right._g &&
               left._b == right._b;
    }

    public static bool operator !=(Color left, Color right)
    {
        return left._r != right._r ||
               left._g != right._g ||
               left._b != right._b;
    }
}

static partial class ColorParser
{
    private const string HexCodeRegex = @"^\#[0-9A-F]{6}$";

    [GeneratedRegex(HexCodeRegex, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 1000)]
    private static partial Regex GetColorRegex();

    public static ColorParseResult ParseColor(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return ColorParseResult.Failure();
        }

        var regex = GetColorRegex();
        var match = regex.Match(value);
        if (!match.Success)
        {
            return ColorParseResult.Failure();
        }

        var r = int.Parse(match.ValueSpan[1..3], style: NumberStyles.HexNumber);
        var g = int.Parse(match.ValueSpan[3..5], style: NumberStyles.HexNumber);
        var b = int.Parse(match.ValueSpan[5..], style: NumberStyles.HexNumber);

        return ColorParseResult.Success(new Color(r, g, b));
    }
}

class ColorParseResult
{
    private Color? _result;

    private ColorParseResult(bool succeeded, Color? result)
    {
        Succeeded = succeeded;
        _result = result;
    }

    public bool Succeeded { get; }
    public Color Result => _result ?? throw new InvalidOperationException("Cannot get Result property for a non successful result");

    public static ColorParseResult Success(Color result) => new(true, result);
    public static ColorParseResult Failure() => new(false, null);
}