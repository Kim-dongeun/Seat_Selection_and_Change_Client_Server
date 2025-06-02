using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Forms;
using 자리배치_클라이언트.ServiceReference1;


namespace 자리배치_클라이언트
{
    public partial class Form1 : Form, IStudentCallback
    {
        private StudentClient s = null;
        private Student current_student = null;

        public Form1()
        {
            InitializeComponent();
            s = new StudentClient(new InstanceContext(this));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrintAll();           
        }

        #region 버튼 핸들러 (client => Service)
        //회원등록 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            AddMember addmember = new AddMember();
            addmember.ShowDialog();
            current_student = addmember.student;

            s.AddMember(current_student);
            MessageBox.Show("회원가입 성공");

            return;
        }

        //로그인 버튼
        private void button2_Click(object sender, EventArgs e)
        {
            /* 클라에서 Login해결할때
            //Student[] students = s.PrintAll();
            //foreach (Student stu in students)
            //{
            //    if (textBox37.Text == stu.ID && textBox38.Text == stu.PW)
            //    {
            //        this.Text = stu.Name;
            //        current_student = stu;
            //        MessageBox.Show("로그인 성공");
            //        return;
            //    }
            //}
            //MessageBox.Show("없는 ID입니다 다시 입력해주세요");
            */

            

            //서버에 Login 함수 쓸떄.
            try
            {
                Student stu = s.LogIn(textBox37.Text, textBox38.Text);
                this.Text = stu.Name;
                current_student = stu;
                MessageBox.Show("로그인 성공");
                return;                
            }
            catch
            {
                MessageBox.Show("없는 ID입니다 다시 입력해주세요");
            }
        }

        //자리배치 버튼   //textBox39
        private void button3_Click(object sender, EventArgs e)
        {
            if (s.Choose_Seat(current_student.ID, textBox39.Text) == true)
            {
                //Clear();
                //PrintAll();
            }
            else
            {
                MessageBox.Show("Choose_Seat 오류");
            }

            textBox39.Text = string.Empty;
        }

        private void PrintAll()
        {
            Student[] students = s.PrintAll();
            for (int i = 1; i < 37; i++)
            {
                var textBox = this.Controls.Find($"textBox{i}", true).FirstOrDefault() as TextBox;                

                textBox.BackColor = Color.LightBlue;
                textBox.Text = i.ToString();
                foreach (Student s in students)
                {
                    if (textBox != null)
                    {
                        if (s.Sit_Num == i.ToString())
                        {
                            //textBox.Clear();
                            textBox.BackColor = Color.LightGreen;
                            textBox.Text = $"{s.Sit_Num}:{s.Name}";
                        }

                    }
                }
            }
        }

        private void Clear()
        {
            Student[] students = s.PrintAll();
            for (int i = 1; i < 37; i++)
            {
                var textBox = this.Controls.Find($"textBox{i}", true).FirstOrDefault() as TextBox;

                foreach (Student s in students)
                {
                    if (textBox == null)
                    {
                        textBox.Clear();
                        textBox.BackColor = Color.LightBlue;
                        textBox.Text = $"{i}";
                    }
                }
            }
        }

        //좌석업데이트 버튼
        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
            PrintAll();
        }
        #endregion

        #region IChatCallback 인터페이스 함수 생성
        public void Join(string id, string name)
        {
            textBox40.AppendText($"[join] {id}, {name} 로그인 - {DateTime.Now}");
            textBox40.AppendText("\r\n");
        }

        public void Update(string sit_num, string name)
        {
            textBox40.AppendText($"[SetSeat] {sit_num}, {name} 자리변경 - {DateTime.Now}");
            textBox40.AppendText("\r\n");

            Clear();
            PrintAll();
        }
        #endregion

        
    }
}


//Student[] students = s.PrintAll();
//for (int i = 1; i < 37; i++)
//{
//    var textBox = this.Controls.Find($"textBox{i}", true).FirstOrDefault() as TextBox;

//    foreach (Student s in students)
//    {
//        if (textBox == null)
//        {
//            textBox.Clear();
//            textBox.BackColor = Color.LightBlue;
//            textBox.Text = $"{i}";
//        }
//    }
//}