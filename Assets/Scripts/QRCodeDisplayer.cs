using UnityEngine;
using UnityEngine.UI;
using System;

public class QRCodeDisplayer : MonoBehaviour
{
    // Assign a UI RawImage (from your Canvas) in the Inspector.
    public RawImage qrDisplay;
    byte[] data;
    string data64;
    bool DoIt = false;
    bool DoItBase64 = false;

    private void Update()
    {
        if (DoIt)
        {
            DisplayQRCode(data);
            DoIt = false;
        }
        if (DoItBase64)
        {
            DisplayQRCodeFromBase64(data64);
            DoItBase64 = false;
        }
    }
    /// <summary>
    /// Displays the QR code from a PNG byte array.
    /// </summary>
    public void DisplayQRCode(byte[] qrCodeBytes)
    {
        Debug.Log("1");

        // Check if the byte array is valid
        if (qrCodeBytes == null || qrCodeBytes.Length == 0)
        {
            Debug.LogError("qrCodeBytes is null or empty.");
            return;
        }

        Debug.Log("qrCodeBytes length: " + qrCodeBytes.Length);

        Texture2D texture = new Texture2D(2, 2);

        try
        {
            bool success = texture.LoadImage(qrCodeBytes);
            Debug.Log("LoadImage returned: " + success);

            if (success)
            {
                qrDisplay.texture = texture;
                Debug.Log("2");
            }
            else
            {
                Debug.LogError("Failed to load QR code image from bytes.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception during LoadImage: " + ex);
        }
    }
    public void DoDisplayQRCode(byte[] data)
    {
        this.data = data;
        DoIt = true;
    }
    public void DoDisplayQRCode(string data)
    {
        this.data64 = data;
        DoItBase64 = true;
    }

    /// <summary>
    /// Converts a Base64 string to a byte array and displays the QR code.
    /// </summary>
    public void DisplayQRCodeFromBase64(string base64Data)
    {
        Debug.Log("from qr displayer: " + base64Data);
        base64Data = base64Data.Trim('[', ']', '"');
        Debug.Log("from qr displayer after trim: " + base64Data);
        try
        {
            byte[] qrCodeBytes = Convert.FromBase64String(base64Data);
            DisplayQRCode(qrCodeBytes);
        }
        catch (Exception ex)
        {
            Debug.LogError("Invalid Base64 string: " + ex.Message);
        }
    }
}
