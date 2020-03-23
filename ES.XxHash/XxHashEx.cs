using System;
using System.Diagnostics.CodeAnalysis;

namespace ES.XxHash
{
  public static class XxHashEx
  {
    private static readonly bool IsLittle = BitConverter.IsLittleEndian;

    [ExcludeFromCodeCoverage]
    private static unsafe ulong LoadULong(byte* source)
    {
      if (IsLittle)
        return source[0]
          | ((ulong) source[1] << 8)
          | ((ulong) source[2] << 16)
          | ((ulong) source[3] << 24)
          | ((ulong) source[4] << 32)
          | ((ulong) source[5] << 40)
          | ((ulong) source[6] << 48)
          | ((ulong) source[7] << 56);
      return ((ulong) source[0] << 56)
        | ((ulong) source[1] << 48)
        | ((ulong) source[2] << 40)
        | ((ulong) source[3] << 32)
        | ((ulong) source[4] << 24)
        | ((ulong) source[5] << 16)
        | ((ulong) source[6] << 8)
        | source[7];
    }

    [ExcludeFromCodeCoverage]
    private static unsafe uint LoadUInt(byte* source)
    {
      if (IsLittle)
        return source[0]
          | ((uint) source[1] << 8)
          | ((uint) source[2] << 16)
          | ((uint) source[3] << 24);
      return ((uint) source[0] << 24)
        | ((uint) source[1] << 16)
        | ((uint) source[2] << 8)
        | source[3];
    }

    public static unsafe ulong XxHash(this string s, ulong seed = 0)
    {
      fixed (char* c = s)
      {
        var len = (ulong) s.Length * 2;
        var p = (byte*) c;
        var bEnd = p + len;

        ulong h64;

        if (len >= 32)
        {
          var limit = bEnd - 32;
          var v1 = seed + PRIME64_1 + PRIME64_2;
          var v2 = seed + PRIME64_2;
          var v3 = seed + 0;
          var v4 = seed - PRIME64_1;

          do
          {
            v1 += LoadULong(p) * PRIME64_2;
            p += 8;
            v1 = (v1 << 31) | (v1 >> (64 - 31));
            v1 *= PRIME64_1;
            v2 += LoadULong(p) * PRIME64_2;
            p += 8;
            v2 = (v2 << 31) | (v2 >> (64 - 31));
            v2 *= PRIME64_1;
            v3 += LoadULong(p) * PRIME64_2;
            p += 8;
            v3 = (v3 << 31) | (v3 >> (64 - 31));
            v3 *= PRIME64_1;
            v4 += LoadULong(p) * PRIME64_2;
            p += 8;
            v4 = (v4 << 31) | (v4 >> (64 - 31));
            v4 *= PRIME64_1;
          } while (p <= limit);

          h64 = ((v1 << 1) | (v1 >> (64 - 1))) + ((v2 << 7) | (v2 >> (64 - 7))) +
            ((v3 << 12) | (v3 >> (64 - 12))) + ((v4 << 18) | (v4 >> (64 - 18)));

          v1 *= PRIME64_2;
          v1 = (v1 << 31) | (v1 >> (64 - 31));
          v1 *= PRIME64_1;
          h64 ^= v1;
          h64 = h64 * PRIME64_1 + PRIME64_4;

          v2 *= PRIME64_2;
          v2 = (v2 << 31) | (v2 >> (64 - 31));
          v2 *= PRIME64_1;
          h64 ^= v2;
          h64 = h64 * PRIME64_1 + PRIME64_4;

          v3 *= PRIME64_2;
          v3 = (v3 << 31) | (v3 >> (64 - 31));
          v3 *= PRIME64_1;
          h64 ^= v3;
          h64 = h64 * PRIME64_1 + PRIME64_4;

          v4 *= PRIME64_2;
          v4 = (v4 << 31) | (v4 >> (64 - 31));
          v4 *= PRIME64_1;
          h64 ^= v4;
          h64 = h64 * PRIME64_1 + PRIME64_4;
        }
        else
        {
          h64 = seed + PRIME64_5;
        }

        h64 += len;

        while (p + 8 <= bEnd)
        {
          var k1 = LoadULong(p);
          k1 *= PRIME64_2;
          k1 = (k1 << 31) | (k1 >> (64 - 31));
          k1 *= PRIME64_1;
          h64 ^= k1;
          h64 = ((h64 << 27) | (h64 >> (64 - 27))) * PRIME64_1 + PRIME64_4;
          p += 8;
        }

        if (p + 4 <= bEnd)
        {
          h64 ^= LoadUInt(p) * PRIME64_1;
          h64 = ((h64 << 23) | (h64 >> (64 - 23))) * PRIME64_2 + PRIME64_3;
          p += 4;
        }

        while (p < bEnd)
        {
          h64 ^= *p * PRIME64_5;
          h64 = ((h64 << 11) | (h64 >> (64 - 11))) * PRIME64_1;
          p++;
        }

        h64 ^= h64 >> 33;
        h64 *= PRIME64_2;
        h64 ^= h64 >> 29;
        h64 *= PRIME64_3;
        h64 ^= h64 >> 32;

        return h64;
      }
    }

    public static unsafe ulong XxHash(this byte[] b, int offset = 0, int length = 0, ulong seed = 0)
    {
      var len = (ulong) length;

      if (len == 0)
        len = (ulong) b.Length;

      if (len == 0)
        return 0;

      fixed (byte* bp = &b[offset])
      {
        var p = bp;
        var bEnd = p + len;

        ulong h64;

        if (len >= 32)
        {
          var limit = bEnd - 32;
          var v1 = seed + PRIME64_1 + PRIME64_2;
          var v2 = seed + PRIME64_2;
          var v3 = seed + 0;
          var v4 = seed - PRIME64_1;

          do
          {
            v1 += LoadULong(p) * PRIME64_2;
            p += 8;
            v1 = (v1 << 31) | (v1 >> (64 - 31));
            v1 *= PRIME64_1;
            v2 += LoadULong(p) * PRIME64_2;
            p += 8;
            v2 = (v2 << 31) | (v2 >> (64 - 31));
            v2 *= PRIME64_1;
            v3 += LoadULong(p) * PRIME64_2;
            p += 8;
            v3 = (v3 << 31) | (v3 >> (64 - 31));
            v3 *= PRIME64_1;
            v4 += LoadULong(p) * PRIME64_2;
            p += 8;
            v4 = (v4 << 31) | (v4 >> (64 - 31));
            v4 *= PRIME64_1;
          } while (p <= limit);

          h64 = ((v1 << 1) | (v1 >> (64 - 1))) + ((v2 << 7) | (v2 >> (64 - 7))) +
            ((v3 << 12) | (v3 >> (64 - 12))) + ((v4 << 18) | (v4 >> (64 - 18)));

          v1 *= PRIME64_2;
          v1 = (v1 << 31) | (v1 >> (64 - 31));
          v1 *= PRIME64_1;
          h64 ^= v1;
          h64 = h64 * PRIME64_1 + PRIME64_4;

          v2 *= PRIME64_2;
          v2 = (v2 << 31) | (v2 >> (64 - 31));
          v2 *= PRIME64_1;
          h64 ^= v2;
          h64 = h64 * PRIME64_1 + PRIME64_4;

          v3 *= PRIME64_2;
          v3 = (v3 << 31) | (v3 >> (64 - 31));
          v3 *= PRIME64_1;
          h64 ^= v3;
          h64 = h64 * PRIME64_1 + PRIME64_4;

          v4 *= PRIME64_2;
          v4 = (v4 << 31) | (v4 >> (64 - 31));
          v4 *= PRIME64_1;
          h64 ^= v4;
          h64 = h64 * PRIME64_1 + PRIME64_4;
        }
        else
        {
          h64 = seed + PRIME64_5;
        }

        h64 += len;

        while (p + 8 <= bEnd)
        {
          var k1 = LoadULong(p);
          k1 *= PRIME64_2;
          k1 = (k1 << 31) | (k1 >> (64 - 31));
          k1 *= PRIME64_1;
          h64 ^= k1;
          h64 = ((h64 << 27) | (h64 >> (64 - 27))) * PRIME64_1 + PRIME64_4;
          p += 8;
        }

        if (p + 4 <= bEnd)
        {
          h64 ^= LoadUInt(p) * PRIME64_1;
          h64 = ((h64 << 23) | (h64 >> (64 - 23))) * PRIME64_2 + PRIME64_3;
          p += 4;
        }

        while (p < bEnd)
        {
          h64 ^= *p * PRIME64_5;
          h64 = ((h64 << 11) | (h64 >> (64 - 11))) * PRIME64_1;
          p++;
        }

        h64 ^= h64 >> 33;
        h64 *= PRIME64_2;
        h64 ^= h64 >> 29;
        h64 *= PRIME64_3;
        h64 ^= h64 >> 32;

        return h64;
      }
    }

    // ReSharper disable InconsistentNaming
    private const ulong PRIME64_1 = 11400714785074694791UL;
    private const ulong PRIME64_2 = 14029467366897019727UL;
    private const ulong PRIME64_3 = 1609587929392839161UL;
    private const ulong PRIME64_4 = 9650029242287828579UL;

    private const ulong PRIME64_5 = 2870177450012600261UL;
    // ReSharper restore InconsistentNaming
  }
}