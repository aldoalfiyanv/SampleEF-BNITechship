using Microsoft.EntityFrameworkCore;
using SampleEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleEF.Data
{
    public class CourseData : ICourse
    {
        private SampleApiDbContext _db;
        public CourseData(SampleApiDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            var results = await (from s in _db.Courses
                                 orderby s.Title
                                 select s).AsNoTracking().ToListAsync();
            return results;
        }

        public async Task Delete(string id)
        {
            var result = await GetById(id);
            if (result != null)
            {
                try
                {
                    _db.Courses.Remove(result);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateException dbEx)
                {
                    throw new Exception($"DbError: {dbEx.Message}");
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        public async Task<Course> GetById(string id)
        {
            var result = await (from s in _db.Courses
                                where s.CourseId == Convert.ToInt32(id)
                                select s).FirstOrDefaultAsync();

            var enrollments = await (from e in _db.Enrollments.Include(e => e.Course)
                                     where e.StudentId == Convert.ToInt32(id)
                                     select e).AsNoTracking().ToListAsync();

            result.Enrollments = enrollments;

            return result;
        }

        public async Task Insert(Course obj)
        {
            try
            {
                _db.Courses.Add(obj);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Db Error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task Update(string id, Course obj)
        {
            try
            {
                var result = await GetById(id);
                if (result != null)
                {
                    //_db.Update(obj);
                    result.Title = obj.Title;
                    result.Credits = obj.Credits;
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
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
