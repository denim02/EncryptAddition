using EncryptAddition.Crypto;
using System;
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
            Random random = new();

            BigInteger prime = Helpers.GenerateSafePrime(32);
            BigInteger generator = Helpers.FindGeneratorForSafePrime(prime);

            ElGamal elGamal = new(prime, generator);

            int[] unencryptedNumbers = { 500, 70, 1, 0 };
            (BigInteger r, BigInteger c)[] numbers = unencryptedNumbers.Select(x => elGamal.Encrypt(x)).ToArray();
            Console.WriteLine(numbers);


            (BigInteger Rmult, BigInteger cmult) = numbers.Aggregate((number1, number2) => Add(number1.Item1, number1.Item2, number2.Item1, number2.Item2, prime)); ;

            var decryptedData = elGamal.Decrypt((Rmult, cmult));

            BigInteger prime1 = Helpers.GenerateSafePrime(32);
            BigInteger prime2 = Helpers.GenerateSafePrime(32);
            while (prime1 == prime2)
                prime2 = Helpers.GenerateSafePrime(32);

            Paillier paillier = new(prime1, prime2);
            BigInteger value1 = paillier.Encrypt(500);
            BigInteger value2 = paillier.Encrypt(70);

            BigInteger decrypt = paillier.Decrypt(value1);
            BigInteger result = paillier.Decrypt(paillier.Add(value1, value2));


            InitializeComponent();
        }

        public static (BigInteger, BigInteger) Add(BigInteger r1, BigInteger c1, BigInteger r2, BigInteger c2, BigInteger prime)
        {
            var Rmult = BigInteger.Multiply(r1 % prime, r2 % prime) % prime;
            var cmult = BigInteger.Multiply(c1 % prime, c2 % prime) % prime;

            return (Rmult, cmult);
        }
    }
}
