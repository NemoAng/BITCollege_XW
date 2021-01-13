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

namespace BITCollege_XW.Models
{
    /// <summary>
    /// GradePointState Model - to represent GradePointState table in database.
    /// </summary>
    public class GradePointState
    {
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

        //Represents many students contained by GradePointState.
        public virtual ICollection<Student> Student { get; set; }
    }

    /// <summary>
    /// SuspendedState Model - to represent SuspendedState table in database.
    /// </summary>
    public class SuspendedState : GradePointState
    {
        private SuspendedState suspendedState;
    }

    /// <summary>
    /// ProbationState Model - to represent ProbationState table in database.
    /// </summary>
    public class ProbationState : GradePointState
    {
        private ProbationState probationState;
    }


    /// <summary>
    /// RegularState Model - to represent RegularState table in database.
    /// </summary>
    public class RegularState : GradePointState
    {
        private RegularState regularState;
    }

    /// <summary>
    /// HonoursState Model - to represent HonoursState table in database.
    /// </summary>
    public class HonoursState : GradePointState
    {
        private HonoursState honoursState;
    }

    /// <summary>
    /// Student Model - to represent Student table in database.
    /// </summary>
    public class Student
    {
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
        [Display(Name = "Student\nNumber")]
        public long StudentNumber { get; set; }

        //First name length between 1 and 35.
        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "First\nName")]
        public string FirstName { get; set; }

        //Last name length between 1 and 35.
        [Required]
        [StringLength(35, MinimumLength = 1)]
        [Display(Name = "Last\nName")]
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

        //0 or 1 Student object.
        public virtual Course Course { get; set; }
    }

    /// <summary>
    /// Course Model - to represent Course table in database.
    /// </summary>
    public class Course
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
        public virtual ICollection<AcademicProgram> AcademicProgram { get; set; }

        //Registration collection.
        public virtual ICollection<Registration> Registration { get; set; }
    }

    /// <summary>
    /// GradedCourse Model - to represent GradedCourse table in database.
    /// </summary>
    public class GradedCourse: Course
    {
        [Required]
        [Display(Name = "Assignment Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public double AssignmentWeight { get; set; }

        [Required]
        [Display(Name = "Midterm Weight")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P2}")]
        public double MidtermWeight { get; set; }

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