using CommunityToolkit.Mvvm.ComponentModel;
using Crossdyne.Security.Abstractions;
using Crossdyne.Security.Configuration;
using EnigmaVault.Desktop.Constants;
using EnigmaVault.Desktop.Enums;
using EnigmaVault.Desktop.Models.Vaults;
using EnigmaVault.Desktop.Services;
using EnigmaVault.Desktop.ViewModels.Features.Credentials.Vault;

namespace EnigmaVault.Desktop.ViewModels.Features.Credentials.Items
{
    public sealed partial class CreditCardViewModel(CredentialsVaultViewModel model) : CredentialItemBaseViewModel(model, VaultType.CreditCard)
    {
        #region Расшифрованные данные

        // Номер карты (лучше хранить строкой, чтобы сохранять пробелы: 0000 0000 0000 0000)
        [ObservableProperty]
        private string? _cardNumber;

        // Имя держателя (как на карте: IVAN IVANOV)
        [ObservableProperty]
        private string? _cardHolder;

        // Срок действия. Можно использовать DateTime, но для карт удобнее строка "MM/YY"
        // т.к. там нет понятия "дня", и так проще вводить.
        [ObservableProperty]
        private string? _expiryDate;

        // CVV / CVC код (3 цифры сзади)
        [ObservableProperty]
        private string? _cvvCode;

        // ПИН-код (опционально, но полезно помнить)
        [ObservableProperty]
        private string? _pinCode;

        // Название банка (для красоты или поиска)
        [ObservableProperty]
        private string? _bankName;

        // Тип карты (Visa, MasterCard, Mir). Можно сделать Enum или просто строку.
        [ObservableProperty]
        private string? _paymentSystem;

        #endregion

        public override void Decrypt(string encryptedOverView, string encryptedDetails, ICryptoService secureData, IUserContext context)
        {
            OverviewPayload overview;
            CreditCard details;

            try
            {
                overview = secureData.DecryptData<OverviewPayload>(encryptedOverView, context.Dek)!;
                details = secureData.DecryptData<CreditCard>(encryptedDetails, context.Dek)!;
            }
            catch (Exception)
            {
                return;
            }

            if (details is null)
                return;

            ServiceName = overview?.ServiceName!;
            Url = overview?.Url;
            Note = overview?.Note;
            SvgCode = overview?.SvgIcon;

            CardNumber = details.CardNumber;
            CardHolder = details.CardHolder;
            ExpiryDate = details.ExpireDate;
            CvvCode = details.CvvCode;
            PinCode = details.PinCode;
            BankName = details.BankName;
            PaymentSystem = details.PaymentSystem;
        }

        public override (string EncryptedOverView, string EncryptedDetails, CryptoVersion CryptoVersion) Encrypt(ICryptoService secureData, IUserContext context)
        {
            var overView = new OverviewPayload(ServiceName, Url!, Note, SvgCode);
            var details = new CreditCard(CardNumber, CardHolder, ExpiryDate, CvvCode, PinCode, BankName, PaymentSystem);

            var encryptedDetails = secureData.EncryptData(details, context.Dek, CryptoConstants.CurrentCryptoVersion);
            var encryptedOverView = secureData.EncryptData(overView, context.Dek);

            return (encryptedOverView, encryptedDetails, CryptoConstants.CurrentCryptoVersion);
        }

        public override void Clear()
        {
            ServiceName = string.Empty;
            Url = string.Empty;
            SvgCode = string.Empty;

            CardNumber = string.Empty;
            CardHolder = string.Empty;
            ExpiryDate = string.Empty;
            CvvCode = string.Empty;
            PinCode = string.Empty;
            BankName = string.Empty;
            PaymentSystem = string.Empty;
        }
    }
}