using Microsoft.EntityFrameworkCore;
using SampleEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleEF.Data
{
    public class EnrollmentData : IEnrollment
    {
        private SampleApiDbContext _db;
        public EnrollmentData(SampleApiDbContext db)
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
                    _db.Enrollments.Remove(result);
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

        public async Task<IEnumerable<Enrollment>> GetAll()
        {
            var results = await _db.Enrollments.Include(e => e.Student).Include(e=>e.Course)
                .OrderBy(e => e.CourseId).AsNoTracking().ToListAsync();
            return results;
        }

        public async Task<Enrollment> GetById(string id)
        {
            /*var result = await _db.Students.Where(s => s.Id == Convert.ToInt32(id))
                .FirstOrDefaultAsync();*/
            var result = await (from s in _db.Enrollments
                                where s.EnrollmentId == Convert.ToInt32(id)
                                select s).FirstOrDefaultAsync();

            return result;
        }

        public async Task Insert(Enrollment obj)
        {
            try
            {
                _db.Enrollments.Add(obj);
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

        public async Task Update(string id, Enrollment obj)
        {
            try
            {
                var result = await GetById(id);
                if (result != null)
                {
                    //_db.Update(obj);
                    result.Grade = obj.Grade;
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
