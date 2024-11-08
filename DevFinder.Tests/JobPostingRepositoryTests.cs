using DevFinder.Data;
using DevFinder.Models;
using DevFinder.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFinder.Tests
{
    public class JobPostingRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("JobPostingDb")
                .Options;
        }

        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddJobPosting()
        {
            // db context
            var db = CreateDbContext();
            // JobPosting Repository
            var repository = new JobPostingRepository(db);
            //job posing to add
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                PostedDate = DateTime.Now,
                Company = "Test Company inc",
                Location = "Test Location",
                UserId = "TestUserId"
            };
            //execute
            await repository.AddAsync(jobPosting);
            // check if we have result
            var result = db.JobPostings.Find(jobPosting.Id);
            //assert
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnJobPosting()
        {
            // db context
            var db = CreateDbContext();
            // JobPosting Repository
            var repository = new JobPostingRepository(db);
            //job posing to add
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                PostedDate = DateTime.Now,
                Company = "Test Company inc",
                Location = "Test Location",
                UserId = "TestUserId"
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync(); 

            var result = await repository.GetByIdAsync(jobPosting.Id);

            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
        {
            var db = CreateDbContext();
            
            var repository = new JobPostingRepository(db);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAsync(65675));
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnAllJobPosting()
        {
            
            var db = CreateDbContext();
            
            var repository = new JobPostingRepository(db);
            
            var jobPosting1 = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                PostedDate = DateTime.Now,
                Company = "Test Company inc",
                Location = "Test Location",
                UserId = "TestUserId"
            };

            var jobPosting2 = new JobPosting
            {
                Title = "Test Title 2",
                Description = "Test Desc 2",
                PostedDate = DateTime.Now,
                Company = "Test Company inc 2",
                Location = "Test Location 2",
                UserId = "TestUserId2"
            };


            await db.JobPostings.AddRangeAsync(jobPosting1,jobPosting2);
            await db.SaveChangesAsync();

            var result = await repository.GetAllAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                PostedDate = DateTime.Now,
                Company = "Test Company inc",
                Location = "Test Location",
                UserId = "TestUserId"
            };
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();
            jobPosting.Description = "Updated Description";
            await repository.UpdateAsync(jobPosting);

            var result = db.JobPostings.Find(jobPosting.Id);
            Assert.NotNull(result);
            Assert.Equal("Updated Description", result.Description);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                PostedDate = DateTime.Now,
                Company = "Test Company inc",
                Location = "Test Location",
                UserId = "TestUserId"
            };
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            await repository.DeleteAsync(jobPosting.Id);
            var result = db.JobPostings.Find(jobPosting.Id);

            Assert.Null(result);
        }
    }
}
