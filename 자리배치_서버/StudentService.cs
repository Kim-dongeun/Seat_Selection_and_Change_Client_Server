using Org.BouncyCastle.Asn1.Misc;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace 자리배치_서버
{
    public delegate void Seat(string type, string id, string name, string sit_num);

    internal class StudentService : IStudent
    {
        private static object syncObj = new object();

        private static List<Student> students = new List<Student>();
        private WbDB db = new WbDB();
        private IStudentCallback callback = null;
        //private Student student = null;


        public StudentService()
        {
            Console.WriteLine("서비스객체 생성");
            //PrintAll();
        }

        #region IStudent 메서드
        public bool AddMember(Student student)
        {
            lock (syncObj)
            {
                db.Connect();

                Student temp = students.Find(ele => ele.ID == student.ID);
                if (temp != null)
                    return false;

                Student stu = new Student(student.ID, student.PW, student.Name);
                db.Insert(stu.ID, stu.PW, stu.Name);
                db.Insert_Seat(stu.ID);
                students.Add(stu);

                db.DisConnect();
                return true;
            }
        }

        public Student LogIn(string id, string pw)
        {
            lock (syncObj)
            {
                db.Connect();

                try
                {
                    callback = OperationContext.Current.GetCallbackChannel<IStudentCallback>();
                    Student test = db.LogIn(id, pw);
                    Student stu = new Student(test.ID, test.PW, test.Name, UserHandler);
                    students.Add(stu);

                    BroadcastMessage("Join", test.ID, test.Name, "");

                    return stu;
                }
                catch
                {
                    //return false;
                    return null;
                }
                finally
                {
                    db.DisConnect();
                }
            }
        }

        public List<Student> PrintAll()
        {
            lock (syncObj)
            {
                db.Connect();


                List<Student> stus = db.SelectAll_Seat();
                foreach (Student student in stus)
                {
                    students.Add(student);
                }

                db.DisConnect();
                return stus;
            }
        }

        public bool Choose_Seat(string id, string seat)
        {
            lock (syncObj)
            {
                db.Connect();

                Student temp = students.Find(ele => ele.ID == id);
                if (temp == null)
                    return false;

                if (db.Count_Id(id) == 1)
                {
                    db.Update_Seat(id, seat);

                    BroadcastMessage("Update", id, temp.Name, seat);

                    return true;
                }
                else
                {
                    MessageBox.Show("없는 id입니다.");
                    db.DisConnect();
                    return false;
                }
            }
        }
        #endregion


        private void BroadcastMessage(string msgType, string id, string name, string sit_num)
        {
            List<Student> studentsCopy;
            lock (syncObj)
            {
                studentsCopy = new List<Student>(students);
            }

            Task.Run(() => Tt(studentsCopy, msgType, id, name, sit_num));

        }

        private void Tt(List<Student> studentsCopy, string msgType, string id, string name, string sit_num)
        {
            foreach (Student stu in studentsCopy)
            {
                try
                {
                    Seat handler = stu.SeatFunc;

                    if (handler != null)
                        handler.BeginInvoke(msgType, id, name, sit_num, new AsyncCallback(EndAsync), null);
                }
                catch
                { 
                }
            }
        }


        //콜백 메서드
        private void UserHandler(string msgType, string id, string name, string sit_num)
        {
            try
            {
                //클라이언트에게 보내기
                switch (msgType)
                {
                    case "Join": callback.Join(id, name); break;
                    case "Update": callback.Update(sit_num, name); break;
                }
            }
            catch//에러가 발생했을 경우
            {
                Console.WriteLine("{0} 에러", id);
                //Leave(idx, name);
            }
        }

        private void EndAsync(IAsyncResult ar)
        {
            Seat d = null;
            try
            {
                System.Runtime.Remoting.Messaging.AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((Seat)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                Console.WriteLine("비동기 결과 오류");
            }
        }

    }
}
