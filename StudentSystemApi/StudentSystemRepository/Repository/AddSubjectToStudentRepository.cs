using Microsoft.EntityFrameworkCore;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystemRepository.Repository
{
    public class AddSubjectToStudentRepository : IAddSubjectToStudentRepository
    {
        private readonly DataContext _context;

        public AddSubjectToStudentRepository(DataContext context)
        {
            _context = context;
        }



        public async Task<string> AddSubjectToStudentAsync(string studentId, int subjectId)
        {
            var subject = await _context.Subjects.FindAsync(subjectId);
            if (subject == null)
                return "Subject not found.";

            var student = await _context.Users.FindAsync(studentId);
            if (student == null)
                return "Student not found.";

            var existingRelation = await _context.StudentSubjects
                .FirstOrDefaultAsync(ss => ss.StudentId == studentId && ss.SubjectId == subjectId);

            if (existingRelation != null)
                return "Student already has the subject.";

            _context.StudentSubjects.Add(new StudentSubject
            {
                StudentId = studentId,
                SubjectId = subjectId
            });

            await _context.SaveChangesAsync();

            return "Subject added to the student successfully.";
        }
        public async Task<IEnumerable<Subject>> GetSubjectsForStudentAsync(string studentId)
        {
            return await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .Select(ss => ss.Subject)
                .ToListAsync();
        }
    }
}
