using System;
using WhizzBase.Attributes;
using WhizzBase.Base;
using NpgsqlTypes;
using Newtonsoft.Json.Linq;

namespace Domain
{
    [Table("auth.users")]
    public partial class AuthUsers : BaseEntity
    {
        [Column("access_failed_count", 6)]
        public short AccessFailedCount
        {
            get => _accessFailedCount;
            set
            {
                _accessFailedCount = value;
                MarkDirty("AccessFailedCount");
            }
        }
        private short _accessFailedCount;

        [Column("active", 10)]
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                MarkDirty("Active");
            }
        }
        private bool _active;

        [Column("email_confirmed", 5)]
        public bool EmailConfirmed
        {
            get => _emailConfirmed;
            set
            {
                _emailConfirmed = value;
                MarkDirty("EmailConfirmed");
            }
        }
        private bool _emailConfirmed;

        [PrimaryKey]
        [Column("id", 1)]
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private Guid _id;

        [Column("locked", 9)]
        public bool Locked
        {
            get => _locked;
            set
            {
                _locked = value;
                MarkDirty("Locked");
            }
        }
        private bool _locked;

        [Column("lockout_enabled", 7)]
        public bool LockoutEnabled
        {
            get => _lockoutEnabled;
            set
            {
                _lockoutEnabled = value;
                MarkDirty("LockoutEnabled");
            }
        }
        private bool _lockoutEnabled;

        [Required]
        [Column("lockout_end", 8)]
        public NpgsqlDateTime LockoutEnd
        {
            get => _lockoutEnd;
            set
            {
                _lockoutEnd = value;
                MarkDirty("LockoutEnd");
            }
        }
        private NpgsqlDateTime _lockoutEnd;

        [Required]
        [Column("password", 3)]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                MarkDirty("Password");
            }
        }
        private string _password;

        [Column("roles", 4)]
        public string[] Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                MarkDirty("Roles");
            }
        }
        private string[] _roles;

        [Required]
        [Column("tstarr", 11)]
        public int[] Tstarr
        {
            get => _tstarr;
            set
            {
                _tstarr = value;
                MarkDirty("Tstarr");
            }
        }
        private int[] _tstarr;

        [Required]
        [Column("username", 2)]
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                MarkDirty("Username");
            }
        }
        private string _username;
    }

    [Table("\"__EFMigrationsHistory\"")]
    public partial class EfMigrationsHistory : BaseEntity
    {
        [PrimaryKey]
        [Required]
        [Length(150)]
        [Column("migration_id", 1)]
        public string MigrationId
        {
            get => _migrationId;
            set
            {
                _migrationId = value;
                MarkDirty("MigrationId");
            }
        }
        private string _migrationId;

        [Required]
        [Length(32)]
        [Column("product_version", 2)]
        public string ProductVersion
        {
            get => _productVersion;
            set
            {
                _productVersion = value;
                MarkDirty("ProductVersion");
            }
        }
        private string _productVersion;
    }

    [Table("color_identity")]
    public partial class ColorIdentity : BaseEntity
    {
        [Column("color_id", 1)]
        public int ColorId
        {
            get => _colorId;
            set
            {
                _colorId = value;
                MarkDirty("ColorId");
            }
        }
        private int _colorId;

        [Required]
        [Column("color_name", 2)]
        public string ColorName
        {
            get => _colorName;
            set
            {
                _colorName = value;
                MarkDirty("ColorName");
            }
        }
        private string _colorName;

        [Required]
        [Column("mood", 3)]
        public string Mood
        {
            get => _mood;
            set
            {
                _mood = value;
                MarkDirty("Mood");
            }
        }
        private string _mood;
    }

    [Table("identity_device_code")]
    public partial class IdentityDeviceCode : BaseEntity
    {
        [Required]
        [Length(200)]
        [Column("client_id", 4)]
        public string ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                MarkDirty("ClientId");
            }
        }
        private string _clientId;

        [Required]
        [Column("creation_time", 5)]
        public NpgsqlDateTime CreationTime
        {
            get => _creationTime;
            set
            {
                _creationTime = value;
                MarkDirty("CreationTime");
            }
        }
        private NpgsqlDateTime _creationTime;

        [Required]
        [Length(50000)]
        [Column("data", 7)]
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                MarkDirty("Data");
            }
        }
        private string _data;

        [Required]
        [Length(200)]
        [UniqueIndex("ix_identity_device_code_device_code")]
        [Column("device_code", 2)]
        public string DeviceCode
        {
            get => _deviceCode;
            set
            {
                _deviceCode = value;
                MarkDirty("DeviceCode");
            }
        }
        private string _deviceCode;

        [Required]
        [Column("expiration", 6)]
        public NpgsqlDateTime Expiration
        {
            get => _expiration;
            set
            {
                _expiration = value;
                MarkDirty("Expiration");
            }
        }
        private NpgsqlDateTime _expiration;

        [Required]
        [Length(200)]
        [Column("subject_id", 3)]
        public string SubjectId
        {
            get => _subjectId;
            set
            {
                _subjectId = value;
                MarkDirty("SubjectId");
            }
        }
        private string _subjectId;

        [PrimaryKey]
        [Required]
        [Length(200)]
        [Column("user_code", 1)]
        public string UserCode
        {
            get => _userCode;
            set
            {
                _userCode = value;
                MarkDirty("UserCode");
            }
        }
        private string _userCode;
    }

    [Table("identity_persisted_grant")]
    public partial class IdentityPersistedGrant : BaseEntity
    {
        [Required]
        [Length(200)]
        [Column("client_id", 4)]
        public string ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                MarkDirty("ClientId");
            }
        }
        private string _clientId;

        [Required]
        [Column("creation_time", 5)]
        public NpgsqlDateTime CreationTime
        {
            get => _creationTime;
            set
            {
                _creationTime = value;
                MarkDirty("CreationTime");
            }
        }
        private NpgsqlDateTime _creationTime;

        [Required]
        [Length(50000)]
        [Column("data", 7)]
        public string Data
        {
            get => _data;
            set
            {
                _data = value;
                MarkDirty("Data");
            }
        }
        private string _data;

        [Required]
        [Column("expiration", 6)]
        public NpgsqlDateTime Expiration
        {
            get => _expiration;
            set
            {
                _expiration = value;
                MarkDirty("Expiration");
            }
        }
        private NpgsqlDateTime _expiration;

        [PrimaryKey]
        [Required]
        [Length(200)]
        [Column("key", 1)]
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                MarkDirty("Key");
            }
        }
        private string _key;

        [Required]
        [Length(200)]
        [Column("subject_id", 3)]
        public string SubjectId
        {
            get => _subjectId;
            set
            {
                _subjectId = value;
                MarkDirty("SubjectId");
            }
        }
        private string _subjectId;

        [Required]
        [Length(50)]
        [Column("type", 2)]
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                MarkDirty("Type");
            }
        }
        private string _type;
    }

    [Table("identity_role")]
    public partial class IdentityRole : BaseEntity
    {
        [Required]
        [Column("concurrency_stamp", 4)]
        public string ConcurrencyStamp
        {
            get => _concurrencyStamp;
            set
            {
                _concurrencyStamp = value;
                MarkDirty("ConcurrencyStamp");
            }
        }
        private string _concurrencyStamp;

        [PrimaryKey]
        [Required]
        [Column("id", 1)]
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private Guid _id;

        [Required]
        [Length(256)]
        [Column("name", 2)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkDirty("Name");
            }
        }
        private string _name;

        [Required]
        [Length(256)]
        [UniqueIndex("ix_identity_role_normalized_name")]
        [Column("normalized_name", 3)]
        public string NormalizedName
        {
            get => _normalizedName;
            set
            {
                _normalizedName = value;
                MarkDirty("NormalizedName");
            }
        }
        private string _normalizedName;
    }

    [Table("identity_role_claim")]
    public partial class IdentityRoleClaim : BaseEntity
    {
        [Required]
        [Column("claim_type", 3)]
        public string ClaimType
        {
            get => _claimType;
            set
            {
                _claimType = value;
                MarkDirty("ClaimType");
            }
        }
        private string _claimType;

        [Required]
        [Column("claim_value", 4)]
        public string ClaimValue
        {
            get => _claimValue;
            set
            {
                _claimValue = value;
                MarkDirty("ClaimValue");
            }
        }
        private string _claimValue;

        [PrimaryKey]
        [Column("id", 1)]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private int _id;

        [Required]
        [Column("role_id", 2)]
        public Guid RoleId
        {
            get => _roleId;
            set
            {
                _roleId = value;
                MarkDirty("RoleId");
            }
        }
        private Guid _roleId;
    }

    [Table("identity_user")]
    public partial class IdentityUser : BaseEntity
    {
        [Required]
        [Column("access_failed_count", 15)]
        public int AccessFailedCount
        {
            get => _accessFailedCount;
            set
            {
                _accessFailedCount = value;
                MarkDirty("AccessFailedCount");
            }
        }
        private int _accessFailedCount;

        [Required]
        [Column("concurrency_stamp", 9)]
        public string ConcurrencyStamp
        {
            get => _concurrencyStamp;
            set
            {
                _concurrencyStamp = value;
                MarkDirty("ConcurrencyStamp");
            }
        }
        private string _concurrencyStamp;

        [Required]
        [Length(256)]
        [Column("email", 4)]
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                MarkDirty("Email");
            }
        }
        private string _email;

        [Required]
        [Column("email_confirmed", 6)]
        public bool EmailConfirmed
        {
            get => _emailConfirmed;
            set
            {
                _emailConfirmed = value;
                MarkDirty("EmailConfirmed");
            }
        }
        private bool _emailConfirmed;

        [PrimaryKey]
        [Required]
        [Column("id", 1)]
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private Guid _id;

        [Required]
        [Column("lockout_enabled", 14)]
        public bool LockoutEnabled
        {
            get => _lockoutEnabled;
            set
            {
                _lockoutEnabled = value;
                MarkDirty("LockoutEnabled");
            }
        }
        private bool _lockoutEnabled;

        [Required]
        [Column("lockout_end", 13)]
        public DateTimeOffset LockoutEnd
        {
            get => _lockoutEnd;
            set
            {
                _lockoutEnd = value;
                MarkDirty("LockoutEnd");
            }
        }
        private DateTimeOffset _lockoutEnd;

        [Required]
        [Length(256)]
        [Column("normalized_email", 5)]
        public string NormalizedEmail
        {
            get => _normalizedEmail;
            set
            {
                _normalizedEmail = value;
                MarkDirty("NormalizedEmail");
            }
        }
        private string _normalizedEmail;

        [Required]
        [Length(256)]
        [UniqueIndex("ix_identity_user_normalized_user_name")]
        [Column("normalized_user_name", 3)]
        public string NormalizedUserName
        {
            get => _normalizedUserName;
            set
            {
                _normalizedUserName = value;
                MarkDirty("NormalizedUserName");
            }
        }
        private string _normalizedUserName;

        [Required]
        [Column("password_hash", 7)]
        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                _passwordHash = value;
                MarkDirty("PasswordHash");
            }
        }
        private string _passwordHash;

        [Required]
        [Column("phone_number", 10)]
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                MarkDirty("PhoneNumber");
            }
        }
        private string _phoneNumber;

        [Required]
        [Column("phone_number_confirmed", 11)]
        public bool PhoneNumberConfirmed
        {
            get => _phoneNumberConfirmed;
            set
            {
                _phoneNumberConfirmed = value;
                MarkDirty("PhoneNumberConfirmed");
            }
        }
        private bool _phoneNumberConfirmed;

        [Required]
        [Column("security_stamp", 8)]
        public string SecurityStamp
        {
            get => _securityStamp;
            set
            {
                _securityStamp = value;
                MarkDirty("SecurityStamp");
            }
        }
        private string _securityStamp;

        [Required]
        [Column("two_factor_enabled", 12)]
        public bool TwoFactorEnabled
        {
            get => _twoFactorEnabled;
            set
            {
                _twoFactorEnabled = value;
                MarkDirty("TwoFactorEnabled");
            }
        }
        private bool _twoFactorEnabled;

        [Required]
        [Length(256)]
        [Column("user_name", 2)]
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                MarkDirty("UserName");
            }
        }
        private string _userName;
    }

    [Table("identity_user_claim")]
    public partial class IdentityUserClaim : BaseEntity
    {
        [Required]
        [Column("claim_type", 3)]
        public string ClaimType
        {
            get => _claimType;
            set
            {
                _claimType = value;
                MarkDirty("ClaimType");
            }
        }
        private string _claimType;

        [Required]
        [Column("claim_value", 4)]
        public string ClaimValue
        {
            get => _claimValue;
            set
            {
                _claimValue = value;
                MarkDirty("ClaimValue");
            }
        }
        private string _claimValue;

        [PrimaryKey]
        [Column("id", 1)]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private int _id;

        [Required]
        [Column("user_id", 2)]
        public Guid UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                MarkDirty("UserId");
            }
        }
        private Guid _userId;
    }

    [Table("identity_user_login")]
    public partial class IdentityUserLogin : BaseEntity
    {
        [PrimaryKey]
        [Required]
        [Length(128)]
        [Column("login_provider", 1)]
        public string LoginProvider
        {
            get => _loginProvider;
            set
            {
                _loginProvider = value;
                MarkDirty("LoginProvider");
            }
        }
        private string _loginProvider;

        [Required]
        [Column("provider_display_name", 3)]
        public string ProviderDisplayName
        {
            get => _providerDisplayName;
            set
            {
                _providerDisplayName = value;
                MarkDirty("ProviderDisplayName");
            }
        }
        private string _providerDisplayName;

        [PrimaryKey]
        [Required]
        [Length(128)]
        [Column("provider_key", 2)]
        public string ProviderKey
        {
            get => _providerKey;
            set
            {
                _providerKey = value;
                MarkDirty("ProviderKey");
            }
        }
        private string _providerKey;

        [Required]
        [Column("user_id", 4)]
        public Guid UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                MarkDirty("UserId");
            }
        }
        private Guid _userId;
    }

    [Table("identity_user_role")]
    public partial class IdentityUserRole : BaseEntity
    {
        [PrimaryKey]
        [Required]
        [Column("role_id", 2)]
        public Guid RoleId
        {
            get => _roleId;
            set
            {
                _roleId = value;
                MarkDirty("RoleId");
            }
        }
        private Guid _roleId;

        [PrimaryKey]
        [Required]
        [Column("user_id", 1)]
        public Guid UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                MarkDirty("UserId");
            }
        }
        private Guid _userId;
    }

    [Table("identity_user_token")]
    public partial class IdentityUserToken : BaseEntity
    {
        [PrimaryKey]
        [Required]
        [Length(128)]
        [Column("login_provider", 2)]
        public string LoginProvider
        {
            get => _loginProvider;
            set
            {
                _loginProvider = value;
                MarkDirty("LoginProvider");
            }
        }
        private string _loginProvider;

        [PrimaryKey]
        [Required]
        [Length(128)]
        [Column("name", 3)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkDirty("Name");
            }
        }
        private string _name;

        [PrimaryKey]
        [Required]
        [Column("user_id", 1)]
        public Guid UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                MarkDirty("UserId");
            }
        }
        private Guid _userId;

        [Required]
        [Column("value", 4)]
        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                MarkDirty("Value");
            }
        }
        private string _value;
    }

    [Table("product")]
    public partial class Product : BaseEntity
    {
        [Required]
        [Column("barcode", 3)]
        public string Barcode
        {
            get => _barcode;
            set
            {
                _barcode = value;
                MarkDirty("Barcode");
            }
        }
        private string _barcode;

        [Required]
        [Column("description", 4)]
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                MarkDirty("Description");
            }
        }
        private string _description;

        [PrimaryKey]
        [Column("id", 1)]
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private Guid _id;

        [Required]
        [Column("name", 2)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkDirty("Name");
            }
        }
        private string _name;

        [Required]
        [Column("rate", 5)]
        public decimal Rate
        {
            get => _rate;
            set
            {
                _rate = value;
                MarkDirty("Rate");
            }
        }
        private decimal _rate;
    }

    [View("relations")]
    public partial class Relations : BaseEntity
    {
        [Readonly]
        [Column("can_delete", 8)]
        public bool CanDelete { get; set; }

        [Readonly]
        [Column("can_insert", 6)]
        public bool CanInsert { get; set; }

        [Readonly]
        [Column("can_select", 5)]
        public bool CanSelect { get; set; }

        [Readonly]
        [Column("can_update", 7)]
        public bool CanUpdate { get; set; }

        [Readonly]
        [Column("relation_kind", 4)]
        public string RelationKind { get; set; }

        [Readonly]
        [Column("relation_name", 2)]
        public string RelationName { get; set; }

        [Readonly]
        [Column("relation_owner", 3)]
        public string RelationOwner { get; set; }

        [Readonly]
        [Column("schema_name", 1)]
        public string SchemaName { get; set; }
    }

    [Table("test")]
    public partial class Test : BaseEntity
    {
        [Column("active", 8)]
        public bool Active
        {
            get => _active;
            set
            {
                _active = value;
                MarkDirty("Active");
            }
        }
        private bool _active;

        [Required]
        [Column("date", 7)]
        public NpgsqlDate Date
        {
            get => _date;
            set
            {
                _date = value;
                MarkDirty("Date");
            }
        }
        private NpgsqlDate _date;

        [Required]
        [Column("date_time", 6)]
        public NpgsqlDateTime DateTime
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                MarkDirty("DateTime");
            }
        }
        private NpgsqlDateTime _dateTime;

        [PrimaryKey]
        [Column("id", 1)]
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private Guid _id;

        [Required]
        [Column("long_description", 3)]
        public string LongDescription
        {
            get => _longDescription;
            set
            {
                _longDescription = value;
                MarkDirty("LongDescription");
            }
        }
        private string _longDescription;

        [Required]
        [UniqueIndex("test_numeric_money_uindex")]
        [Column("money", 5)]
        public decimal Money
        {
            get => _money;
            set
            {
                _money = value;
                MarkDirty("Money");
            }
        }
        private decimal _money;

        [Required]
        [Length(6)]
        [UniqueIndex("test_text_uindex")]
        [Column("name", 2)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkDirty("Name");
            }
        }
        private string _name;

        [Required]
        [UniqueIndex("test_numeric_money_uindex")]
        [Column("quantity", 4)]
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                MarkDirty("Quantity");
            }
        }
        private decimal _quantity;
    }

    [Table("test_2")]
    public partial class Test2 : BaseEntity
    {
        [Required]
        [Column("date", 3)]
        public NpgsqlDateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                MarkDirty("Date");
            }
        }
        private NpgsqlDateTime _date;

        [Required]
        [Column("id", 1)]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private int _id;

        [Required]
        [UniqueIndex("test_2_name_uindex")]
        [Column("name", 2)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkDirty("Name");
            }
        }
        private string _name;
    }

    [Table("test_foreign")]
    public partial class TestForeign : BaseEntity
    {
        [Required]
        [Column("arr", 3)]
        public int[] Arr
        {
            get => _arr;
            set
            {
                _arr = value;
                MarkDirty("Arr");
            }
        }
        private int[] _arr;

        [Required]
        [Column("fk", 2)]
        public Guid Fk
        {
            get => _fk;
            set
            {
                _fk = value;
                MarkDirty("Fk");
            }
        }
        private Guid _fk;

        [PrimaryKey]
        [Column("id", 1)]
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private Guid _id;
    }

    [Table("todo_item")]
    public partial class TodoItem : BaseEntity
    {
        [Required]
        [Column("created", 9)]
        public NpgsqlDateTime Created
        {
            get => _created;
            set
            {
                _created = value;
                MarkDirty("Created");
            }
        }
        private NpgsqlDateTime _created;

        [Required]
        [Column("created_by", 8)]
        public Guid CreatedBy
        {
            get => _createdBy;
            set
            {
                _createdBy = value;
                MarkDirty("CreatedBy");
            }
        }
        private Guid _createdBy;

        [Required]
        [Column("done", 5)]
        public bool Done
        {
            get => _done;
            set
            {
                _done = value;
                MarkDirty("Done");
            }
        }
        private bool _done;

        [PrimaryKey]
        [Column("id", 1)]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private int _id;

        [Required]
        [Column("last_modified", 11)]
        public NpgsqlDateTime LastModified
        {
            get => _lastModified;
            set
            {
                _lastModified = value;
                MarkDirty("LastModified");
            }
        }
        private NpgsqlDateTime _lastModified;

        [Required]
        [Column("last_modified_by", 10)]
        public Guid LastModifiedBy
        {
            get => _lastModifiedBy;
            set
            {
                _lastModifiedBy = value;
                MarkDirty("LastModifiedBy");
            }
        }
        private Guid _lastModifiedBy;

        [Required]
        [Column("list_id", 2)]
        public int ListId
        {
            get => _listId;
            set
            {
                _listId = value;
                MarkDirty("ListId");
            }
        }
        private int _listId;

        [Required]
        [Column("note", 4)]
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                MarkDirty("Note");
            }
        }
        private string _note;

        [Required]
        [Column("priority", 7)]
        public int Priority
        {
            get => _priority;
            set
            {
                _priority = value;
                MarkDirty("Priority");
            }
        }
        private int _priority;

        [Required]
        [Column("reminder", 6)]
        public NpgsqlDateTime Reminder
        {
            get => _reminder;
            set
            {
                _reminder = value;
                MarkDirty("Reminder");
            }
        }
        private NpgsqlDateTime _reminder;

        [Required]
        [Length(200)]
        [Column("title", 3)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                MarkDirty("Title");
            }
        }
        private string _title;
    }

    [Table("todo_list")]
    public partial class TodoList : BaseEntity
    {
        [Required]
        [Column("color", 3)]
        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                MarkDirty("Color");
            }
        }
        private string _color;

        [Required]
        [Column("created", 5)]
        public NpgsqlDateTime Created
        {
            get => _created;
            set
            {
                _created = value;
                MarkDirty("Created");
            }
        }
        private NpgsqlDateTime _created;

        [Required]
        [Column("created_by", 4)]
        public Guid CreatedBy
        {
            get => _createdBy;
            set
            {
                _createdBy = value;
                MarkDirty("CreatedBy");
            }
        }
        private Guid _createdBy;

        [PrimaryKey]
        [Column("id", 1)]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private int _id;

        [Required]
        [Column("last_modified", 7)]
        public NpgsqlDateTime LastModified
        {
            get => _lastModified;
            set
            {
                _lastModified = value;
                MarkDirty("LastModified");
            }
        }
        private NpgsqlDateTime _lastModified;

        [Required]
        [Column("last_modified_by", 6)]
        public Guid LastModifiedBy
        {
            get => _lastModifiedBy;
            set
            {
                _lastModifiedBy = value;
                MarkDirty("LastModifiedBy");
            }
        }
        private Guid _lastModifiedBy;

        [Required]
        [Length(200)]
        [Column("title", 2)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                MarkDirty("Title");
            }
        }
        private string _title;
    }

    [Table("sch.relation_kind")]
    public partial class SchRelationKind : BaseEntity
    {
        [PrimaryKey]
        [Required]
        [Length(1)]
        [Column("id", 1)]
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private string _id;

        [Required]
        [Column("name", 2)]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                MarkDirty("Name");
            }
        }
        private string _name;
    }

    [View("whizz.foreign_keys")]
    public partial class WhizzForeignKeys : BaseEntity
    {
        [Readonly]
        [Column("column_name", 3)]
        public string ColumnName { get; set; }

        [Readonly]
        [Column("constraint_name", 4)]
        public string ConstraintName { get; set; }

        [Readonly]
        [Column("primary_key_column_name", 7)]
        public string PrimaryKeyColumnName { get; set; }

        [Readonly]
        [Column("primary_key_schema_name", 5)]
        public string PrimaryKeySchemaName { get; set; }

        [Readonly]
        [Column("primary_key_table_name", 6)]
        public string PrimaryKeyTableName { get; set; }

        [Readonly]
        [Column("schema_name", 1)]
        public string SchemaName { get; set; }

        [Readonly]
        [Column("table_name", 2)]
        public string TableName { get; set; }
    }

    [View("whizz.relations")]
    public partial class WhizzRelations : BaseEntity
    {
        [Readonly]
        [Column("can_delete", 8)]
        public bool CanDelete { get; set; }

        [Readonly]
        [Column("can_insert", 6)]
        public bool CanInsert { get; set; }

        [Readonly]
        [Column("can_select", 5)]
        public bool CanSelect { get; set; }

        [Readonly]
        [Column("can_update", 7)]
        public bool CanUpdate { get; set; }

        [Readonly]
        [Column("relation_kind", 4)]
        public string RelationKind { get; set; }

        [Readonly]
        [Column("relation_name", 2)]
        public string RelationName { get; set; }

        [Readonly]
        [Column("relation_owner", 3)]
        public string RelationOwner { get; set; }

        [Readonly]
        [Column("schema_name", 1)]
        public string SchemaName { get; set; }
    }

    [View("whizz.schemas")]
    public partial class WhizzSchemas : BaseEntity
    {
        [Readonly]
        [Column("schema_name", 1)]
        public string SchemaName { get; set; }

        [Readonly]
        [Column("schema_owner", 2)]
        public string SchemaOwner { get; set; }
    }

    [Table("whizz.transaction_entry")]
    public partial class WhizzTransactionEntry : BaseEntity
    {
        [Required]
        [Column("command", 2)]
        public string Command
        {
            get => _command;
            set
            {
                _command = value;
                MarkDirty("Command");
            }
        }
        private string _command;

        [Required]
        [Column("executed", 6)]
        public long Executed
        {
            get => _executed;
            set
            {
                _executed = value;
                MarkDirty("Executed");
            }
        }
        private long _executed;

        [Column("id", 1)]
        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                MarkDirty("Id");
            }
        }
        private long _id;

        [Required]
        [Column("primary_key", 4)]
        public JObject PrimaryKey
        {
            get => _primaryKey;
            set
            {
                _primaryKey = value;
                MarkDirty("PrimaryKey");
            }
        }
        private JObject _primaryKey;

        [Required]
        [Column("table_name", 3)]
        public string TableName
        {
            get => _tableName;
            set
            {
                _tableName = value;
                MarkDirty("TableName");
            }
        }
        private string _tableName;

        [Required]
        [Column("user_id", 5)]
        public Guid UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                MarkDirty("UserId");
            }
        }
        private Guid _userId;
    }

    [View("whizz.unique_indexes")]
    public partial class WhizzUniqueIndexes : BaseEntity
    {
        [Readonly]
        [Column("column_names", 4)]
        public string[] ColumnNames { get; set; }

        [Readonly]
        [Column("index_name", 3)]
        public string IndexName { get; set; }

        [Readonly]
        [Column("schema_name", 1)]
        public string SchemaName { get; set; }

        [Readonly]
        [Column("table_name", 2)]
        public string TableName { get; set; }
    }

}