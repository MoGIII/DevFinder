﻿using DevFinder.Data;
using DevFinder.Models;
using Microsoft.EntityFrameworkCore;

namespace DevFinder.Repositories
{
    public class JobPostingRepository : IRepository<JobPosting>
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context) { 
            _context = context;
        }
        public async Task AddAsync(JobPosting entity)
        {
            await _context.JobPostings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobPosing = await _context.JobPostings.FindAsync(id);
            if (jobPosing == null)
            {
                throw new KeyNotFoundException();
            }
            _context.JobPostings.Remove(jobPosing);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
           return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting> GetByIdAsync(int id)
        {
            var jobPosing = await _context.JobPostings.FindAsync(id);
            if (jobPosing == null)
            {
                throw new KeyNotFoundException();
            }
            return jobPosing;
        }

        public async Task UpdateAsync(JobPosting entity)
        {
            _context.JobPostings.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
