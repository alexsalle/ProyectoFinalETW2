using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.DirectoryServices.AccountManagement;
using System.Configuration;
using System.Web.Security;

namespace RolesProviders
{
    public class ActiveDirectory : RoleProvider
    {
        private static String DOMAIN = ConfigurationManager.AppSettings["ADDomain"].ToString();
        private static String PATH_DOMAIN = ConfigurationManager.AppSettings["ADPath"].ToString();
        private static String ADMIN_USER = ConfigurationManager.AppSettings["ADAdminUser"].ToString();
        private static String ADMIN_PASSWORD = ConfigurationManager.AppSettings["ADAdminPassword"].ToString();

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (String user in usernames)
            {
                foreach (String rol in roleNames)
                {
                    UserPrincipal objUser = GetUser(user);
                    GroupPrincipal objGroup = GetGroup(rol);
                    if (!objGroup.Members.Contains(objUser))
                    {
                        objGroup.Members.Add(objUser);
                        objGroup.Save();
                    }
                }
            }
        }

        public string appname { get; set; }
        public override string ApplicationName
        {
            get
            {
                return appname;
            }
            set
            {
                appname = value;
            }
        }

        public override void CreateRole(string roleName)
        {
            GroupPrincipal newGroup = new GroupPrincipal(GetPrincipalContext(), roleName);
            newGroup.IsSecurityGroup = true;
            newGroup.Description = roleName;
            newGroup.Save();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (RoleExists(roleName))
            {
                GroupPrincipal grp = GetGroup(roleName);
                try
                {
                    grp.Delete();
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

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            var usrs = GetUsersInRole(roleName);
            var result = usrs.Where(p => p.Contains(usernameToMatch));

            return result.ToArray();
        }

        public override string[] GetAllRoles()
        {
            GroupPrincipal objGroup = new GroupPrincipal(GetPrincipalContext());
            objGroup.Name = "*";
            return SearchGroups(objGroup);
        }

        public override string[] GetRolesForUser(string username)
        {
            UserPrincipal objUser = UserPrincipal.FindByIdentity(GetPrincipalContext(), username); 
            PrincipalSearchResult<Principal> objSearch = objUser.GetGroups();
            List<String> roles = new List<string>();
            foreach (Principal oresult in objSearch)
            {
                roles.Add(oresult.Name);
            }
            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            GroupPrincipal group = GetGroup(roleName);
            List<string> lstUsers = new List<string>();
            foreach (var item in group.Members)
            {
                lstUsers.Add(item.SamAccountName);
            }
            return lstUsers.ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (GetUser(username) != null && GetGroup(roleName) != null)
            {
                return GetGroup(roleName).Members.Contains(GetUser(username));
            }
            else
            {
                return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (String user in usernames)
            {
                foreach (String rol in roleNames)
                {
                    GroupPrincipal objGroup = GetGroup(rol);
                    objGroup.Members.Remove(GetUser(user));
                    objGroup.Save();
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            if (GetGroup(roleName) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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

        

        #endregion
    }
}
