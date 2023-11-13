using EncryptAddition.Analysis.Benchmarking;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int primeBitLength = 256;

            ComparisonSuite suite = new ComparisonSuite(primeBitLength);
            var results = suite.RunBenchmarks(4, 2, 3, 5);

            Console.WriteLine(results.PaillierResults);
            Console.WriteLine(results.ElGamalResults);

            //int[] unencryptedNumbers = { 3, 4, 5, 6, 1, 2, 7, 8, 9 };

            //CipherText[] elGamalCiphers = unencryptedNumbers.Select(x => elGamal.Encrypt(x)).ToArray();

            //for (var i = 0; i < elGamalCiphers.Length; i++)
            //{
            //    Console.WriteLine($"Plain: {unencryptedNumbers[i]}\tCipher: {elGamalCiphers[i].EncryptedMessage}");
            //}

            //CipherText elGamalSum = elGamal.Add(elGamalCiphers);
            //BigInteger decryptedElGamal = elGamal.Decrypt(elGamalSum);

            //Console.WriteLine($"Decrypted sum: {decryptedElGamal}\tEncrypted sum: {elGamalSum.EncryptedMessage}");
        }
    }
}
