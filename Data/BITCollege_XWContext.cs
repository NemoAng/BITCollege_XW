using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BITCollege_XW.Data
{
    public class BITCollege_XWContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BITCollege_XWContext() : base("name=BITCollege_XWContext")
        {
        }

        public System.Data.Entity.DbSet<BITCollege_XW.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<BITCollege_XW.Models.AcademicProgram> AcademicPrograms { get; set; }


        public System.Data.Entity.DbSet<BITCollege_XW.Models.Registration> Registrations { get; set; }


        public System.Data.Entity.DbSet<BITCollege_XW.Models.Course> Courses { get; set; }

        //GradedCourse dbset.
        public System.Data.Entity.DbSet<BITCollege_XW.Models.GradedCourse> GradedCourses { get; set; }

        //AuditCourse dbset.
        public System.Data.Entity.DbSet<BITCollege_XW.Models.AuditCourse> AuditCourses { get; set; }

        //MasteryCourse dbset.
        public System.Data.Entity.DbSet<BITCollege_XW.Models.MasteryCourse> MasteryCourses { get; set; }

        public System.Data.Entity.DbSet<BITCollege_XW.Models.GradePointState> GradePointStates { get; set; }

        //ProbationState dbset.
        public System.Data.Entity.DbSet<BITCollege_XW.Models.ProbationState> ProbationStates { get; set; }

        //SuspendedState dbset.
        public System.Data.Entity.DbSet<BITCollege_XW.Models.SuspendedState> SuspendedStates { get; set; }

        //RegularState dbset.
        public System.Data.Entity.DbSet<BITCollege_XW.Models.RegularState> RegularStates { get; set; }

        //HonoursState dbset.
        public System.Data.Entity.DbSet<BITCollege_XW.Models.HonoursState> HonoursStates { get; set; }
    }
}
