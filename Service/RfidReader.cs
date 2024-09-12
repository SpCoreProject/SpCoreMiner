using System;
using System.Device.Gpio;
using System.Device.Spi;
using Iot.Device.Card.Mifare;
using Iot.Device.Mfrc522;

public class RfidServicex
{
    private readonly MfRc522 _mfrc522;

    public RfidServicex()
    {
        try
        {
            // راه‌اندازی دستگاه RFID (مانند کدی که قبلاً نوشته شد)
            var gpioController = new GpioController();
            var spi = SpiDevice.Create(new SpiConnectionSettings(0, 1) { ClockFrequency = 5_000_000 });
            _mfrc522 = new MfRc522(spi, 4, gpioController, false);

        }
        catch (Exception ex)
        {
            var msg = ex.Message;
        }
    }

    public byte[]? ReadCardUid()
    {
        if (_mfrc522.ListenToCardIso14443TypeA(out var card, TimeSpan.FromSeconds(2)))
        {
            return card.NfcId;
        }
        return null;
    }

    public bool WriteToCard(byte[] data)
    {
        var cardUid = ReadCardUid();
        if (cardUid == null)
        {
            return false;
        }

        var mifare = new MifareCard(_mfrc522, 0)
        {
            SerialNumber = cardUid,
            KeyA = MifareCard.DefaultKeyA.ToArray(),
            KeyB = MifareCard.DefaultKeyB.ToArray(),
            BlockNumber = 0,
            Command = MifareCardCommand.AuthenticationB
        };

        mifare.RunMifareCardCommand();
        mifare.Command = MifareCardCommand.Write16Bytes;
        mifare.Data = data;
        var result = mifare.RunMifareCardCommand();

        return result >= 0;
    }
}
