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

            ElGamalAlgorithm elGamal = new(primeBitLength);

            int[] unencryptedNumbers = { 500, 70, 1, 0 };

            ElGamalCipherText[] elGamalCiphers = unencryptedNumbers.Select(x => elGamal.Encrypt(x)).ToArray();
            ElGamalCipherText elGamalSum = elGamal.Add(elGamalCiphers);
            BigInteger decryptedElGamal = elGamal.Decrypt(elGamalSum);

            PaillierAlgorithm paillier = new(primeBitLength);

            PaillierCipherText[] paillierCiphers = unencryptedNumbers.Select(x => paillier.Encrypt(x)).ToArray();
            PaillierCipherText paillierSum = paillier.Add(paillierCiphers);
            BigInteger decryptedPaillier = paillier.Decrypt(paillierSum);

            InitializeComponent();
        }
    }
}
