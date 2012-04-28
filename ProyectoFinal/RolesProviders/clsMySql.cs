using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Security;
using System.Configuration;
using LogInMySQLEntity;
using System.Security.Cryptography;

namespace RolesProviders
{
    public class MySQL : RoleProvider
    {
        string cnnstring = ConfigurationManager.ConnectionStrings["dbmysqlEntities"].ConnectionString;

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                foreach (String user in usernames)
                {
                    foreach (String rol in roleNames)
                    {
                        usersgroup usrgrp = (from e1 in db.usersgroups
                                            let usr = db.users.Where(u => u.UserName == user).FirstOrDefault()
                                            let grp = db.groups.Where(g => g.GroupName == rol).FirstOrDefault()
                                            select new usersgroup
                                            {
                                                IdUsers = usr != null ? usr.IdUsers : -1,
                                                IdGroups = grp != null ? grp.IdGroups : -1 
                                            }).FirstOrDefault();
                        if (usrgrp.IdGroups != -1 || usrgrp.IdUsers != -1)
                        {
                            db.usersgroups.AddObject(usrgrp);
                            db.SaveChanges();
                        }
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
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                db.groups.AddObject(new group() { GroupName = roleName });
                db.SaveChanges();
            }
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                try
                {
                    group grp = db.groups.Where(g => g.GroupName == roleName).FirstOrDefault();
                    db.groups.DeleteObject(grp);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                var users = from e1 in db.users
                            where e1.IdUsers == (db.usersgroups.Where(u => u.IdGroups == db.groups.Where(g => g.GroupName == roleName).FirstOrDefault().IdGroups)).FirstOrDefault().IdUsers
                            select e1.UserName;
                return users.Where(u => u.Contains(usernameToMatch)).ToArray();
            }
        }

        public override string[] GetAllRoles()
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                return db.groups.Select(g => g.GroupName).ToArray();
            }
        }

        public override string[] GetRolesForUser(string username)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                user usr = db.users.Where(u => u.UserName == username).FirstOrDefault();
                var roles = db.usersgroups.Where(ug => ug.IdUsers == usr.IdUsers);

                List<string> rls = new List<string>();
                foreach (var rol in roles)
                    rls.Add(db.groups.Where(g => g.IdGroups == rol.IdGroups).FirstOrDefault().GroupName);

                return rls.ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                group grp = db.groups.Where(g => g.GroupName == roleName).FirstOrDefault();
                var usrs = db.usersgroups.Where(ug => ug.IdGroups == grp.IdGroups);

                List<string> users = new List<string>();
                foreach (var usr in usrs)
                    users.Add(db.users.Where(u => u.IdUsers == usr.IdUsers).FirstOrDefault().UserName);

                return users.ToArray();
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                if (db.usersgroups.Where(ug => ug.IdUsers == db.users.Where(u => u.UserName == username).First().IdUsers && ug.IdGroups == db.groups.Where(g => g.GroupName == roleName).First().IdGroups).FirstOrDefault() != null)
                    return true;
                else
                    return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                foreach (String user in usernames)
                {
                    foreach (String rol in roleNames)
                    {
                        usersgroup usrgrp = (from e1 in db.usersgroups
                                             let usr = db.users.Where(u => u.UserName == user).FirstOrDefault()
                                             let grp = db.groups.Where(g => g.GroupName == rol).FirstOrDefault()
                                             where e1.IdGroups == grp.IdGroups && e1.IdUsers == usr.IdUsers
                                             select e1).FirstOrDefault();

                        if (usrgrp != null)
                        {
                            db.usersgroups.DeleteObject(usrgrp);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        public override bool RoleExists(string roleName)
        {
            using (dbmysqlEntities db = new dbmysqlEntities(cnnstring))
            {
                if (db.groups.Where(g => g.GroupName == roleName).FirstOrDefault() != null)
                    return true;
                else
                    return false;
            }
        }
    }
}