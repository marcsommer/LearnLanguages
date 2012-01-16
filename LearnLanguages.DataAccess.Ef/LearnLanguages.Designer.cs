﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
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
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("LearnLanguages", "AssociateLanguageDataWithPhraseData", "LanguageData", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(LearnLanguages.DataAccess.Ef.LanguageData), "PhraseData", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(LearnLanguages.DataAccess.Ef.PhraseData), true)]
[assembly: EdmRelationshipAttribute("LearnLanguages", "AssociationUserDataWithPhraseData", "UserData", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(LearnLanguages.DataAccess.Ef.UserData), "PhraseData", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(LearnLanguages.DataAccess.Ef.PhraseData), true)]
[assembly: EdmRelationshipAttribute("LearnLanguages", "AssociationUserDataWithLanguageData", "UserData", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(LearnLanguages.DataAccess.Ef.UserData), "LanguageData", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(LearnLanguages.DataAccess.Ef.LanguageData))]
[assembly: EdmRelationshipAttribute("LearnLanguages", "AssociationUserDataWithRoleData", "UserData", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(LearnLanguages.DataAccess.Ef.UserData), "RoleData", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(LearnLanguages.DataAccess.Ef.RoleData))]

#endregion

namespace LearnLanguages.DataAccess.Ef
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class LearnLanguagesContext : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new LearnLanguagesContext object using the connection string found in the 'LearnLanguagesContext' section of the application configuration file.
        /// </summary>
        public LearnLanguagesContext() : base("name=LearnLanguagesContext", "LearnLanguagesContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new LearnLanguagesContext object.
        /// </summary>
        public LearnLanguagesContext(string connectionString) : base(connectionString, "LearnLanguagesContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new LearnLanguagesContext object.
        /// </summary>
        public LearnLanguagesContext(EntityConnection connection) : base(connection, "LearnLanguagesContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<LanguageData> LanguageDatas
        {
            get
            {
                if ((_LanguageDatas == null))
                {
                    _LanguageDatas = base.CreateObjectSet<LanguageData>("LanguageDatas");
                }
                return _LanguageDatas;
            }
        }
        private ObjectSet<LanguageData> _LanguageDatas;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<PhraseData> PhraseDatas
        {
            get
            {
                if ((_PhraseDatas == null))
                {
                    _PhraseDatas = base.CreateObjectSet<PhraseData>("PhraseDatas");
                }
                return _PhraseDatas;
            }
        }
        private ObjectSet<PhraseData> _PhraseDatas;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<UserData> UserDatas
        {
            get
            {
                if ((_UserDatas == null))
                {
                    _UserDatas = base.CreateObjectSet<UserData>("UserDatas");
                }
                return _UserDatas;
            }
        }
        private ObjectSet<UserData> _UserDatas;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<RoleData> RoleDatas
        {
            get
            {
                if ((_RoleDatas == null))
                {
                    _RoleDatas = base.CreateObjectSet<RoleData>("RoleDatas");
                }
                return _RoleDatas;
            }
        }
        private ObjectSet<RoleData> _RoleDatas;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the LanguageDatas EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToLanguageDatas(LanguageData languageData)
        {
            base.AddObject("LanguageDatas", languageData);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the PhraseDatas EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToPhraseDatas(PhraseData phraseData)
        {
            base.AddObject("PhraseDatas", phraseData);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the UserDatas EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToUserDatas(UserData userData)
        {
            base.AddObject("UserDatas", userData);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the RoleDatas EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToRoleDatas(RoleData roleData)
        {
            base.AddObject("RoleDatas", roleData);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="LearnLanguages", Name="LanguageData")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class LanguageData : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new LanguageData object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="text">Initial value of the Text property.</param>
        public static LanguageData CreateLanguageData(global::System.Guid id, global::System.String text)
        {
            LanguageData languageData = new LanguageData();
            languageData.Id = id;
            languageData.Text = text;
            return languageData;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Guid _Id;
        partial void OnIdChanging(global::System.Guid value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                OnTextChanging(value);
                ReportPropertyChanging("Text");
                _Text = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Text");
                OnTextChanged();
            }
        }
        private global::System.String _Text;
        partial void OnTextChanging(global::System.String value);
        partial void OnTextChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="LearnLanguages", Name="PhraseData")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class PhraseData : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new PhraseData object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="text">Initial value of the Text property.</param>
        /// <param name="languageDataId">Initial value of the LanguageDataId property.</param>
        /// <param name="userDataId">Initial value of the UserDataId property.</param>
        /// <param name="userDataUsername">Initial value of the UserDataUsername property.</param>
        public static PhraseData CreatePhraseData(global::System.Guid id, global::System.String text, global::System.Guid languageDataId, global::System.Guid userDataId, global::System.String userDataUsername)
        {
            PhraseData phraseData = new PhraseData();
            phraseData.Id = id;
            phraseData.Text = text;
            phraseData.LanguageDataId = languageDataId;
            phraseData.UserDataId = userDataId;
            phraseData.UserDataUsername = userDataUsername;
            return phraseData;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Guid _Id;
        partial void OnIdChanging(global::System.Guid value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                OnTextChanging(value);
                ReportPropertyChanging("Text");
                _Text = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Text");
                OnTextChanged();
            }
        }
        private global::System.String _Text;
        partial void OnTextChanging(global::System.String value);
        partial void OnTextChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid LanguageDataId
        {
            get
            {
                return _LanguageDataId;
            }
            set
            {
                OnLanguageDataIdChanging(value);
                ReportPropertyChanging("LanguageDataId");
                _LanguageDataId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("LanguageDataId");
                OnLanguageDataIdChanged();
            }
        }
        private global::System.Guid _LanguageDataId;
        partial void OnLanguageDataIdChanging(global::System.Guid value);
        partial void OnLanguageDataIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid UserDataId
        {
            get
            {
                return _UserDataId;
            }
            set
            {
                OnUserDataIdChanging(value);
                ReportPropertyChanging("UserDataId");
                _UserDataId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("UserDataId");
                OnUserDataIdChanged();
            }
        }
        private global::System.Guid _UserDataId;
        partial void OnUserDataIdChanging(global::System.Guid value);
        partial void OnUserDataIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String UserDataUsername
        {
            get
            {
                return _UserDataUsername;
            }
            set
            {
                OnUserDataUsernameChanging(value);
                ReportPropertyChanging("UserDataUsername");
                _UserDataUsername = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("UserDataUsername");
                OnUserDataUsernameChanged();
            }
        }
        private global::System.String _UserDataUsername;
        partial void OnUserDataUsernameChanging(global::System.String value);
        partial void OnUserDataUsernameChanged();

        #endregion

    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("LearnLanguages", "AssociateLanguageDataWithPhraseData", "LanguageData")]
        public LanguageData LanguageData
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LanguageData>("LearnLanguages.AssociateLanguageDataWithPhraseData", "LanguageData").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LanguageData>("LearnLanguages.AssociateLanguageDataWithPhraseData", "LanguageData").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<LanguageData> LanguageDataReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<LanguageData>("LearnLanguages.AssociateLanguageDataWithPhraseData", "LanguageData");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<LanguageData>("LearnLanguages.AssociateLanguageDataWithPhraseData", "LanguageData", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="LearnLanguages", Name="RoleData")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class RoleData : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new RoleData object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="text">Initial value of the Text property.</param>
        public static RoleData CreateRoleData(global::System.Guid id, global::System.String text)
        {
            RoleData roleData = new RoleData();
            roleData.Id = id;
            roleData.Text = text;
            return roleData;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Guid _Id;
        partial void OnIdChanging(global::System.Guid value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                OnTextChanging(value);
                ReportPropertyChanging("Text");
                _Text = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Text");
                OnTextChanged();
            }
        }
        private global::System.String _Text;
        partial void OnTextChanging(global::System.String value);
        partial void OnTextChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="LearnLanguages", Name="UserData")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class UserData : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new UserData object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="username">Initial value of the Username property.</param>
        /// <param name="salt">Initial value of the Salt property.</param>
        /// <param name="saltedHashedPasswordValue">Initial value of the SaltedHashedPasswordValue property.</param>
        public static UserData CreateUserData(global::System.Guid id, global::System.String username, global::System.Int32 salt, global::System.String saltedHashedPasswordValue)
        {
            UserData userData = new UserData();
            userData.Id = id;
            userData.Username = username;
            userData.Salt = salt;
            userData.SaltedHashedPasswordValue = saltedHashedPasswordValue;
            return userData;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Guid _Id;
        partial void OnIdChanging(global::System.Guid value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (_Username != value)
                {
                    OnUsernameChanging(value);
                    ReportPropertyChanging("Username");
                    _Username = StructuralObject.SetValidValue(value, false);
                    ReportPropertyChanged("Username");
                    OnUsernameChanged();
                }
            }
        }
        private global::System.String _Username;
        partial void OnUsernameChanging(global::System.String value);
        partial void OnUsernameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Salt
        {
            get
            {
                return _Salt;
            }
            set
            {
                OnSaltChanging(value);
                ReportPropertyChanging("Salt");
                _Salt = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Salt");
                OnSaltChanged();
            }
        }
        private global::System.Int32 _Salt;
        partial void OnSaltChanging(global::System.Int32 value);
        partial void OnSaltChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String SaltedHashedPasswordValue
        {
            get
            {
                return _SaltedHashedPasswordValue;
            }
            set
            {
                OnSaltedHashedPasswordValueChanging(value);
                ReportPropertyChanging("SaltedHashedPasswordValue");
                _SaltedHashedPasswordValue = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("SaltedHashedPasswordValue");
                OnSaltedHashedPasswordValueChanged();
            }
        }
        private global::System.String _SaltedHashedPasswordValue;
        partial void OnSaltedHashedPasswordValueChanging(global::System.String value);
        partial void OnSaltedHashedPasswordValueChanged();

        #endregion

    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("LearnLanguages", "AssociationUserDataWithPhraseData", "PhraseData")]
        public EntityCollection<PhraseData> PhraseDatas
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<PhraseData>("LearnLanguages.AssociationUserDataWithPhraseData", "PhraseData");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<PhraseData>("LearnLanguages.AssociationUserDataWithPhraseData", "PhraseData", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("LearnLanguages", "AssociationUserDataWithLanguageData", "LanguageData")]
        public EntityCollection<LanguageData> LanguageDatas
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<LanguageData>("LearnLanguages.AssociationUserDataWithLanguageData", "LanguageData");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<LanguageData>("LearnLanguages.AssociationUserDataWithLanguageData", "LanguageData", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("LearnLanguages", "AssociationUserDataWithRoleData", "RoleData")]
        public EntityCollection<RoleData> RoleDatas
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<RoleData>("LearnLanguages.AssociationUserDataWithRoleData", "RoleData");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<RoleData>("LearnLanguages.AssociationUserDataWithRoleData", "RoleData", value);
                }
            }
        }

        #endregion

    }

    #endregion

    
}
