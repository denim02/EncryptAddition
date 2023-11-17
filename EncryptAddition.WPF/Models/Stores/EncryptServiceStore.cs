using EncryptAddition.Crypto;
using EncryptAddition.WPF.Models.ServiceAdapters;

namespace EncryptAddition.WPF.Models.Stores
{
    public sealed class EncryptServiceStore
    {
        private static EncryptServiceStore _instance;
        public static bool IsInstantiated => _instance != null;

        private static string? _serializedKeyPair;
        private static int? _bitLength;
        private static EncryptionChoice _choice;

        public IAsyncEncryptionServiceAdapter AsyncEncryptionAdapter { get; private set; }

        private EncryptServiceStore(EncryptionChoice algorithmChoice, string serializedKeyPair)
        {
            _serializedKeyPair = serializedKeyPair;
            _bitLength = null;
            _choice = algorithmChoice;
            AsyncEncryptionAdapter = new AsyncEncryptionServiceAdapter(algorithmChoice, serializedKeyPair);
        }

        private EncryptServiceStore(EncryptionChoice algorithmChoice, int bitLength)
        {
            _serializedKeyPair = null;
            _bitLength = bitLength;
            _choice = algorithmChoice;
            AsyncEncryptionAdapter = new AsyncEncryptionServiceAdapter(algorithmChoice, bitLength);
        }

        public static EncryptServiceStore GetInstance(EncryptionChoice algorithmChoice, string serializedKeyPair)
        {
            if (_instance == null || algorithmChoice != _choice || _serializedKeyPair != serializedKeyPair)
            {
                _instance = new EncryptServiceStore(algorithmChoice, serializedKeyPair);
                return _instance;
            }

            return _instance;
        }

        public static EncryptServiceStore GetInstance(EncryptionChoice algorithmChoice, int bitLength)
        {
            if (_instance == null || algorithmChoice != _choice || _bitLength != bitLength)
            {
                _instance = new EncryptServiceStore(algorithmChoice, bitLength);
                return _instance;
            }

            return _instance;
        }
    }
}
