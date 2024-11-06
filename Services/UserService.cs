using AutoMapper;
using FirmTracker_Server.Authentication;
using FirmTracker_Server.Entities;
using FirmTracker_Server.Exceptions;
using FirmTracker_Server.Models;
using FirmTracker_Server.Authentication;
using FirmTracker_Server.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using szyfrowanie;
using FirmTracker_Server.nHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace FirmTracker_Server.Services
{
    public interface IUserService
    {
        UserDto GetById(int id);
        int AddUser(CreateUserDto dto);
        string CreateTokenJwt(LoginDto dto);
        bool ChangePassword(int userMail, ChangePasswordDto dto);
        bool ResetPassword(string userId, string newPassword);

    }

    public class UserService : IUserService
    {
       // private readonly GeneralDbContext DbContext;
        private readonly IMapper Mapper;
        private readonly IPasswordHasher<User> PasswordHasher;
        private readonly AuthenticationSettings AuthenticationSettings;
       private readonly SimplerAES SimplerAES;
        //private readonly SessionFactory sessionFactory;

        public UserService( IMapper mapper, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
           // DbContext = dbContext;
            Mapper = mapper;
            PasswordHasher = passwordHasher;
            AuthenticationSettings = authenticationSettings;
            SimplerAES = new SimplerAES();
            //SessionFactory = sessionFactory;
        }

        public UserDto GetById(int id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var user = session.Get<User>(id);
                return user == null ? null : Mapper.Map<UserDto>(user);
            }
        }


        public int AddUser(CreateUserDto dto)
        {
            var user = Mapper.Map<User>(dto);

            // Encrypt or hash the password based on NewEncryption flag
            user.PassHash = SimplerAES.Encrypt(dto.Password); //: PasswordHasher.HashPassword(user, dto.Password);
            user.Role = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dto.Role.ToLower());

            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Save(user);
                    transaction.Commit();
                    return user.Id;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public bool ChangePassword(int userId, ChangePasswordDto dto)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                // Find user by ID
                var user = session.Get<User>(userId);
                if (user == null)
                {
                    throw new WrongUserOrPasswordException("User not found.");
                }

                // Verify old password
                var oldPasswordCorrect = false;
                if (user.NewEncryption)
                {
                    oldPasswordCorrect = SimplerAES.Decrypt(user.PassHash) == SimplerAES.Decrypt(dto.OldPassword);
                }
                else
                {
                    oldPasswordCorrect = SimplerAES.Decrypt(user.PassHash) == SimplerAES.Decrypt(dto.OldPassword);
                }

                if (!oldPasswordCorrect)
                {
                    throw new WrongUserOrPasswordException("The old password is incorrect.");
                }

               
                if (user.NewEncryption)
                {
                    user.PassHash = SimplerAES.Encrypt(dto.NewPassword);
                }
                else
                {
                    user.PassHash = SimplerAES.Encrypt(dto.NewPassword);
                }

                session.Update(user);
                transaction.Commit();
                return true;
            }
        }
        public bool ResetPassword(string userMail, string newPassword)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var user = session.Get<User>(userMail);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Encrypt or hash the new password
                if (user.NewEncryption)
                {
                    user.PassHash = SimplerAES.Encrypt(newPassword); // Or apply hashing if needed
                }
                else
                {
                    user.PassHash = SimplerAES.Encrypt(newPassword);
                }

                session.Update(user);
                transaction.Commit();
                return true;
            }
        }
        public string CreateTokenJwt(LoginDto dto)
        {
            User user = null;

            using (var session = SessionFactory.OpenSession())
            {
                if (!string.IsNullOrEmpty(dto.Email))
                {
                    user = session.Query<User>().FirstOrDefault(x => x.Email == dto.Email);
                }
                else
                {
                    throw new WrongUserOrPasswordException("Nieprawidłowy login lub hasło.");
                }

                if (user == null)
                {
                    throw new WrongUserOrPasswordException("Nieprawidłowy login lub hasło.");
                }

                // Password verification logic
                if (user.NewEncryption)
                {
                    try
                    {
                        Console.WriteLine(SimplerAES.Decrypt(user.PassHash)+" "+SimplerAES.Decrypt(dto.Password));
                        var ready = SimplerAES.Decrypt(user.PassHash) == SimplerAES.Decrypt(dto.Password);
                        if (!ready)
                        {
                            throw new WrongUserOrPasswordException("Nieprawidłowy login lub hasło.");
                        }
                    }
                    catch (Exception)
                    {
                        throw new WrongUserOrPasswordException("Wystąpił błąd podczas logowania");
                    }
                }
                else
                {
                    var ready = PasswordVerificationResult.Failed;
                    if (SimplerAES.Decrypt(user.PassHash) == SimplerAES.Decrypt(dto.Password)) { ready = PasswordVerificationResult.Success; } //PasswordHasher.VerifyHashedPassword(user, user.PassHash, dto.Password);
                    if (ready == PasswordVerificationResult.Failed)
                    {
                        throw new WrongUserOrPasswordException("Nieprawidłowy login lub hasło.");
                    }
                }

                // Generate JWT token
                var claims = new List<Claim>() {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Role, user.Role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationSettings.JwtSecKey));
                var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(AuthenticationSettings.JwtExpireDays);
                var token = new JwtSecurityToken(AuthenticationSettings.JwtIssuer, AuthenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: credential);
                var finalToken = new JwtSecurityTokenHandler();
                return finalToken.WriteToken(token);
            }
        }
    }
}
