using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;

namespace Entities
{
    public class Security
    {
        private string privateKey;
        private string publicKey;
        private static UnicodeEncoding _encoder = new UnicodeEncoding();

        public Security()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            privateKey = rsa.ToXmlString(true);
            publicKey = rsa.ToXmlString(false);
        }

        private string Decrypt(string data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            var dataArray = data.Split(new char[] { ',' });
            var dataByte = new byte[dataArray.Length];

            for (int i = 0; i < dataArray.Length; i++)
            {
                dataByte[i] = Convert.ToByte(dataArray[i]);
            }

            rsa.FromXmlString(privateKey);
            var decryptedByte = rsa.Decrypt(dataByte, false);
            return _encoder.GetString(decryptedByte);
        }

        private string Encrypt(string data)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            byte[] dataToEncrypt = _encoder.GetBytes(data);
            byte[] encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            int length = encryptedByteArray.Count();
            int item = 0;
            StringBuilder sb = new StringBuilder();
            foreach (byte x in encryptedByteArray)
            {
                item++;
                sb.Append(x);

                if (item < length)
                    sb.Append(",");
            }

            return sb.ToString();
        }

        public string GetPublicKey()
        {
            return publicKey;
        }

        public string Encryptation(string filePath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            XmlNode node = xmlDocument.DocumentElement.SelectSingleNode("/file/type");

            if(node.InnerText == "decrypt")
            {
                try
                {
                    return Decrypt(
                        xmlDocument.DocumentElement.SelectSingleNode("/file/text").InnerText
                    ) ;
                }
                catch(Exception error)
                {
                    throw new Exception(error.Message);
                }
            }

            try
            {
                return Encrypt(
                        xmlDocument.DocumentElement.SelectSingleNode("/file/text").InnerText
                    );
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}
