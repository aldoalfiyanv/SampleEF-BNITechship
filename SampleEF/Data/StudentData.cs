using Microsoft.EntityFrameworkCore;
using SampleEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleEF.Data
{
    public class StudentData : IStudent
    {
        private SampleApiDbContext _db;
        public StudentData(SampleApiDbContext db)
        {
            _db = db;
        }

        public async Task Delete(string id)
        {
            var result = await GetById(id);
            if (result != null)
            {
                try
                {
                    _db.Students.Remove(result);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    throw new Exception($"DbError: {dbEx.Message}");
                }
                catch(Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            /*var results = await _db.Students.OrderBy(s => s.FirstMidName)
                .ToListAsync();*/
            var results = await (from s in _db.Students
                                 orderby s.FirstMidName
                                 select s).AsNoTracking().ToListAsync();
            return results;
        }

        public async Task<Student> GetById(string id)
        {
            /*var result = await _db.Students.Where(s => s.Id == Convert.ToInt32(id))
                .FirstOrDefaultAsync();*/
            var result = await (from s in _db.Students
                                where s.Id == Convert.ToInt32(id)
                                select s).FirstOrDefaultAsync();

            var enrollments = await (from e in _db.Enrollments.Include(e=>e.Course)
                                     where e.StudentId == Convert.ToInt32(id)
                                     select e).AsNoTracking().ToListAsync();

            result.Enrollments = enrollments;

            return result;
        }

        public async Task Insert(Student obj)
        {
            try
            {
                _db.Students.Add(obj);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Db Error: {dbEx.Message}");
            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task Update(string id, Student obj)
        {
            try
            {
                var result = await GetById(id);
                if (result != null)
                {
                    //_db.Update(obj);
                    result.FirstMidName = obj.FirstMidName;
                    result.LastName = obj.LastName;
                    result.EnrollmentDate = obj.EnrollmentDate;
                    await _db.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Data id:{id} tidak ditemukan");
                }
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"DbError: {dbEx.Message}");
            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
