using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace ES.XxHash
{
  [TestFixture]
  [ExcludeFromCodeCoverage]
  public sealed class XxHashTf
  {
    [Test]
    [TestCase(0UL, new byte[] { })]
    [TestCase(9962287286179718960UL, new byte[] {1})]
    [TestCase(8827404902424034886UL, new byte[] {1, 2})]
    [TestCase(8376154270085342629UL, new byte[] {1, 2, 3})]
    [TestCase(6063570110359613137UL, new byte[] {1, 2, 3, 4})]
    [TestCase(10197745259203581487UL, new byte[] {1, 2, 3, 4, 5})]
    [TestCase(8864009169433841820UL, new byte[] {1, 2, 3, 4, 5, 6})]
    [TestCase(12154783592163663049UL, new byte[] {1, 2, 3, 4, 5, 6, 7})]
    [TestCase(9316896406413536788UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8})]
    [TestCase(11372013628968716477UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9})]
    [TestCase(14320912991164269784UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10})]
    [TestCase(1322912620761971483UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11})]
    [TestCase(13996535720148396453UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12})]
    [TestCase(6041232589023366369UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13})]
    [TestCase(1426934014501647993UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})]
    [TestCase(7981396696738583322UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15})]
    [TestCase(4291993593311583621UL, new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16})]
    [TestCase(17009353642554696703UL,
      new byte[]
      {
        1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 1,
        2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16
      })]
    public void TestByteArray(ulong expected, byte[] b)
    {
      var hash = b.XxHash();
      Assert.AreEqual(expected, hash);
    }

    [Test]
    [TestCase(17241709254077376921UL, "")]
    [TestCase(12647288196429669931UL, "hello world")]
    [TestCase(11451410659828637527UL,
      "hello world xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")]
    public void TestString(ulong expected, string s)
    {
      var hash = s.XxHash();
      Assert.AreEqual(expected, hash);
    }
  }
}