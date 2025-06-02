using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using 자리배치_클라이언트.ServiceReference1;

namespace 자리배치_클라이언트
{
    public partial class AddMember : Form
    {
        public Student student { get; private set; }

        public AddMember()
        {
            InitializeComponent();
        }

        //회원가입버튼
        private void button1_Click(object sender, EventArgs e)
        {
            student = new Student() {
                ID = textBox1.Text,
                PW = textBox2.Text,
                Name = textBox3.Text
            };

            Close();

            //if (s.AddMember(stu) == true)
            //{
            //    MessageBox.Show("저장 완료");
            //    this.Hide();
            //}                
            //else
            //    MessageBox.Show("저장 실패");
        }

    }
}
