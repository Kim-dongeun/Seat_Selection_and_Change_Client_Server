using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.ServiceModel;

namespace 자리배치_서버
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IStudentCallback))]
    public interface IStudent
    {
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        bool AddMember(Student student);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        Student LogIn(string id, string pw);    // 회원정보 리턴

        //[OperationContract]
        //List<Student> GetStudentList();

        //[OperationContract]
        //void SelectAll();

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        bool Choose_Seat(string id, string seat);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        List<Student> PrintAll();
    }

    public interface IStudentCallback
    {
        [OperationContract(IsOneWay = true)]
        void Join(string id, string name);

        [OperationContract(IsOneWay = true)]
        void Update(string sit_num, string name);
    }
}
