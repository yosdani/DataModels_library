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

namespace Datamodels.Logic
{
    public abstract class BaseLogic
    {
        protected const string createTransaction = "create", updateTransaction = "update", crudTransaction = $"crud", deleteTransaction = "delete";
        protected readonly Context context;

        protected BaseLogic(Context context) => this.context = context;

        protected bool ExistsUser(string email, MasterUser masterUser)
        {
            if (EmailSupport.ValidateEmail(email))
                return context.Users.Any(GetEmailFilter(email, masterUser));
            return false;
        }

        public User GetUser(string email, MasterUser masterUser, IEnumerable<int> generalStatus = null, IEnumerable<int> userRoles = null)
        {
            if (EmailSupport.ValidateEmail(email))
                return context.Users.Include(u => u.Role).FirstOrDefault(GetFilter(email, masterUser, generalStatus, userRoles));
            return null;
        }

        public int? GetUserId(string email, MasterUser masterUser, IEnumerable<int> generalStatus = null)
        {
            if (EmailSupport.ValidateEmail(email))
                return context.Users.Where(GetFilter(email, masterUser, generalStatus)).Select(u => u.Id).Cast<int?>().FirstOrDefault();
            return null;
        }

        public Tuple<int, int> GetUserIdRoleId(string email, MasterUser masterUser, IEnumerable<int> generalStatus = null)
        {
            if (EmailSupport.ValidateEmail(email))
                return context.Users.Where(GetFilter(email, masterUser, generalStatus)).Select(u => new Tuple<int, int>(u.Id, u.RoleId)).FirstOrDefault();
            return null;
        }

        protected internal Expression<Func<User, bool>> GetEmailFilter(string email, MasterUser masterUser)
        {
            email = email?.ToLower().Trim();
            return u => u.Id != masterUser.Id && u.Email.ToLower().Trim() == email;
        }

        protected internal Expression<Func<User, bool>> GetStatusFilter(IEnumerable<int> generalStatus = null) => u => generalStatus == null || generalStatus.Any(s => s == u.StatusId);

        protected internal Expression<Func<User, bool>> GetRoleFilter(IEnumerable<int> userRoles = null) => u => userRoles == null || userRoles.Any(r => r == u.RoleId);

        protected internal Expression<Func<User, bool>> GetFilter(IEnumerable<int> generalStatus = null, IEnumerable<int> userRoles = null) => GetStatusFilter(generalStatus)._AndAlso(GetRoleFilter(userRoles));

        protected internal Expression<Func<User, bool>> GetFilter(string email, MasterUser masterUser, IEnumerable<int> generalStatus = null) => GetEmailFilter(email, masterUser)._AndAlso(GetStatusFilter(generalStatus));

        protected internal Expression<Func<User, bool>> GetFilter(string email, MasterUser masterUser, IEnumerable<int> generalStatus = null, IEnumerable<int> userRoles = null) => GetEmailFilter(email, masterUser)._AndAlso(GetFilter(generalStatus, userRoles));



        protected static string EncryptURL(string url) => Encryption.ToURLFix(Encryption.Encrypt_2(url));
    }
}
