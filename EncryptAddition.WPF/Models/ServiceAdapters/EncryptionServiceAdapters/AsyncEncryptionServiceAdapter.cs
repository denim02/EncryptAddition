using EncryptAddition.Crypto;
using EncryptAddition.WPF.DataTypes;
using EncryptAddition.WPF.Models.Services;
using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.Models.ServiceAdapters
{
    public class AsyncEncryptionServiceAdapter : IAsyncEncryptionServiceAdapter
    {
        private EncryptionService _encryptionService;
        public bool IsReady { get; private set; } = false;

        public AsyncEncryptionServiceAdapter(EncryptionChoice algorithmChoice, string serializedKey)
        {
            _encryptionService = new EncryptionService(algorithmChoice, serializedKey);
        }

        public AsyncEncryptionServiceAdapter(EncryptionChoice algorithmChoice, int bitLength)
        {
            _encryptionService = new EncryptionService(algorithmChoice, bitLength);
        }

        public Task PrepareService()
        {
            IsReady = true;
            return Task.Run(() => _encryptionService.PrepareService());
        }

        public async Task<EncryptionServiceResult> Encrypt(BigInteger[] inputs)
        {
            if (!IsReady)
                throw new InvalidOperationException("The service was not prepared properly to handle this operation.");

            var ciphers = await Task.Run(() => _encryptionService.Encrypt(inputs));
            var processedResults = ciphers.Select((cipher, index) => new Tuple<object, object>(inputs[index], cipher)).ToArray();

            return new EncryptionServiceResult(_encryptionService.GetEncryptionChoice(), OperationChoice.ENCRYPTION, _encryptionService.PrintKeys(), _encryptionService.GetMaxPlaintextSize(), processedResults);
        }

        public async Task<EncryptionServiceResult> Decrypt(CipherText[] ciphers)
        {
            if (!IsReady)
                throw new InvalidOperationException("The service was not prepared properly to handle this operation.");

            var plaintexts = await Task.Run(() => _encryptionService.Decrypt(ciphers));

            var processedResults = plaintexts.Select((plaintext, index) => new Tuple<object, object>(ciphers[index], plaintext)).ToArray();

            return new EncryptionServiceResult(_encryptionService.GetEncryptionChoice(), OperationChoice.DECRYPTION, _encryptionService.PrintKeys(), _encryptionService.GetMaxPlaintextSize(), processedResults);
        }

        public async Task<string> PrintKeys()
        {
            if (!IsReady)
                throw new InvalidOperationException("The service was not prepared properly to handle this operation.");

            var result = await Task.Run(() => _encryptionService.PrintKeys());
            return result;
        }
    }
}
