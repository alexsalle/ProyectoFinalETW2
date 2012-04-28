using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Security;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
using Tamir.SharpSsh;
using System.Net.Mail;

namespace MembershipProviders
{
    public class ActiveDirectory : MembershipProvider
    {
        #region Variables

        private static string DOMAIN = ConfigurationManager.AppSettings["ADDomain"].ToString();
        private static string PATH_DOMAIN = ConfigurationManager.AppSettings["ADPath"].ToString();
        private static string ADMIN_USER = ConfigurationManager.AppSettings["ADAdminUser"].ToString();
        private static string ADMIN_PASSWORD = ConfigurationManager.AppSettings["ADAdminPassword"].ToString();

        #endregion

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
            UserPrincipal user = GetUser(username) ?? null;
   
            if (user == null)
            {
                user = new UserPrincipal(GetPrincipalContext());
                //User Log on Name
                user.SamAccountName = username;
                user.SetPassword(password);
                user.Enabled = true;
                user.UserPrincipalName = username;
                user.GivenName = username;
                user.Surname = username;
                user.EmailAddress = email;
                user.UserCannotChangePassword = false;
                user.DisplayName = username;
                try
                {
                    user.Save();

                    MembershipUser msUser = new MembershipUser("ActiveDirectoryMembershipProvider", user.SamAccountName, providerUserKey, user.EmailAddress, string.Empty, string.Empty, true, user.IsAccountLockedOut(), DateTime.MinValue, user.LastLogon ?? DateTime.Now, user.LastBadPasswordAttempt ?? DateTime.Now, user.LastPasswordSet ?? DateTime.Now, user.AccountLockoutTime ?? DateTime.Now);

                    // Nos conectamos via SSH hacia el servidor de Zimbra
                    SshExec exec = new SshExec("mail.dxstudio.net", "alex");
                    exec.Password = "admin123";
                    exec.Connect();
                    // Una vez conectados al servidor de Zimbra
                    // estructuramos y armamos el comando Linux 
                    // necesario crear el MailBox
                    string strCommand = string.Empty;
                    strCommand = "/opt/zimbra/bin/./zmprov -a admin -p Admin1234 ca " + user.SamAccountName + "@dxstudio.net SoyUnPassword";
                    // Ejecutamos el comando Linux para crear el MailBox
                    strCommand = exec.RunCommand(strCommand);
                    // Cerreamos la Conexion SSH
                    exec.Close();
                    // Enviamos Mensaje de bienvenida
                    SenMail(user.SamAccountName);

                    status = MembershipCreateStatus.Success;
                    return msUser;
                }
                catch (Exception ex)
                {
                    // verificamos que efectivamente no se cree el usuario
                    var usr = GetUser(username) ?? null;
                    if (usr != null)
                        usr.Delete();
                    status = MembershipCreateStatus.UserRejected;
                    return null;
                }
            }
            else
            {
                MembershipUser msUser = new MembershipUser("ActiveDirectoryMembershipProvider", user.SamAccountName, providerUserKey, user.EmailAddress, string.Empty, string.Empty, true, user.IsAccountLockedOut(), DateTime.MinValue, user.LastLogon ?? DateTime.Now, user.LastBadPasswordAttempt ?? DateTime.Now, user.LastPasswordSet ?? DateTime.Now, user.AccountLockoutTime ?? DateTime.Now);
                status = MembershipCreateStatus.DuplicateUserName;
                return msUser;
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            var usr = GetUser(username) ?? null;
            if (usr != null)
            {
                try
                {
                    usr.Delete();
                    //borramos el mailbox
                    SshExec exec = new SshExec("mail.dxstudio.net", "alex");
                    exec.Password = "admin123";
                    exec.Connect();
                    string strCommand = string.Empty;
                    strCommand = "/opt/zimbra/bin/./zmprov -a admin -p Admin1234 da " + usr.SamAccountName + "@dxstudio.net";
                    // Ejecutamos el comando Linux para eliminar el MailBox
                    strCommand = exec.RunCommand(strCommand);
                    // Cerreamos la Conexion SSH
                    exec.Close();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
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
            get { return 6; }
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
            get { return false; }
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
            PrincipalContext oPrincipalContext = GetPrincipalContext();
            return oPrincipalContext.ValidateCredentials(username, password);
        }


        #region Private Methods

        private UserPrincipal GetUser(String strUserName)
        {
            UserPrincipal objUser = UserPrincipal.FindByIdentity(GetPrincipalContext(), strUserName);
            return objUser;
        }
        private GroupPrincipal GetGroup(String strGroupName)
        {
            GroupPrincipal objGroup = GroupPrincipal.FindByIdentity(GetPrincipalContext(), strGroupName);
            return objGroup;
        }
        private String[] SearchGroups(GroupPrincipal gpGroup)
        {
            PrincipalSearcher objSearcher = new PrincipalSearcher();
            objSearcher.QueryFilter = gpGroup;
            PrincipalSearchResult<Principal> results = objSearcher.FindAll();
            List<String> roles = new List<String>();
            foreach (Principal p in results)
            {
                roles.Add(p.Name);
            }
            return roles.ToArray();
        }

        private PrincipalContext GetPrincipalContext()
        {
            //PrincipalContext objContext = new PrincipalContext(ContextType.Domain, DOMAIN, PATH_DOMAIN, ContextOptions.SimpleBind, ADMIN_USER, ADMIN_PASSWORD);
           PrincipalContext objContext = new PrincipalContext(ContextType.Domain, DOMAIN, ADMIN_USER, ADMIN_PASSWORD);
           return objContext;
        }

        protected void SenMail(string username)
        {
	        MailMessage correo = new MailMessage();
            StringBuilder mensaje = new StringBuilder();
            correo.From = new MailAddress("admin@dxstudio.net", "Administrador");
            correo.Bcc.Add("admin@dxstudio.net");
            correo.To.Add(new MailAddress(username + "@dxstudio.net", username));
            correo.Subject = "Bienvenido";
            correo.IsBodyHtml = true;
            //Encabezado HTML
            mensaje.Append("<h3>Bienvenido</h3>");
            mensaje.Append("<div>Gracias por registrarte " + username);
            mensaje.Append("</div>");
            correo.Body = mensaje.ToString();
            correo.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 25;
            smtp.Host = "mail.dxstudio.net";
            smtp.Credentials= new System.Net.NetworkCredential("admin", "Admin1234");                   
	        try
            {
        	    smtp.Send(correo);                
       	    }
            catch(Exception ex)
	        {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
