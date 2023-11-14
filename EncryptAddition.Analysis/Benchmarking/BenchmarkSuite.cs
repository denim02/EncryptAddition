using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.Crypto;
using EncryptAddition.Crypto.ElGamal;
using EncryptAddition.Crypto.Paillier;
using System.Numerics;

namespace EncryptAddition.Analysis.Benchmarking
{
    public enum EncryptionStrategy { PAILLIER, ELGAMAL }

    public class BenchmarkSuite
    {
        private AlgorithmBenchmarker _algorithmBenchmarker;

        private int _bitLength;
        private int BitLength
        {
            get => _bitLength;
            set
            {
                if (EncryptionType == EncryptionStrategy.ELGAMAL && value < 3)
                    throw new ArgumentOutOfRangeException("primeBitLength", "The prime bit length must be 3 or greater for ElGamal.");
                if (EncryptionType == EncryptionStrategy.PAILLIER && value < 2)
                    throw new ArgumentOutOfRangeException("primeBitLength", "The prime bit length must be 2 or greater for Paillier.");

                _bitLength = value;
            }
        }
        public EncryptionStrategy EncryptionType { get; }

        public BenchmarkSuite(EncryptionStrategy encryptionType, int primeBitLength)
        {
            EncryptionType = encryptionType;
            BitLength = primeBitLength;

            _algorithmBenchmarker = EncryptionType == EncryptionStrategy.ELGAMAL ? new AlgorithmBenchmarker(new ElGamalEncryption(BitLength)) : new AlgorithmBenchmarker(new PaillierEncryption(BitLength));
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
            (double decryptTime, _) = _algorithmBenchmarker.TimeToDecrypt(cipher);

            return new BenchmarkResult(EncryptionType.ToString(), BitLength, keyGenTime, encryptTime, decryptTime, new CipherText[] { cipher });
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
            (double decryptTime, _) = _algorithmBenchmarker.TimeToDecrypt(cipher);

            return new BenchmarkResult(EncryptionType.ToString(), BitLength, keyGenTime, encryptTime, decryptTime, ciphers.Append(cipher).ToArray(), addTime);
        }
    }
}
