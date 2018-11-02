using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using Microsoft.CSharp;
using System.Numerics;

namespace protocol
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }
        private BigInteger AliceSecret(BigInteger n)
        {
            byte[] bytes = n.ToByteArray();
            Random rnd = new Random();
            BigInteger R;

            do
            {
                rnd.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F; 
                R = new BigInteger(bytes);
            } while (R >=n-1);
           
            //BigInteger s =rnd.Next(1, Convert.ToInt32(n) - 1);
            return R;
        }
        private BigInteger AlicePublic(BigInteger n,BigInteger s)
        {
            BigInteger v = (s * s) % n;
            return v;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BigInteger n;
            BigInteger p = Int32.Parse(pnumber.Text);
            BigInteger q = Int32.Parse(qnumber.Text);
            n = p * q;
            BigInteger s=AliceSecret(n);
            BigInteger v=AlicePublic(n, s);
            BigInteger r = R(n);
            BigInteger x = Certificate(n,r);
           int c= Sigh();

            BigInteger y = AnswerA(n, r, v, c);
            Verificate(n, y, x, v, c); 
        }
        private BigInteger Certificate(BigInteger n, BigInteger r)
        {
            BigInteger x = (r * r) % n;
            return x;
        }
        private BigInteger R(BigInteger n)
        {
            Random rnd = new Random();
            byte[] bytes = n.ToByteArray();
            BigInteger R;

            do
            {
                rnd.NextBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                R = new BigInteger(bytes);
            } while (R >= n - 1);

            //BigInteger s =rnd.Next(1, Convert.ToInt32(n) - 1);
            return R;
        }
        private BigInteger AnswerA(BigInteger n, BigInteger r, BigInteger s,int c)
        {
            BigInteger y = (r * Int32.Parse(BigInteger.Pow(s, c).ToString())) % n;
            return y;
        }
        private int Sigh()
        {
            Random rnd = new Random();
            int c = rnd.Next(0, 1);
            return c;

        }
        private void Verificate(BigInteger n, BigInteger y, BigInteger x, BigInteger v,int c)
        {
            BigInteger OneV = (y * y) % n;
            BigInteger TwoV = (x * Int32.Parse(BigInteger.Pow(v, c).ToString())) % n;
            if (OneV==TwoV)
            {
              result.Content = OneV + "=" + TwoV+ "\nCertificate is confirmed!";

            }
            else
            {
                result.Content = "Some problems is appeared...";
            }
           

        }
    }
}
