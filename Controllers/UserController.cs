using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        public enum HttpStatusCode
        {
            Moved = 301,
            OK = 200,
            Redirect = 302
        }
        private MySQLDBContext _dbContext;
        public UserController(MySQLDBContext context)
        {
            _dbContext = context;
        }

        //routes starts
        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<User>>> Get()
        {
            var list = await _dbContext.User.Select(
                s => new User
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Username = s.Username,
                    Password = s.Password,
                    EnrollmentDate = s.EnrollmentDate
                }
            ).ToListAsync();
            if (list.Count < 0) return (list);
            else return list;
        }

        //getOne
        [HttpGet("GetUserById/{Id}")]
        public async Task<ActionResult<User>> GetUserById(int Id)
        {
            User UserData = await _dbContext.User.Select(
                    s => new User
                    {
                        Id = s.Id,
                        FirstName = s.FirstName,
                        LastName = s.LastName,
                        Username = s.Username,
                        Password = s.Password,
                        EnrollmentDate = s.EnrollmentDate
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);
            return UserData;
        }

        //Insert
        [HttpPost("InsertOne")]
        public async Task<HttpStatusCode> InsertUser(User User)
        {
            var entity = new User()
            {
                FirstName = User.FirstName,
                LastName = User.LastName,
                Username = User.Username,
                Password = User.Password,
                EnrollmentDate = User.EnrollmentDate
            };

            _dbContext.User.Add(entity);
            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        //update
        [HttpPut("UpdateUser")]
        public async Task<HttpStatusCode> UpdateUser(User User)
        {
            var entity = await _dbContext.User.FirstOrDefaultAsync(s => s.Id == User.Id);

            entity.FirstName = User.FirstName;
            entity.LastName = User.LastName;
            entity.Username = User.Username;
            entity.Password = User.Password;
            entity.EnrollmentDate = User.EnrollmentDate;

            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        //Delete
        [HttpDelete("DeleteUser/{Id}")]
        public async Task<HttpStatusCode> DeleteUser(int Id)
        {
            var entity = new User()
            {
                Id = Id
            };
            _dbContext.User.Attach(entity);
            _dbContext.User.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}