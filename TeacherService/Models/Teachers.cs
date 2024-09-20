using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherService.Models
{
    [Table("Teacher")]
    public class Teachers
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SubjectId { get; set; }
        public TimeOnly TimeFrom { get; set; }
        public TimeOnly TimeTo { get; set; }
        public int Days { get;set; }
        public int ModeOfTeachingId { get; set; }
        public double Fee { get;set;}
        public string Address {  get; set; }= string.Empty;
        public string PhoneNo { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string AadharNumber {  get; set; } = string.Empty;
        public byte[] Image { get; set; }
        public string CreatedBy {  get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime UpdatedOn { get; set; }
        public bool IsActive {  get; set; }
    }
}
