using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Security;
using System.Configuration;
using LogInMySQLEntity;
using System.Security.Cryptography;



namespace MembershipProviders
{
    public class MySQL : MembershipProvider
    {
        string cnnstring = ConfigurationManager.ConnectionStrings["dbmysqlEntities"].ConnectionString;

        public string appName { get; set; }
        public override string ApplicationName
        {
            get
            {
                return appName;
            }
            set
            {
                appName = value;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                user usr = db.users.Where(u => u.UserName == username).FirstOrDefault();
                if (usr == null)
                {
                    usr = new user()
                    {
                        UserName = username,
                        Password = GetSHA1(password)
                    };

                    try
                    {
                        db.users.AddObject(usr);
                        db.SaveChanges();

                        MembershipUser msUser = new MembershipUser("MySQLMembershipProvider", usr.UserName, providerUserKey, usr.UserName + "@dxstudio.net", string.Empty, string.Empty, true, false, DateTime.MinValue, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
                        status = MembershipCreateStatus.Success;
                        return msUser;
                    }
                    catch (Exception ex)
                    {
                        //Verificamos que efectivamente no se cree el usuario
                        usr = db.users.Where(u => u.UserName == username).FirstOrDefault();
                        if (usr != null)
                        {
                            db.users.DeleteObject(usr);
                            db.SaveChanges();
                        }
                        status = MembershipCreateStatus.UserRejected;
                        return null;
                    }
                }
                else
                {
                    MembershipUser msUser = new MembershipUser("MySQLMembershipProvider", usr.UserName, providerUserKey, usr.UserName + "@dxstudio.net", string.Empty, string.Empty, true, false, DateTime.MinValue, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
                    status = MembershipCreateStatus.DuplicateUserName;
                    return msUser;
                }
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                user usr = db.users.Where(u => u.UserName == username).FirstOrDefault();
                if (usr != null)
                {
                    try
                    {
                        db.users.DeleteObject(usr);
                        db.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                string pass = GetSHA1(password);
                if (db.users.Where(u => u.UserName == username && u.Password == pass).FirstOrDefault() != null)
                    return true;
                else
                    return false;
            }
        }

        public static string GetSHA1(string str)
        {
            SHA1 sha1 = SHA1Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha1.ComputeHash(encoding.GetBytes(str));
            
            for (int i = 0; i < stream.Length; i++) 
                sb.AppendFormat("{0:x2}", stream[i]);

            return sb.ToString();
        }
    }
}