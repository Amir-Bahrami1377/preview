using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FormerUrban_Afta.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogFilters",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormName = table.Column<int>(type: "int", nullable: false),
                    AddStatus = table.Column<bool>(type: "bit", nullable: false),
                    UpdateStatus = table.Column<bool>(type: "bit", nullable: false),
                    GetStatus = table.Column<bool>(type: "bit", nullable: false),
                    DeleteStatus = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogFilters", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "AllowedIPRange",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IPRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedIPRange", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Apartman",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    radif = table.Column<int>(type: "int", nullable: false),
                    pelakabi = table.Column<int>(type: "int", nullable: false),
                    codeposti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_noesanad = table.Column<int>(type: "int", nullable: true),
                    noesanad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_vazsanad = table.Column<int>(type: "int", nullable: true),
                    vazsanad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_noemalekiyat = table.Column<int>(type: "int", nullable: true),
                    noemalekiyat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sabti = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MasahatKol = table.Column<double>(type: "float", nullable: true),
                    MasahatArse = table.Column<double>(type: "float", nullable: true),
                    sh_Darkhast = table.Column<long>(type: "bigint", nullable: true),
                    tafkiki = table.Column<int>(type: "int", nullable: true),
                    azFari = table.Column<int>(type: "int", nullable: true),
                    fari = table.Column<int>(type: "int", nullable: true),
                    asli = table.Column<int>(type: "int", nullable: true),
                    bakhsh = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    NoeSaze = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_NoeSaze = table.Column<int>(type: "int", nullable: true),
                    C_Jahat = table.Column<int>(type: "int", nullable: true),
                    jahat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartman", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditFilters",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditFilters", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Form = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlockedIPRange",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IPRange = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedIPRange", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "ControlMap",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    sh_Darkhast = table.Column<int>(type: "int", nullable: false),
                    masahat_s = table.Column<double>(type: "float", nullable: true),
                    masahat_m = table.Column<double>(type: "float", nullable: true),
                    masahat_e = table.Column<double>(type: "float", nullable: true),
                    masahat_b = table.Column<double>(type: "float", nullable: true),
                    C_NoeNama = table.Column<int>(type: "int", nullable: true),
                    NoeNama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_noesaghf = table.Column<int>(type: "int", nullable: true),
                    noesaghf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tarakom = table.Column<double>(type: "float", nullable: true),
                    satheshghal = table.Column<double>(type: "float", nullable: true),
                    NoeSaze = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_NoeSaze = table.Column<int>(type: "int", nullable: true),
                    TedadTabaghe = table.Column<double>(type: "float", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlMap", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Darkhast",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    shodarkhast = table.Column<int>(type: "int", nullable: false),
                    noedarkhast = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_noedarkhast = table.Column<int>(type: "int", nullable: false),
                    noemot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_noemot = table.Column<int>(type: "int", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    moteghazi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codeposti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_nosazi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeMeli = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Darkhast", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Dv_karbari",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    d_radif = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false),
                    mtable_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_tabagheh = table.Column<int>(type: "int", nullable: true),
                    tabagheh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_karbari = table.Column<int>(type: "int", nullable: true),
                    karbari = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_noeestefadeh = table.Column<int>(type: "int", nullable: true),
                    noeestefadeh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    masahat_k = table.Column<double>(type: "float", nullable: true),
                    c_noesakhteman = table.Column<int>(type: "int", nullable: true),
                    noesakhteman = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_noesazeh = table.Column<int>(type: "int", nullable: true),
                    noesazeh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_marhaleh = table.Column<int>(type: "int", nullable: true),
                    marhaleh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tarikhehdas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dv_karbari", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Dv_malekin",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    d_radif = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<int>(type: "int", nullable: false),
                    mtable_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_noemalek = table.Column<int>(type: "int", nullable: true),
                    noemalek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    father = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sh_sh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kodemeli = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mob = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sahm_a = table.Column<double>(type: "float", nullable: true),
                    dong_a = table.Column<double>(type: "float", nullable: true),
                    sahm_b = table.Column<double>(type: "float", nullable: true),
                    dong_b = table.Column<double>(type: "float", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArzeshArse = table.Column<double>(type: "float", nullable: true),
                    ArzeshAyan = table.Column<double>(type: "float", nullable: true),
                    meghdarsahmarse = table.Column<double>(type: "float", nullable: true),
                    meghdarsahmayan = table.Column<double>(type: "float", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dv_malekin", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Dv_savabegh",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    d_radif = table.Column<int>(type: "int", nullable: false),
                    mtable_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_noe = table.Column<int>(type: "int", nullable: true),
                    noe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sh_darkhast = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dv_savabegh", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Erja",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sh_darkhast = table.Column<int>(type: "int", nullable: true),
                    tarikh_darkhast = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_nodarkhast = table.Column<int>(type: "int", nullable: false),
                    noedarkhast = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_marhaleh = table.Column<int>(type: "int", nullable: true),
                    marhaleh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code_nosazi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shop = table.Column<double>(type: "float", nullable: true),
                    name_mot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tarikh_erja = table.Column<int>(type: "int", nullable: false),
                    ijadkonandeh_c = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ijadkonandeh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ersalkonandeh_c = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ersalkonandeh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    girandeh_c = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    girandeh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    flag = table.Column<bool>(type: "bit", nullable: true),
                    saat_erja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_vaziatErja = table.Column<int>(type: "int", nullable: true),
                    vaziatErja = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erja", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Estelam",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sh_Darkhast = table.Column<int>(type: "int", nullable: false),
                    Sh_Pasokh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tarikh_Pasokh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Dang_Enteghal = table.Column<double>(type: "float", nullable: false),
                    Kharidar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tozihat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    codeNoeMalekiat = table.Column<long>(type: "bigint", nullable: false),
                    NoeMalekiat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estelam", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "EventLogFilter",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MustLoginBeLogged = table.Column<bool>(type: "bit", nullable: false),
                    LogBarayeRaddeRamzeObour = table.Column<bool>(type: "bit", nullable: false),
                    LogBarayeGozarAzHaddeAstaneyeBohraneMomayezi = table.Column<bool>(type: "bit", nullable: false),
                    LogBarayeGozarAzHaddeAstaneyeHoshdareMomayezi = table.Column<bool>(type: "bit", nullable: false),
                    LogBarayeHarGooneTalasheEhrazeHoviateChandGaneyeKarbar = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogFilter", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "EventLogThreshold",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersLoginLogWarning = table.Column<int>(type: "int", nullable: false),
                    UsersLoginLogCritical = table.Column<int>(type: "int", nullable: false),
                    UsersActivityLogWarning = table.Column<int>(type: "int", nullable: false),
                    UsersActivityLogCritical = table.Column<int>(type: "int", nullable: false),
                    UsersAuditsLogWarning = table.Column<int>(type: "int", nullable: false),
                    UsersAuditsLogCritical = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUserLoginLogWarningSmsSent = table.Column<int>(type: "int", nullable: false),
                    IsUserActivityLogWarningSmsSent = table.Column<int>(type: "int", nullable: false),
                    IsAuditsLogWarningSmsSent = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogThreshold", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Expert",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestNumber = table.Column<int>(type: "int", nullable: false),
                    DateVisit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expert", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: true),
                    shod = table.Column<int>(type: "int", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tarikh = table.Column<int>(type: "int", nullable: true),
                    saat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sharh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name_karbar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    name_form = table.Column<int>(type: "int", nullable: false),
                    noeamal = table.Column<int>(type: "int", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CNosazi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "LogSMS",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextSMS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileSMS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusSMS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeSMS = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSMS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Melk",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    radif = table.Column<int>(type: "int", nullable: false),
                    pelakabi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    codeposti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    masahat_s = table.Column<double>(type: "float", nullable: true),
                    masahat_m = table.Column<double>(type: "float", nullable: true),
                    masahat_e = table.Column<double>(type: "float", nullable: true),
                    masahat_b = table.Column<double>(type: "float", nullable: true),
                    c_vazsanad = table.Column<int>(type: "int", nullable: true),
                    vazsanad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_noesanad = table.Column<int>(type: "int", nullable: true),
                    noesanad = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sabti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_vazmelk = table.Column<int>(type: "int", nullable: true),
                    vazmelk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_mahdodeh = table.Column<int>(type: "int", nullable: true),
                    mahdodeh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_noemelk = table.Column<int>(type: "int", nullable: true),
                    noemelk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    c_marhaleh = table.Column<int>(type: "int", nullable: true),
                    marhaleh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sh_Darkhast = table.Column<long>(type: "bigint", nullable: true),
                    tafkiki = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    azFari = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fari = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    asli = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bakhsh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    ArzeshArse = table.Column<double>(type: "float", nullable: true),
                    C_karbariAsli = table.Column<int>(type: "int", nullable: true),
                    KarbariAsli = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    utmx = table.Column<double>(type: "float", nullable: true),
                    utmy = table.Column<double>(type: "float", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Melk", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Parvandeh",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<double>(type: "float", nullable: false),
                    mantaghe = table.Column<int>(type: "int", nullable: false),
                    hoze = table.Column<int>(type: "int", nullable: false),
                    blok = table.Column<int>(type: "int", nullable: false),
                    shomelk = table.Column<int>(type: "int", nullable: false),
                    sakhteman = table.Column<int>(type: "int", nullable: false),
                    apar = table.Column<int>(type: "int", nullable: false),
                    senfi = table.Column<int>(type: "int", nullable: false),
                    idparent = table.Column<int>(type: "int", nullable: false),
                    code_tree = table.Column<int>(type: "int", nullable: false),
                    sws = table.Column<bool>(type: "bit", nullable: false),
                    Formol = table.Column<int>(type: "int", nullable: true),
                    codeN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    locked = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parvandeh", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Parvaneh",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    sh_darkhast = table.Column<int>(type: "int", nullable: false),
                    sho_parvaneh = table.Column<double>(type: "float", nullable: false),
                    c_noeParvaneh = table.Column<int>(type: "int", nullable: true),
                    noe_parvaneh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tarikh_parvaneh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    masahat_m_s_tarakom = table.Column<double>(type: "float", nullable: true),
                    masahat_m_esh_zamin = table.Column<double>(type: "float", nullable: true),
                    tarikh_end_amaliat_s = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sho_bimenameh = table.Column<int>(type: "int", nullable: false),
                    tarikh_e_bimeh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tozihat_parvaneh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parvaneh", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "RoleRestrictions",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleRestrictions", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Sakhteman",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shop = table.Column<int>(type: "int", nullable: false),
                    radif = table.Column<int>(type: "int", nullable: false),
                    masahatkol = table.Column<double>(type: "float", nullable: true),
                    c_noenama = table.Column<int>(type: "int", nullable: true),
                    noenama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_noesaghf = table.Column<int>(type: "int", nullable: true),
                    noesaghf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tarakom = table.Column<double>(type: "float", nullable: true),
                    satheshghal = table.Column<double>(type: "float", nullable: true),
                    c_marhaleh = table.Column<int>(type: "int", nullable: true),
                    marhaleh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sh_Darkhast = table.Column<long>(type: "bigint", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    c_NoeSakhteman = table.Column<int>(type: "int", nullable: true),
                    NoeSaze = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    c_NoeSaze = table.Column<int>(type: "int", nullable: true),
                    TarikhEhdas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArzeshAyan = table.Column<double>(type: "float", nullable: true),
                    NoeSakhteman = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TedadTabaghe = table.Column<double>(type: "float", nullable: true),
                    MasahatZirbana = table.Column<double>(type: "float", nullable: true),
                    MasahatArse = table.Column<double>(type: "float", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sakhteman", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Tarifha",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sms_user = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sms_pass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sms_shomare = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RetryLoginCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaximumSessions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KhatemeSessionAfterMinute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaximumAccessDenied = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestRateLimitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordLength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifha", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "UserLogined",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoginDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LogoutDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsViewer = table.Column<bool>(type: "bit", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogined", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "UserSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSession", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeakPassword",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeakPasswordText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeakPassword", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_RolePermission_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    Identity = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Hashed = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_UserPermission_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_UserId",
                table: "UserPermission",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogFilters");

            migrationBuilder.DropTable(
                name: "AllowedIPRange");

            migrationBuilder.DropTable(
                name: "Apartman");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditFilters");

            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "BlockedIPRange");

            migrationBuilder.DropTable(
                name: "ControlMap");

            migrationBuilder.DropTable(
                name: "Darkhast");

            migrationBuilder.DropTable(
                name: "Dv_karbari");

            migrationBuilder.DropTable(
                name: "Dv_malekin");

            migrationBuilder.DropTable(
                name: "Dv_savabegh");

            migrationBuilder.DropTable(
                name: "Erja");

            migrationBuilder.DropTable(
                name: "Estelam");

            migrationBuilder.DropTable(
                name: "EventLogFilter");

            migrationBuilder.DropTable(
                name: "EventLogThreshold");

            migrationBuilder.DropTable(
                name: "Expert");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "LogSMS");

            migrationBuilder.DropTable(
                name: "Melk");

            migrationBuilder.DropTable(
                name: "Parvandeh");

            migrationBuilder.DropTable(
                name: "Parvaneh");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "RoleRestrictions");

            migrationBuilder.DropTable(
                name: "Sakhteman");

            migrationBuilder.DropTable(
                name: "Tarifha");

            migrationBuilder.DropTable(
                name: "UserLogined");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropTable(
                name: "UserSession");

            migrationBuilder.DropTable(
                name: "WeakPassword");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
