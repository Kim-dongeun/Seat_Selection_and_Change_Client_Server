using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;

namespace 자리배치_서버
{
    internal class WbDB
    {
        private string connstring = @"Data Source=localhost;Initial Catalog=wb39;User ID=root;Password=1234";
        private MySqlConnection conn = null;

        #region 연결 및 종료
        public void Connect()
        {
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = connstring;
                conn.Open();
                Console.WriteLine("연결 성공");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisConnect()
        {
            conn.Close();
        }
        #endregion


        #region Insert, Update, Delete --> ExecuateNonQuery()
        //insert into Student values('1','1', 'dd');
        public void Insert(string id, string pw, string name)
        {
            try
            {
                string sql = $"insert into Student values('{id}','{pw}', '{name}');";
                ExecuateNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        //insert into Seat values('1','1');
        public void Insert_Seat(string id)
        {
            try
            {
                string sql = $"insert into Seat values(0,'{id}');";
                ExecuateNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public Student LogIn(string id, string pw)
        {
            //string sql = $"select stu.id, s.sit_num, stu.pw, stu.name from student stu, seat s where stu.id = s.id and stu.id = '{id}' and stu.pw = '{pw}';";
            string sql = $"select * from Seat join Student using(id) where id = '{id}' and pw = '{pw}';";

            using (MySqlCommand com = new MySqlCommand(sql, conn))
            {
                MySqlDataReader r = com.ExecuteReader();

                r.Read();

                string _id = r[0].ToString();
                string seat_num = r[1].ToString();
                string _pw = r[2].ToString();
                string name = r[3].ToString();
                Student stu = new Student(_id, seat_num, _pw, name);

                r.Close();
                return stu;
            }
        }

        //update Seat set sit_num = 16 where id = 1;
        public void Update_Seat(string id, string num)
        {
            try
            {
                string sql = $"update Seat set sit_num = '{num}' where id = '{id}';";
                ExecuateNonQuery(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //select count(*) from Student where id = 1;
        public int Count_Id(string id)
        {
            int count = -1;
            try
            {
                string sql = $"select count(*) from Student where id = '{id}';";
                using (MySqlCommand com = new MySqlCommand(sql, conn))
                {
                    MySqlDataReader r = com.ExecuteReader();
                    while (r.Read())
                    {
                      count = int.Parse(r[0].ToString());
                    }
                    r.Close();
                }
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return count;
            }
        }

        

        public List<Student> SelectAll_Seat()
        {
            List<Student> list = new List<Student>();
            string sql = $"select * from Seat join Student using(id);";

            using (MySqlCommand com = new MySqlCommand(sql, conn))
            {
                MySqlDataReader r = com.ExecuteReader();
                while (r.Read())
                {
                    string s_id = r[0].ToString();
                    string sit = r[1].ToString();
                    string pw = r[2].ToString();
                    string name = r[3].ToString();

                    Student stu = new Student(s_id, sit, pw, name);
                    list.Add(stu);
                }
                r.Close();
                return list;
            }
        }        

        private void ExecuateNonQuery(string sql)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = sql;      //쿼리문
                cmd.Connection = conn;     //연결객체
                cmd.CommandType = System.Data.CommandType.Text; //쿼리문 : default(Text)

                int value = cmd.ExecuteNonQuery();   //영향을 준 rowdata 개수
                if (value >= 1)
                    Console.WriteLine("성공");
                else
                    Console.WriteLine("실패");
            }  //cmd.Dispose()
        }

        #endregion
    }
}
