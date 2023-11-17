using EncryptAddition.Analysis.Utils;
using EncryptAddition.Crypto;
using EncryptAddition.Crypto.ElGamal;
using EncryptAddition.Crypto.Paillier;
using System.Numerics;

namespace EncryptAddition.Analysis.Benchmarking
{
    public class AlgorithmBenchmarker
    {
        private IEncryptionStrategy _asymmetricAlgorithm;
        private double _keyGenerationTime;

        public AlgorithmBenchmarker(EncryptionChoice encryptionStrategy, int primeBitLength)
        {
            _keyGenerationTime = Profiling.Profile(() => { return encryptionStrategy == EncryptionChoice.ElGamal ? new ElGamalEncryption(primeBitLength) : new PaillierEncryption(primeBitLength); }, out _asymmetricAlgorithm);
        }

        public BigInteger GetMaxPlaintextSize()
        {
            return _asymmetricAlgorithm.MaxPlaintextSize;
        }

        public (double ExecutionTime, CipherText Cipher) TimeToEncrypt(BigInteger input)
        {
            double executionTime = Profiling.Profile(() => _asymmetricAlgorithm.Encrypt(input), out CipherText cipher);
            return (executionTime, cipher);
        }

        public (double ExecutionTime, CipherText[] Ciphers) TimeToEncrypt(BigInteger[] inputs)
        {
            double averageExecutionTime = 0;
            CipherText[] encryptedInputs = new CipherText[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                double executionTime = Profiling.Profile(() => _asymmetricAlgorithm.Encrypt(inputs[i]), out encryptedInputs[i]);
                averageExecutionTime += executionTime;
            }

            averageExecutionTime /= inputs.Length;
            return (averageExecutionTime, encryptedInputs);
        }

        public (double ExecutionTime, BigInteger Result) TimeToDecrypt(CipherText input)
        {
            double executionTime = Profiling.Profile(() => _asymmetricAlgorithm.Decrypt(input), out BigInteger result);
            return (executionTime, result);
        }

        public (double ExecutionTime, BigInteger[] Results) TimeToDecrypt(CipherText[] ciphers)
        {
            double averageExecutionTime = 0;
            BigInteger[] decryptedResults = new BigInteger[ciphers.Length];

            for (int i = 0; i < ciphers.Length; i++)
            {
                double executionTime = Profiling.Profile(() => _asymmetricAlgorithm.Decrypt(ciphers[i]), out decryptedResults[i]);
                averageExecutionTime += executionTime;
            }

            averageExecutionTime /= ciphers.Length;
            return (averageExecutionTime, decryptedResults);
        }

        public (double ExecutionTime, CipherText Result) TimeToAdd(params CipherText[] ciphers)
        {
            double executionTime = Profiling.Profile(() => _asymmetricAlgorithm.Add(ciphers), out CipherText result);
            return (executionTime, result);
        }

        public double TimeToGenerateKeys()
        {
            return _keyGenerationTime;
        }
    }
}