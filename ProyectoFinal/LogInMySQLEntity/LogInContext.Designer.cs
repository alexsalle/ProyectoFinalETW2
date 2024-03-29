﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]

namespace LogInMySQLEntity
{
    #region Contextos
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    public partial class dbmysqlEntities : ObjectContext
    {
        #region Constructores
    
        /// <summary>
        /// Inicializa un nuevo objeto dbmysqlEntities usando la cadena de conexión encontrada en la sección 'dbmysqlEntities' del archivo de configuración de la aplicación.
        /// </summary>
        public dbmysqlEntities() : base("name=dbmysqlEntities", "dbmysqlEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto dbmysqlEntities.
        /// </summary>
        public dbmysqlEntities(string connectionString) : base(connectionString, "dbmysqlEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Inicializar un nuevo objeto dbmysqlEntities.
        /// </summary>
        public dbmysqlEntities(EntityConnection connection) : base(connection, "dbmysqlEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Métodos parciales
    
        partial void OnContextCreated();
    
        #endregion
    
        #region Propiedades de ObjectSet
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<group> groups
        {
            get
            {
                if ((_groups == null))
                {
                    _groups = base.CreateObjectSet<group>("groups");
                }
                return _groups;
            }
        }
        private ObjectSet<group> _groups;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<user> users
        {
            get
            {
                if ((_users == null))
                {
                    _users = base.CreateObjectSet<user>("users");
                }
                return _users;
            }
        }
        private ObjectSet<user> _users;
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        public ObjectSet<usersgroup> usersgroups
        {
            get
            {
                if ((_usersgroups == null))
                {
                    _usersgroups = base.CreateObjectSet<usersgroup>("usersgroups");
                }
                return _usersgroups;
            }
        }
        private ObjectSet<usersgroup> _usersgroups;

        #endregion
        #region Métodos AddTo
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet groups. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddTogroups(group group)
        {
            base.AddObject("groups", group);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet users. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddTousers(user user)
        {
            base.AddObject("users", user);
        }
    
        /// <summary>
        /// Método desusado para agregar un nuevo objeto al EntitySet usersgroups. Considere la posibilidad de usar el método .Add de la propiedad ObjectSet&lt;T&gt; asociada.
        /// </summary>
        public void AddTousersgroups(usersgroup usersgroup)
        {
            base.AddObject("usersgroups", usersgroup);
        }

        #endregion
    }
    

    #endregion
    
    #region Entidades
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="dbmysqlModel", Name="group")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class group : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto group.
        /// </summary>
        /// <param name="idGroups">Valor inicial de la propiedad IdGroups.</param>
        /// <param name="groupName">Valor inicial de la propiedad GroupName.</param>
        public static group Creategroup(global::System.Int32 idGroups, global::System.String groupName)
        {
            group group = new group();
            group.IdGroups = idGroups;
            group.GroupName = groupName;
            return group;
        }

        #endregion
        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 IdGroups
        {
            get
            {
                return _IdGroups;
            }
            set
            {
                if (_IdGroups != value)
                {
                    OnIdGroupsChanging(value);
                    ReportPropertyChanging("IdGroups");
                    _IdGroups = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("IdGroups");
                    OnIdGroupsChanged();
                }
            }
        }
        private global::System.Int32 _IdGroups;
        partial void OnIdGroupsChanging(global::System.Int32 value);
        partial void OnIdGroupsChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String GroupName
        {
            get
            {
                return _GroupName;
            }
            set
            {
                OnGroupNameChanging(value);
                ReportPropertyChanging("GroupName");
                _GroupName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("GroupName");
                OnGroupNameChanged();
            }
        }
        private global::System.String _GroupName;
        partial void OnGroupNameChanging(global::System.String value);
        partial void OnGroupNameChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="dbmysqlModel", Name="user")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class user : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto user.
        /// </summary>
        /// <param name="idUsers">Valor inicial de la propiedad IdUsers.</param>
        /// <param name="userName">Valor inicial de la propiedad UserName.</param>
        /// <param name="password">Valor inicial de la propiedad Password.</param>
        public static user Createuser(global::System.Int32 idUsers, global::System.String userName, global::System.String password)
        {
            user user = new user();
            user.IdUsers = idUsers;
            user.UserName = userName;
            user.Password = password;
            return user;
        }

        #endregion
        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 IdUsers
        {
            get
            {
                return _IdUsers;
            }
            set
            {
                if (_IdUsers != value)
                {
                    OnIdUsersChanging(value);
                    ReportPropertyChanging("IdUsers");
                    _IdUsers = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("IdUsers");
                    OnIdUsersChanged();
                }
            }
        }
        private global::System.Int32 _IdUsers;
        partial void OnIdUsersChanging(global::System.Int32 value);
        partial void OnIdUsersChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                OnUserNameChanging(value);
                ReportPropertyChanging("UserName");
                _UserName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("UserName");
                OnUserNameChanged();
            }
        }
        private global::System.String _UserName;
        partial void OnUserNameChanging(global::System.String value);
        partial void OnUserNameChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                OnPasswordChanging(value);
                ReportPropertyChanging("Password");
                _Password = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Password");
                OnPasswordChanged();
            }
        }
        private global::System.String _Password;
        partial void OnPasswordChanging(global::System.String value);
        partial void OnPasswordChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No hay documentación de metadatos disponible.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="dbmysqlModel", Name="usersgroup")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class usersgroup : EntityObject
    {
        #region Método de generador
    
        /// <summary>
        /// Crear un nuevo objeto usersgroup.
        /// </summary>
        /// <param name="idUsers">Valor inicial de la propiedad IdUsers.</param>
        /// <param name="idGroups">Valor inicial de la propiedad IdGroups.</param>
        public static usersgroup Createusersgroup(global::System.Int32 idUsers, global::System.Int32 idGroups)
        {
            usersgroup usersgroup = new usersgroup();
            usersgroup.IdUsers = idUsers;
            usersgroup.IdGroups = idGroups;
            return usersgroup;
        }

        #endregion
        #region Propiedades primitivas
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 IdUsers
        {
            get
            {
                return _IdUsers;
            }
            set
            {
                if (_IdUsers != value)
                {
                    OnIdUsersChanging(value);
                    ReportPropertyChanging("IdUsers");
                    _IdUsers = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("IdUsers");
                    OnIdUsersChanged();
                }
            }
        }
        private global::System.Int32 _IdUsers;
        partial void OnIdUsersChanging(global::System.Int32 value);
        partial void OnIdUsersChanged();
    
        /// <summary>
        /// No hay documentación de metadatos disponible.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 IdGroups
        {
            get
            {
                return _IdGroups;
            }
            set
            {
                if (_IdGroups != value)
                {
                    OnIdGroupsChanging(value);
                    ReportPropertyChanging("IdGroups");
                    _IdGroups = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("IdGroups");
                    OnIdGroupsChanged();
                }
            }
        }
        private global::System.Int32 _IdGroups;
        partial void OnIdGroupsChanging(global::System.Int32 value);
        partial void OnIdGroupsChanged();

        #endregion
    
    }

    #endregion
    
}
