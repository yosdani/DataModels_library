using CommonTypes.Settings.Keys;
using CommonTypes.Util;
using Datamodels.Extentions;
using Datamodels.Models;
using Datamodels.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static CommonTypes.Language.LanguageSupport;
using static CommonTypes.Request.Models.UserRequests;

namespace Datamodels.Logic
{
    public class UserLogic : BaseLogic
    {
        private static readonly LanguageObject message_exists = new LanguageObject("The user already exists", "O utilizador já existe");

        public UserLogic(Context context) : base(context) { }

        public User Authenticate(User_Authenticate aut, MasterUser masterUser, out LanguageObject message, IEnumerable<int> generalStatus = null)
        {
            if (aut.IsValid(out message))
            {
                string password = Encryption.Encrypt_1(aut.Password);
                bool filterByStatus = generalStatus != null;
                return context.Users.Include(u => u.Role).FirstOrDefault(GetAuthFilter(aut.Email, password, masterUser, generalStatus));
            }
            return null;
        }

        private Expression<Func<User, bool>> GetAuthFilter(string email, string password, MasterUser masterUser, IEnumerable<int> generalStatus = null) => GetFilter(email, masterUser, generalStatus)._AndAlso(u => password == u.Password);

        public User Register(User_Register request, MasterUser masterUser, int statusId, UserRoles userRoles, out LanguageObject message) => CreateUser(new User_Create()
        {
            Email = request.Email,
            Name = request.Name,
            Lastname = request.Lastname,
            StatusId = statusId,
            RoleId = userRoles.Admin
        }, request.Password, masterUser, out message);

        public User CreateUser(User_Create request, string password, MasterUser masterUser, out LanguageObject message)
        {
            if (request.IsValid(out message))
            {
                if (ExistsUser(request.Email, masterUser))
                {
                    message = message_exists;
                    return null;
                }
                User user = new User
                {
                    Email = request.Email,
                    Name = request.Name,
                    Password = Encryption.Encrypt_1(password),
                    Surname = request.Lastname,
                    StatusId = request.StatusId,
                    RoleId = request.RoleId,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }
            return null;
        }

        public User ChangeStatus(string email, MasterUser masterUser, int newStatus, out LanguageObject message, IEnumerable<int> generalStatus = null)
        {
            User user = GetUser(email, masterUser, generalStatus);
            if (user != null)
            {
                message = null;
                ChangeStatus(user, newStatus);
                context.SaveChanges();
                return user;
            }
            message = message_invalidCredentials;
            return null;
        }

        public bool ChangeStatus(IEnumerable<string> emails, MasterUser masterUser, int newStatus, out LanguageObject message, IEnumerable<int> generalStatus = null)
        {
            User user;
            foreach (string email in emails)
            {
                user = GetUser(email, masterUser, generalStatus);
                if (user != null)
                    ChangeStatus(user, newStatus);
                else
                {
                    message = message_invalidUser;
                    return false;
                }
            }
            context.SaveChanges();
            message = null;
            return true;
        }

        private void ChangeStatus(User u, int status)
        {
            u.StatusId = status;
            u.Token = null;
        }

        public User SetToken(string email, string token, MasterUser masterUser, IEnumerable<int> generalStatus = null)
        {
            User user = GetUser(email, masterUser, generalStatus);
            if (user != null)
            {
                user.Token = token;
                context.SaveChanges();
                return user;
            }
            return null;
        }

        public User ChangePassword(string email, string password, MasterUser masterUser, IEnumerable<int> generalStatus = null)
        {
            User user = GetUser(email, masterUser, generalStatus);
            if (user != null)
            {
                user.Password = Encryption.Encrypt_1(password);
                user.Token = null;
                context.SaveChanges();
                return user;
            }
            return null;
        }

        public bool UpdateUser(UserCore request, MasterUser masterUser, out LanguageObject message, IEnumerable<int> generalStatus = null)
        {
            if (request.IsValid(out message))
            {
                User user = GetUser(request.Email, masterUser, generalStatus);
                if (user != null)
                {
                    user.Name = request.Name;
                    user.Surname = request.Lastname;
                    user.Token = null;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool UpdateUser(User_Update request, MasterUser masterUser, out LanguageObject message, IEnumerable<int> generalStatus = null)
        {
            if (request.IsValid(out message))
            {
                User user = GetUser(request.Email, masterUser, generalStatus);
                if (user != null)
                {
                    user.Name = request.Name;
                    user.Surname = request.Lastname;
                    user.RoleId = request.RoleId;
                    user.StatusId = request.StatusId;
                    user.Token = null;
                    context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<object> GetUsers(User_Get request, MasterUser masterUser) => context.Users.Where(GetFilter(request, masterUser)).OrderByDescending(u => u.Id).Select(u => new
        {
            name = u.Name,
            lastName = u.Surname,
            email = u.Email,

            status = new
            {
                id = u.StatusId,
                name = new LanguageObject(u.Status.NameEn, u.Status.NameEs)
            },
            role = new
            {
                name = new LanguageObject(u.Role.NameEn, u.Role.NameEs),

            }
        }).ToList();


        private Expression<Func<User, bool>> GetFilter(User_Get request, MasterUser masterUser) => GetFilter(request.StatusIds, request.RolesIds)._AndAlso(u => u.Id != masterUser.Id);
        public object GetUserData(string email)
        {
            if (EmailSupport.ValidateEmail(email))
            {
                email = email.ToLower().Trim();
                return context.Users.Where(u => u.Email.ToLower().Trim() == email).Select(u => new
                {
                    name = u.Name,
                    lastName = u.Surname,
                    email = u.Email,

                    status = new
                    {
                        id = u.StatusId,
                        name = new LanguageObject(u.Status.NameEn, u.Status.NameEs)
                    },
                    role = new
                    {
                        id = u.RoleId,
                        name = new LanguageObject(u.Role.NameEn, u.Role.NameEs),
                        description = new LanguageObject(u.Role.DescriptionEn, u.Role.DescriptionEs),

                    }
                }).FirstOrDefault();
            }
            return null;
        }
    }
}
