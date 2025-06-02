
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace 자리배치_서버
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Address
            Uri uri = new Uri(ConfigurationManager.AppSettings["addr"]);
            //Contract-> Setting
            //Binding -> App.Config
            ServiceHost host = new ServiceHost(typeof(StudentService), uri);

            //오픈
            host.Open();
            Console.WriteLine("자리배치 서비스를 시작합니다. {0}", uri.ToString());
            Console.WriteLine("http://127.0.0.1:9000/GetService");
            Console.WriteLine("멈추시려면 엔터를 눌러주세요..");
            Console.ReadLine();
            //서비스
            host.Abort();
            host.Close();
        }

    }
}
