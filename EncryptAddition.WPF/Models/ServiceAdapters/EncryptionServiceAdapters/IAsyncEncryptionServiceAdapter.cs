using EncryptAddition.Crypto;
using EncryptAddition.WPF.DataTypes;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.Models.ServiceAdapters
{
    public interface IAsyncEncryptionServiceAdapter
    {
        public bool IsReady { get; }
        Task PrepareService();
        Task<EncryptionServiceResult> Encrypt(BigInteger[] inputs);
        Task<EncryptionServiceResult> Decrypt(CipherText[] ciphers);
        Task<string> PrintKeys();
    }
}
