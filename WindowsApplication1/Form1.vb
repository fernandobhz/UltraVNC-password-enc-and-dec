Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms

Namespace WindowsFormsApplication1
    Partial Public Class Form1
        Inherits Form

        Private Sub button1_Click(sender As Object, e As EventArgs)
            Dim a = "E3D7A8FC2C1FE8E677"
            Dim b = vncpass.DecryptPassword("E3D7A8FC2C1FE8E677")
            Dim c = vncpass.EncryptPassword(b)
            Dim d = vncpass.DecryptPassword(c)

            'MessageBox.Show(b);
            'MessageBox.Show(a + "\n" + c + "\n" + d);

            Dim m = vncpass.EncryptPassword("xxxx" & vbNullChar & vbNullChar)
            Dim n = vncpass.EncryptPassword("xxxx")
            MessageBox.Show(Convert.ToString(m & Convert.ToString(vbLf)) & n)


            'MessageBox.Show(vncpass.EncryptPassword("794613"));
            'MessageBox.Show(vncpass.DecryptPassword(vncpass.EncryptPassword("794613")));
        End Sub
    End Class

    Class vncpass
        Public Shared Function EncryptPassword(plainPassword As String) As String
            Dim des As DES = CreateDES()
            Dim cryptoTransfrom As ICryptoTransform = des.CreateEncryptor()

            plainPassword = plainPassword & Convert.ToString(vbNullChar & vbNullChar & vbNullChar & vbNullChar & vbNullChar & vbNullChar & vbNullChar & vbNullChar)
            plainPassword = If(plainPassword.Length > 8, plainPassword.Substring(0, 8), plainPassword)
            Dim data As Byte() = Encoding.ASCII.GetBytes(plainPassword)
            Dim encryptedBytes As Byte() = cryptoTransfrom.TransformFinalBlock(data, 0, data.Length)

            Return ByteArrayToHex(encryptedBytes) & Convert.ToString("00")
        End Function

        Public Shared Function DecryptPassword(encryptedPassword As String) As String
            Dim des As DES = CreateDES()
            Dim cryptoTransfrom As ICryptoTransform = des.CreateDecryptor()
            Dim data As Byte() = HexToByteArray(encryptedPassword.Substring(0, encryptedPassword.Length - 2))
            Dim decryptedBytes As Byte() = cryptoTransfrom.TransformFinalBlock(data, 0, data.Length)

            Return Encoding.ASCII.GetString(decryptedBytes)
        End Function

        Private Shared Function CreateDES() As DES
            Dim key As Byte() = {&HE8, &H4A, &HD6, &H60, &HC4, &H72, _
                &H1A, &HE0}
            Dim des__1 As DES = DES.Create()
            des__1.Key = key
            des__1.IV = key
            des__1.Mode = CipherMode.ECB
            des__1.Padding = PaddingMode.Zeros
            Return des__1
        End Function

        Private Shared Function ByteArrayToHex(bytes As Byte()) As String
            Dim c As Char() = New Char(bytes.Length * 2 - 1) {}
            Dim b As Byte
            For i As Integer = 0 To bytes.Length - 1
                b = CByte(bytes(i) >> 4)
                c(i * 2) = CChar(ChrW(If(b > 9, b + &H37, b + &H30)))
                b = CByte(bytes(i) And &HF)
                c((i * 2) + 1) = CChar(ChrW(If(b > 9, b + &H37, b + &H30)))
            Next
            Return New String(c)
        End Function

        Private Shared Function HexToByteArray(hex As String) As Byte()
            If hex.Length Mod 2 = 1 Then
                Throw New Exception("The binary key cannot have an odd number of digits")
            End If
            Dim arr As Byte() = New Byte((hex.Length >> 1) - 1) {}
            For i As Integer = 0 To (hex.Length >> 1) - 1
                arr(i) = CByte(((hex(i << 1) - (If(hex(i << 1) < 58, 48, 55))) << 4) + (hex((i << 1) + 1) - (If(hex((i << 1) + 1) < 58, 48, 55))))
            Next
            Return arr
        End Function
    End Class

End Namespace

