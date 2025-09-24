using System;
using System.Globalization;
using System.Text;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace FrostyEditor.Converters;

public class HexConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return string.Empty;
        }

        if (value is not byte[] bytes)
        {
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        StringBuilder sb = new(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            sb.Append($"{b:X2}");
        }
        return sb.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return Array.Empty<byte>();
        }

        if (value is not string hex)
        {
            return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
        }

        if (string.IsNullOrWhiteSpace(hex))
        {
            return Array.Empty<byte>();
        }

        if (hex.Length % 2 != 0)
        {
            return new BindingNotification(new InvalidCastException(), BindingErrorType.DataValidationError);
        }

        int byteCount = hex.Length / 2;
        byte[] bytes = new byte[byteCount];
        for (int i = 0; i < byteCount; i++)
        {
            string hexPair = hex.Substring(i * 2, 2);
            bytes[i] = byte.Parse(hexPair, NumberStyles.HexNumber, culture);
        }
        return bytes;
    }
}