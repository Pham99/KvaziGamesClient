using UnityEngine;
using UnityEngine.UI;
using System;

public class QRCodeDisplayer : MonoBehaviour
{
    // Assign a UI RawImage (from your Canvas) in the Inspector.
    [SerializeField]
    private RawImage qrDisplay;

    /// <summary>
    /// Displays the QR code from a PNG byte array.
    /// </summary>
    public void DisplayQRCode(byte[] qrCodeBytes)
    {
        // Check if the byte array is valid
        if (qrCodeBytes == null || qrCodeBytes.Length == 0)
        {
            Debug.LogError("qrCodeBytes is null or empty.");
            return;
        }


        Texture2D texture = new Texture2D(2, 2);

        try
        {
            bool success = texture.LoadImage(qrCodeBytes);
            Debug.Log("LoadImage returned: " + success);

            if (success)
            {
                qrDisplay.texture = texture;
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
