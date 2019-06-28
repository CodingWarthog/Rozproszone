/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     25.06.2019 19:35:21                          */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Department') and o.name = 'FK_DEPARTME_DEPARTMEN_CITY')
alter table Department
   drop constraint FK_DEPARTME_DEPARTMEN_CITY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Employee') and o.name = 'FK_EMPLOYEE_EMPLOYEE _DEPARTME')
alter table Employee
   drop constraint "FK_EMPLOYEE_EMPLOYEE _DEPARTME"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Employee') and o.name = 'FK_EMPLOYEE_HAS SALAR_PAYMENT')
alter table Employee
   drop constraint "FK_EMPLOYEE_HAS SALAR_PAYMENT"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('WorkBonusAssignment') and o.name = 'FK_WORKBONU_HAS ASSIG_EMPLOYEE')
alter table WorkBonusAssignment
   drop constraint "FK_WORKBONU_HAS ASSIG_EMPLOYEE"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('WorkBonusAssignment') and o.name = 'FK_WORKBONU_REFERENCE_WORKBONU')
alter table WorkBonusAssignment
   drop constraint FK_WORKBONU_REFERENCE_WORKBONU
go

if exists (select 1
            from  sysobjects
           where  id = object_id('City')
            and   type = 'U')
   drop table City
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Department')
            and   type = 'U')
   drop table Department
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Employee')
            and   type = 'U')
   drop table Employee
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Payment')
            and   type = 'U')
   drop table Payment
go

if exists (select 1
            from  sysobjects
           where  id = object_id('WorkBonus')
            and   type = 'U')
   drop table WorkBonus
go

if exists (select 1
            from  sysobjects
           where  id = object_id('WorkBonusAssignment')
            and   type = 'U')
   drop table WorkBonusAssignment
go

/*==============================================================*/
/* Table: City                                                  */
/*==============================================================*/
create table City (
   Id                   int                  identity,
   CityName             varchar(50)          null,
   constraint PK_CITY primary key (Id)
)
go

/*==============================================================*/
/* Table: Department                                            */
/*==============================================================*/
create table Department (
   Id                   int                  not null,
   Name                 varchar(50)          null,
   AreaId               int                  null,
   constraint PK_DEPARTMENT primary key (Id)
)
go

/*==============================================================*/
/* Table: Employee                                              */
/*==============================================================*/
create table Employee (
   Id                   int                  not null,
   Firstname            varchar(50)          null,
   Lastname             varchar(50)          null,
   Age                  varchar(50)          null,
   Speciality           varchar(50)          null,
   DepartmentId         int                  null,
   PaymentId            int                  null,
   constraint PK_EMPLOYEE primary key (Id)
)
go

/*==============================================================*/
/* Table: Payment                                               */
/*==============================================================*/
create table Payment (
   Id                   int                  identity,
   MonthRate            int                  null,
   RateName             varchar(50)          null,
   constraint PK_PAYMENT primary key (Id)
)
go

/*==============================================================*/
/* Table: WorkBonus                                             */
/*==============================================================*/
create table WorkBonus (
   Id                   int                  identity,
   BonusName            varchar(50)          null,
   Amount               int                  null,
   constraint PK_WORKBONUS primary key (Id)
)
go

/*==============================================================*/
/* Table: WorkBonusAssignment                                   */
/*==============================================================*/
create table WorkBonusAssignment (
   Id                   int                  not null,
   AssignmentDate       datetime             null,
   EmployeeId           int                  null,
   WorkBonusId          int                  null,
   constraint PK_WORKBONUSASSIGNMENT primary key (Id)
)
go

alter table Department
   add constraint FK_DEPARTME_DEPARTMEN_CITY foreign key (AreaId)
      references City (Id)
go

alter table Employee
   add constraint "FK_EMPLOYEE_EMPLOYEE _DEPARTME" foreign key (DepartmentId)
      references Department (Id)
go

alter table Employee
   add constraint "FK_EMPLOYEE_HAS SALAR_PAYMENT" foreign key (PaymentId)
      references Payment (Id)
go

alter table WorkBonusAssignment
   add constraint "FK_WORKBONU_HAS ASSIG_EMPLOYEE" foreign key (EmployeeId)
      references Employee (Id)
go

alter table WorkBonusAssignment
   add constraint FK_WORKBONU_REFERENCE_WORKBONU foreign key (WorkBonusId)
      references WorkBonus (Id)
go

