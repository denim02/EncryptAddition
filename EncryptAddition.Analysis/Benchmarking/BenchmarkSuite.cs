using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.Crypto;
using System.Numerics;

namespace EncryptAddition.Analysis.Benchmarking
{
    public class BenchmarkSuite
    {
        private AlgorithmBenchmarker _algorithmBenchmarker;

        private int _bitLength;
        private int BitLength
        {
            get => _bitLength;
            set
            {
                if (EncryptionType == EncryptionChoice.ELGAMAL && value < 3)
                    throw new ArgumentOutOfRangeException("primeBitLength", "The prime bit length must be 3 or greater for ElGamal.");
                if (EncryptionType == EncryptionChoice.PAILLIER && value < 2)
                    throw new ArgumentOutOfRangeException("primeBitLength", "The prime bit length must be 2 or greater for Paillier.");

                _bitLength = value;
            }
        }
        public EncryptionChoice EncryptionType { get; }

        public BenchmarkSuite(EncryptionChoice encryptionType, int primeBitLength)
        {
            EncryptionType = encryptionType;
            BitLength = primeBitLength;

            _algorithmBenchmarker = new AlgorithmBenchmarker(encryptionType, primeBitLength);
        }

        public BenchmarkResult RunBenchmarks(params BigInteger[] values)
        {
            if (values.Length == 0)
                throw new InvalidOperationException("Cannot run benchmarks without any input values.");

            return values.Length == 1 ? RunBenchmarksWithoutAddition(values[0]) : RunBenchmarksWithAddition(values);
        }

        private BenchmarkResult RunBenchmarksWithoutAddition(BigInteger value)
        {
            double keyGenTime = _algorithmBenchmarker.TimeToGenerateKeys();
            (double encryptTime, CipherText cipher) = _algorithmBenchmarker.TimeToEncrypt(value);
            (double decryptTime, BigInteger result) = _algorithmBenchmarker.TimeToDecrypt(cipher);

            return new BenchmarkResult(EncryptionType.ToString(), BitLength, _algorithmBenchmarker.GetMaxPlaintextSize(), keyGenTime, encryptTime, decryptTime, result, new CipherText[] { cipher });
        }

        private BenchmarkResult RunBenchmarksWithAddition(BigInteger[] values)
        {
            // Determine if the sum is larger than the max value for the bit length.
            BigInteger sum = values.Aggregate((a, b) => a + b);

            if (sum > _algorithmBenchmarker.GetMaxPlaintextSize())
                throw new ArgumentException($"The sum of the arguments provided exceeds the max plaintext size supported by the {EncryptionType} algorithm. The sum should be <= {_algorithmBenchmarker.GetMaxPlaintextSize()}.");

            // Run benchmarks for the algorithm
            double keyGenTime = _algorithmBenchmarker.TimeToGenerateKeys();
            (double encryptTime, CipherText[] ciphers) = _algorithmBenchmarker.TimeToEncrypt(values);
            (double addTime, CipherText cipher) = _algorithmBenchmarker.TimeToAdd(ciphers);
            (double decryptTime, BigInteger result) = _algorithmBenchmarker.TimeToDecrypt(cipher);

            return new BenchmarkResult(EncryptionType.ToString(), BitLength, _algorithmBenchmarker.GetMaxPlaintextSize(), keyGenTime, encryptTime, decryptTime, result, ciphers.Append(cipher).ToArray(), addTime);
        }
    }
}
