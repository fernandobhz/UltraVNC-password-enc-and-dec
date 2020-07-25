using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
        }

        private void button1_Click(object sender, EventArgs e) {
            MessageBox.Show(vncpass.EncryptPassword("xxxxxxx"));
            return;

            var a = "E3D7A8FC2C1FE8E677";
            var b = vncpass.DecryptPassword("E3D7A8FC2C1FE8E677");
            var c = vncpass.EncryptPassword(b);
            var d = vncpass.DecryptPassword(c);

            //MessageBox.Show(b);
            //MessageBox.Show(a + "\n" + c + "\n" + d);

            var m = vncpass.EncryptPassword("xxxx\0\0");
            var n = vncpass.EncryptPassword("xxxx");
            MessageBox.Show(m + "\n" + n);


            //MessageBox.Show(vncpass.EncryptPassword("794613"));
            //MessageBox.Show(vncpass.DecryptPassword(vncpass.EncryptPassword("794613")));
        }
    }

    class vncpass {
        public static string EncryptPassword(string plainPassword) {
            DES des = CreateDES();
            ICryptoTransform cryptoTransfrom = des.CreateEncryptor();

            plainPassword = plainPassword + "\x0000\x0000\x0000\x0000\x0000\x0000\x0000\x0000";
            plainPassword = plainPassword.Length > 8 ? plainPassword.Substring(0, 8) : plainPassword;
            byte[] data = Encoding.ASCII.GetBytes(plainPassword);
            byte[] encryptedBytes = cryptoTransfrom.TransformFinalBlock(data, 0, data.Length);

            return ByteArrayToHex(encryptedBytes) + "00";
        }

        public static string DecryptPassword(string encryptedPassword) {
            DES des = CreateDES();
            ICryptoTransform cryptoTransfrom = des.CreateDecryptor();
            byte[] data = HexToByteArray(encryptedPassword.Substring(0, encryptedPassword.Length - 2));
            byte[] decryptedBytes = cryptoTransfrom.TransformFinalBlock(data, 0, data.Length);

            return Encoding.ASCII.GetString(decryptedBytes);
        }

        private static DES CreateDES() {
            byte[] key = { 0xE8, 0x4A, 0xD6, 0x60, 0xC4, 0x72, 0x1A, 0xE0 };
            DES des = DES.Create();
            des.Key = key;
            des.IV = key;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            return des;
        }

        private static string ByteArrayToHex(byte[] bytes) {
            char[] c = new char[bytes.Length * 2];
            byte b;
            for (int i = 0;i < bytes.Length;i++) {
                b = (byte)(bytes[i] >> 4);
                c[i * 2] = (char)(b > 9 ? b + 0x37 : b + 0x30);
                b = (byte)(bytes[i] & 0xF);
                c[(i * 2) + 1] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }
            return new string(c);
        }

        private static byte[] HexToByteArray(string hex) {
            if (hex.Length % 2 == 1) {
                throw new Exception("The binary key cannot have an odd number of digits");
            }
            byte[] arr = new byte[hex.Length >> 1];
            for (int i = 0;i < (hex.Length >> 1);++i) {
                arr[i] = (byte)(((hex[i << 1] - (hex[i << 1] < 58 ? 48 : 55)) << 4) + (hex[(i << 1) + 1] - (hex[(i << 1) + 1] < 58 ? 48 : 55)));
            }
            return arr;
        }
    }

}
