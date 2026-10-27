using AdoNetStudentApp.Models;

namespace AdoNetStudentApp.Repositories;

public interface IStudentRepository
{
    int Add(Student student);

    List<Student> GetAll();

    Student GetById(int id);
    bool Update(Student student);
}

