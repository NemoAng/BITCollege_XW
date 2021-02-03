/*******************************************************************
 * Name: XUNJIN WANG
 * Program: Business Information Technology
 * Course: ADEV-3008 Programming 3
 * Created: 1-12-2021
 * Updated: 1-12-2021
 * TODO:
 * - .
 *******************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility;
using BITCollege_XW.Data;
using BITCollege_XW.Models;
using System.Linq;

namespace BITCollege_XW.Models
{
    /// <summary>
    /// GradePointState Model - to represent GradePointState table in database.
    /// </summary>
    //public abstract class GradePointState
    public class GradePointState
    {
        protected static BITCollege_XWContext singletonDBContext = new BITCollege_XWContext();

        //Annotation to have database generate next available primary key.
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GradePointStateId { get; set; }

        //LowerLimit of GradePointState.
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        [Display(Name = "Lower Limit")]
        public double LowerLimit { get; set; }

        //UpperLimit of GradePointState.
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        [Display(Name = "Upper Limit")]
        public double UpperLimit { get; set; }

        //TuitionRateFactor of GradePointState.
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        [Display(Name = "Tuition Rate Factor")]
        public double TuitionRateFactor { get; set; }

        //Indicate GradePointState name.
        [Display(Name = "Grade Point State")]
        public string Description { 
            get
            {
                return StringHelper.SubString(this.GetType().Name, 'S');
            }
        }

        //Tuition Rate adjustment.
        public virtual double TuitionRateAdjustment(Student student) 
        {
            return 0;
        }

        //State change check.
        public virtual void StateChangeCheck(Student student)
        {

        }

        //Represents many students contained by GradePointState.
        public virtual ICollection<Student> Student { get; set; }
    }

    /// <summary>
    /// SuspendedState Model - to represent SuspendedState table in database.
    /// </summary>
    public class SuspendedState : GradePointState
    {
        private static SuspendedState suspendedState;

        private SuspendedState()
        {
            LowerLimit = 0.0;
            UpperLimit = 1.0;
            TuitionRateFactor = 1.1;
        }


        /// <summary>
        /// Get SuspendedState instance.
        /// </summary>
        /// <returns></returns>
        public static SuspendedState GetInstance()
        {
            if(suspendedState == null)
            {
                SuspendedState susState = singletonDBContext.SuspendedStates.SingleOrDefault();
                if (susState != null)
                {
                    suspendedState = susState;
                } 
                else
                {
                    suspendedState = new SuspendedState();
                    singletonDBContext.GradePointStates.Add(suspendedState);
                    singletonDBContext.SaveChanges();
                }
            }

            return suspendedState;
        }

        /// <summary>
        /// Tuition Rate adjustment.
        /// </summary>
        /// <param name="student">current student.</param>
        /// <returns></returns>
        public override double TuitionRateAdjustment(Student student)
        {
            if (student.GradePointAverage < 0.5)
            {
                return TuitionRateAdjustmentValue.ADJ_P005;
            }    
            else if(student.GradePointAverage < 0.75)
            {
                return TuitionRateAdjustmentValue.ADJ_P002;
            }
            else
            {
                return TuitionRateAdjustmentValue.ADJ_NULL;
            }       
        }

        /// <summary>
        /// State change check.
        /// </summary>
        /// <param name="student">current student</param>
        public override void StateChangeCheck(Student student)
        {
            if(student.GradePointAverage > UpperLimit)
            {
                student.GradePointStateId = ProbationState.GetInstance().GradePointStateId;

                //State check chain
                ProbationState.GetInstance().StateChangeCheck(student);
            }
        }
    }

    /// <summary>
    /// ProbationState Model - to represent ProbationState table in database.
    /// </summary>
    public class ProbationState : GradePointState
    {
        private static ProbationState probationState;

        private ProbationState()
        {
            LowerLimit = 1.0;
            UpperLimit = 2.0;
            TuitionRateFactor = 1.075;
        }

        /// <summary>
        /// Get a probation state instance
        /// </summary>
        /// <returns></returns>
        public static ProbationState GetInstance()
        {
            if (probationState == null)
            {
                ProbationState proState = singletonDBContext.ProbationStates.SingleOrDefault();
                if (proState != null)
                {
                    probationState = proState;
                }
                else
                {
                    probationState = new ProbationState();
                    singletonDBContext.GradePointStates.Add(probationState);
                    singletonDBContext.SaveChanges();
                }
            }

            return probationState;
        }

        public override double TuitionRateAdjustment(Student student)
        {
            int courseNumCompleted = 0;

            List<Registration> registrations = singletonDBContext.Registrations.ToList<Registration>();

            foreach (var reg in registrations)
            {
                if (reg.Grade != null)
                {
                    courseNumCompleted++;
                }
            }

            if (courseNumCompleted >= (int)CourseFinished.FIVE)
            {
                return TuitionRateAdjustmentValue.ADJ_M004;//discussion???
            }
            return TuitionRateAdjustmentValue.ADJ_NULL;
        }

        /// <summary>
        /// State change check
        /// </summary>
        /// <param name="student">current student.</param>
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage > UpperLimit)
            {
                student.GradePointStateId = RegularState.GetInstance().GradePointStateId;

                //State check chain
                RegularState.GetInstance().StateChangeCheck(student);
            }
            else if (student.GradePointAverage < LowerLimit)
            {
                student.GradePointStateId = SuspendedState.GetInstance().GradePointStateId;

                //State check chain
                SuspendedState.GetInstance().StateChangeCheck(student);
            }
        }
    }


    /// <summary>
    /// RegularState Model - to represent RegularState table in database.
    /// </summary>
    public class RegularState : GradePointState
    {
        private static RegularState regularState;

        private RegularState()
        {
            LowerLimit = 2.0;
            UpperLimit = 3.7;
            TuitionRateFactor = 1.0;
        }

        /// <summary>
        /// Get regular state instance
        /// </summary>
        /// <returns></returns>
        public static RegularState GetInstance()
        {
            if (regularState == null)
            {
                RegularState regState = singletonDBContext.RegularStates.SingleOrDefault();
                if (regState != null)
                {
                    regularState = regState;
                }
                else
                {
                    regularState = new RegularState();
                    singletonDBContext.GradePointStates.Add(regularState);
                    singletonDBContext.SaveChanges();
                }
            }

            return regularState;
        }

        public override double TuitionRateAdjustment(Student student)
        {
            return TuitionRateAdjustmentValue.ADJ_NULL;
        }

        /// <summary>
        /// State change check
        /// </summary>
        /// <param name="student">current student.</param>
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage > UpperLimit)
            {
                student.GradePointStateId = HonoursState.GetInstance().GradePointStateId;

                //State check chain
                HonoursState.GetInstance().StateChangeCheck(student);
            }
            else if (student.GradePointAverage < LowerLimit)
            {
                student.GradePointStateId = ProbationState.GetInstance().GradePointStateId;

                //State check chain
                ProbationState.GetInstance().StateChangeCheck(student);
            }
        }
    }

    /// <summary>
    /// HonoursState Model - to represent HonoursState table in database.
    /// </summary>
    public class HonoursState : GradePointState
    {
        private static HonoursState honoursState;

        private HonoursState()
        {
            LowerLimit = 3.7;
            UpperLimit = 4.5;
            TuitionRateFactor = 0.9;
        }

        public static HonoursState GetInstance()
        {
            if (honoursState == null)
            {
                HonoursState horState = singletonDBContext.HonoursStates.SingleOrDefault();
                if (horState != null)
                {
                    honoursState = horState;
                }
                else
                {
                    honoursState = new HonoursState();
                    singletonDBContext.GradePointStates.Add(honoursState);
                    singletonDBContext.SaveChanges();
                }
            }

            return honoursState;
        }

        public override double TuitionRateAdjustment(Student student)
        {
            double discount = 0.0;
            int courseNumCompleted = 0;

            List<Registration> registrations = singletonDBContext.Registrations.ToList<Registration>();

            foreach (var reg in registrations)
            {
                if (reg.Grade != null)
                {
                    courseNumCompleted++;
                }
            }

            if (courseNumCompleted >= (int)CourseFinished.FIVE)
            {
                discount += TuitionRateAdjustmentValue.ADJ_M015;
                if (student.GradePointAverage > 4.25)
                {
                    discount += TuitionRateAdjustmentValue.ADJ_M002;
                }
            }
            else
            {
                if (student.GradePointAverage > 4.25)
                {
                    discount += TuitionRateAdjustmentValue.ADJ_M002;
                }
            }
            return discount;
        }

        /// <summary>
        /// State change check
        /// </summary>
        /// <param name="student">current student.</param>
        public override void StateChangeCheck(Student student)
        {
            if (student.GradePointAverage < LowerLimit)
            {
                student.GradePointStateId = RegularState.GetInstance().GradePointStateId;

                //State check chain
                RegularState.GetInstance().StateChangeCheck(student);
            }
        }
    }

    /// <summary>
    /// Student Model - to represent Student table in database.
    /// </summary>
    public class Student
    {
        //database context.
        BITCollege_XWContext db = new BITCollege_XWContext();

        //Annotation to have database generate next available primary key.
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int StudentID { get; set; }

        //Foreign key to GradePointState.
        [Required]
        [ForeignKey("GradePointState")]
        public int GradePointStateId { get; set; }

        //Foreign key to AcademicProgram.
        [ForeignKey("AcademicProgram")]
        public int? AcademicProgramId { get; set; }

        //StudentNumber is between 10000000 and 99999999.
        [Required]
        [Range(10000000, 99999999, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Display(Name = "Student Number")]
        public long StudentNumber { get; set; }

        //First name length between 1 and 35.
        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //Last name length between 1 and 35.
        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //Address length between 1 and 35.
        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string Address { get; set; }

        //City length between 1 and 35.
        [Required]
        [StringLength(35, MinimumLength = 1)]
        public string City { get; set; }

        //Province limited to AB,BC,MB,NB,NL,NS,NT,NU,ON,PE,QC,SK,YT.
        [Required]
        [RegularExpression("[A-Z][A-Z]", ErrorMessage = "Must be in AB,BC,MB,NB,NL,NS,NT,NU,ON,PE,QC,SK,YT.")]
        public string Province { get; set; }

        //Canada post code.
        [Required]
        [RegularExpression("[^dfioquwzDFIOQUWZ0-9]([0-9][^dfioquDFIOQU0-9])[ ]?([0-9][^dfioquDFIOQU0-9])[0-9]", 
            ErrorMessage = "Canadian Postal codes do not include the letters D, F, I, O, Q or U, and the first position also does not make use of the letters W or Z.")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        //Date created on "MM/dd/yyyy" format.
        [Required]
        [Display(Name = "Date Created")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateCreated { get; set; }

        //Average Grade Point.
        [Display(Name = "Grade Point Average")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        [Range(0, 4.5, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? GradePointAverage { get; set; }

        //Outstanding fees in currency format.
        [Required]
        [Display(Name = "Outstanding Fees")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        public double OutstandingFees { get; set; }

        public string Notes { get; set; }

        //Formated student name.
        [Display(Name = "Name")]
        public string FullName {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        //Formated student address.
        [Display(Name = "Address")]
        public string FullAddress
        {
            get
            {
                return String.Format("{0} {1} {2}, {3}", Address, City, Province, PostalCode);
            }
        }

        //0 or 1 GradePointState object.
        public virtual GradePointState GradePointState { get; set; }

        //0 or 1 AcademicProgram object.
        public virtual AcademicProgram AcademicProgram { get; set; }

        //More than 1 Registration object.
        public virtual ICollection<Registration> Registration { get; set; }

        //Change Grade Point State.
        public void ChangeState()
        {
            GradePointState gradePointState = db.GradePointStates.Find(this.GradePointStateId);
            gradePointState.StateChangeCheck(this);
        }
    }

    /// <summary>
    /// AcademicProgram Model - to represent AcademicProgram table in database.
    /// </summary>
    public class AcademicProgram
    {
        //Annotation to have database generate next available primary key.
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AcademicProgramId { get; set; }

        //Indicate program acronym.
        [Required]
        [Display(Name = "Program")]
        public string ProgramAcronym { get; set; }

        //Program description.
        [Required]
        [Display(Name = "Program Name")]
        public string Description { get; set; }

        //Student collection.
        public virtual ICollection<Student> Student { get; set; }

        //Course collection.
        public virtual ICollection<Course> Course { get; set; }
    }

    /// <summary>
    /// Registration Model - to represent Registration table in database.
    /// </summary>
    public class Registration
    {
        //Annotation to have database generate next available primary key.
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RegistrationId { get; set; }

        //Foreign key to Student.
        [ForeignKey("Student")]
        public int StudentId { get; set; }

        //Foreign key to Course.
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        //Indicates Registration number.
        [Required]
        [Display(Name = "Registration Number")]
        public long RegistrationNumber { get; set; }

        //Indicates Registration date with format 'MM/dd/yyyy'.
        [Required]
        [Display(Name = "Registration Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        // Applying DisplayFormatAttribute
        // Display the text [Ungraded] when the data field is empty.
        // Also, convert empty string to null for storing.
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "[Ungraded]")]
        [Range(0, 1, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Grade { get; set; }

        public string Notes { get; set; }

        //0 or 1 Student object.
        public virtual Student Student { get; set; }

        //0 or 1 Course object.
        public virtual Course Course { get; set; }
    }

    /// <summary>
    /// Course Model - to represent Course table in database.
    /// </summary>
    public abstract class Course
    {
        //Annotation to have database generate next available primary key.
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        //Foreign key to AcademicProgram.
        [ForeignKey("AcademicProgram")]
        public int AcademicProgramId { get; set; }

        //Indicates course number.
        [Required]
        [Display(Name = "Course Number")]
        public string CourseNumber { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        [Display(Name = "Credit Hours")]
        public double CreditHours { get; set; }

        //Final weight in currency.
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C2}")]
        [Display(Name = "Tuition Amount")]
        public double TuitionAmount { get; set; }

        [Display(Name = "Course Type")]
        public string CourseType {
            get
            {
                return StringHelper.SubString(this.GetType().Name, 'C');
            }
        }

        public string Notes { get; set; }

        //AcademicProgram collection.
        public virtual AcademicProgram AcademicProgram { get; set; }

        //Registration collection.
        public virtual ICollection<Registration> Registration { get; set; }
    }

    /// <summary>
    /// GradedCourse Model - to represent GradedCourse table in database.
    /// </summary>
    public class GradedCourse: Course
    {
        //Assignment weight in percentage.
        [Required]
        [Display(Name = "Assignment Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public double AssignmentWeight { get; set; }

        //Midterm weight in percentage.
        [Required]
        [Display(Name = "Midterm Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public double MidtermWeight { get; set; }

        //Final weight in percentage.
        [Required]
        [Display(Name = "Final Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public double FinalWeight { get; set; }
    }

    /// <summary>
    /// AuditCourse Model - to represent AuditCourse table in database.
    /// </summary>
    public class AuditCourse : Course
    {
        
    }

    /// <summary>
    /// MasteryCourse Model - to represent MasteryCourse table in database.
    /// </summary>
    public class MasteryCourse : Course
    {
        [Required]
        [Display(Name = "Maximum Attempts")]
        public int MaximumAttempts { get; set; }
    }
}