using EncryptAddition.Crypto;
using EncryptAddition.Crypto.ElGamal;
using System.Numerics;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int primeBitLength = 128;

            Console.WriteLine("ElGamal Algorithm");
            Console.WriteLine("==================");

            IEncryptionStrategy elGamal = new ElGamalEncryption(primeBitLength);

            int[] unencryptedNumbers = { 3, 4, 5, 6, 1, 2, 7, 8, 9 };

            CipherText[] elGamalCiphers = unencryptedNumbers.Select(x => elGamal.Encrypt(x)).ToArray();

            for (var i = 0; i < elGamalCiphers.Length; i++)
            {
                Console.WriteLine($"Plain: {unencryptedNumbers[i]}\tCipher: {elGamalCiphers[i].EncryptedMessage}");
            }

            CipherText elGamalSum = elGamal.Add(elGamalCiphers);
            BigInteger decryptedElGamal = elGamal.Decrypt(elGamalSum);

            Console.WriteLine($"Decrypted sum: {decryptedElGamal}\tEncrypted sum: {elGamalSum.EncryptedMessage}");
        }
    }
}
