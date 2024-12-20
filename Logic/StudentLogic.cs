using Azure.Core;
using CommonTypes.Request.General;
using Datamodels.Models;
using Datamodels.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CommonTypes.Language.LanguageSupport;
using static CommonTypes.Request.Models.UserRequests;

namespace Datamodels.Logic
{
    public class StudentLogic : BaseLogic
    {
        private static readonly LanguageObject message_exists = new LanguageObject("The students already exists", "El estudiante ya existe");

        public StudentLogic(Context context) : base(context) { }

        public void ReadTxt(string uri)
        {
            string[] lines = File.ReadAllLines(uri);
            foreach (var line in lines)
            {
                string[] fields = line.Split(',');
                Student student = new Student
                {

                    Name = fields[0],
                    Sex = fields[1],
                    Age = Convert.ToInt32(fields[2]),
                    Specialty = fields[3],
                    Year = Convert.ToInt32(fields[4]),


                };
                context.Students.Add(student);
                context.SaveChanges();
            }
        }

        public bool Exists(Student_Create request)
        {

            return context.Students.Any(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim());
        }

        public bool ExistsAnother(Student_Update request)
        {
            return context.Students.Any(c => c.Name.ToLower().Trim() == request.Name.ToLower().Trim());

        }

        public Student Find(int id) => context.Students.FirstOrDefault(c => c.Id == id);

        public bool UpdateStudent(Student_Update request, out LanguageObject message)
        {
            if (request.IsValid(out message))
            {
                Student student = Find(request.Id);
                if (student != null)
                {
                    if (ExistsAnother(request))
                    {
                        message = message_exists;
                        return false;
                    }
                    student.Name = request.Name;
                    student.Age = request.Age;
                    student.Sex = request.Sex;
                    student.Specialty = request.Specialty;
                    student.Year = request.Year;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteStudent(IEnumerable<int> ids)
        {
            if (ids != null)
            {
                Student student;
                foreach (int id in ids)
                    if ((student = Find(id)) != null)
                        context.Students.Remove(student);
                    else
                        return false;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<object> GetStudents() => context.Students.OrderBy(c => c.Id).Select(c => new
        {
            Name = c.Name,
            Age = c.Age,
            Sex= c.Sex,
            Specialty = c.Specialty,
            Year = c.Year,
            
        }).ToList();
    }
}

