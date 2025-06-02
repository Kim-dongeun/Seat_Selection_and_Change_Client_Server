using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace 자리배치_서버
{
    [DataContract]
    public class Student
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string PW { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Sit_Num { get; set; }

        public Seat SeatFunc { get; set; }      //**

        public Student() { }

        public Student(string id, string sit_num, string pw, string name)
        {
            ID = id;
            PW = pw;
            Name = name;
            Sit_Num = sit_num;
        }
        public Student(string id, string pw, string name, Seat seatfunc)
        {
            ID = id;
            PW = pw;
            Name = name;    
            SeatFunc = seatfunc;
        }
        public Student(string id, string pw, string name)
        {
            ID = id;
            PW = pw;
            Name = name;
        }

        [OperationContract]
        public override string ToString()
        {
            return string.Format($"{ID} \t {PW} \t {Name} \t {Sit_Num}");
        }
    }
}
