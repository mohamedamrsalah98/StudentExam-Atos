using Microsoft.EntityFrameworkCore;
using StudentSystemRepository.Interfaces;
using StudentSytemData.Data;
using StudentSytemData.Models;


namespace StudentSystemRepository.Repository
{

        public class SubjectRepository : ISubjectRepository
        {
            private readonly DataContext _context;

            public SubjectRepository(DataContext context)
            {
                _context = context;
            }
            public async Task<ICollection<Subject>> GetAllSubjectsAsync()
            {
                return await _context.Subjects.ToListAsync();
            }


            public async Task<bool> CreateSubjectAsync(Subject subject)
            {
                _context.Subjects.Add(subject);
                await _context.SaveChangesAsync();
                return true;
            }
            public async Task<bool> SubjectExistsAsync(string subjectName)
            {
                return await _context.Subjects.AnyAsync(s => s.SubjectName.Trim().ToUpper() == subjectName.Trim().ToUpper());
            }
            public async Task<bool> SubjectExistsAsync(int id)
            {
                return await _context.Subjects.AnyAsync(s => s.Id == id);
            }

            public async Task<Subject> GetSubjectByIdAsync(int subjectId)
            {
                return await _context.Subjects.FindAsync(subjectId);
            }

            public async Task<bool> UpdateSubjectAsync(Subject subject)
            {
                try
                {
                    _context.Entry(subject).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        public async Task<Subject> GetSubjectByNameAsync(string subjectName)
        {
            return await _context.Subjects.SingleOrDefaultAsync(s => s.SubjectName == subjectName);
        }

        public async Task<Subject> GetSubjectAsync(int id)
            {
                return await _context.Subjects.FindAsync(id);
            }

            public async Task<bool> DeleteSubjectAsync(int id)
            {
                var subject = await _context.Subjects.FindAsync(id);
                if (subject == null)
                {
                    return false;
                }

                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    
}
