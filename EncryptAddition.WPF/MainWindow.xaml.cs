using EncryptAddition.Analysis.Benchmarking;
using EncryptAddition.Crypto;
using EncryptAddition.Crypto.ElGamal;
using EncryptAddition.Crypto.Paillier;
using System.Linq;
using System.Numerics;
using System.Windows;

namespace EncryptAddition.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            int primeBitLength = 32;

            ElGamalEncryption elGamal = new(primeBitLength);

            int[] unencryptedNumbers = { 3, 4, 5, 6, 1, 2, 7, 8, 9 };

            CipherText[] elGamalCiphers = unencryptedNumbers.Select(x => elGamal.Encrypt(x)).ToArray();
            CipherText elGamalSum = elGamal.Add(elGamalCiphers);
            BigInteger decryptedElGamal = elGamal.Decrypt(elGamalSum);

            PaillierEncryption paillier = new(primeBitLength);

            CipherText[] paillierCiphers = unencryptedNumbers.Select(x => paillier.Encrypt(x)).ToArray();
            CipherText paillierSum = paillier.Add(paillierCiphers);
            BigInteger decryptedPaillier = paillier.Decrypt(paillierSum);

            AlgorithmBenchmark paillierBenchmark = new(paillier);
            (double timeToEncrypt, CipherText cipher) = paillierBenchmark.TimeToEncrypt(12312421);
            (double timeToDecrypt, _) = paillierBenchmark.TimeToDecrypt(cipher);

            InitializeComponent();
        }
    }
}
