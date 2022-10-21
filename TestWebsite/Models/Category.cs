using System.ComponentModel.DataAnnotations;

namespace TestWebsite.Models
{
    public enum Category
    {
        Lorem = 0,
        Ipsum = 1,
        Dolor = 2,
        Sit = 3,
        Amet = 4,
        [Display(Name = "Test field display override")]
        Test = 5,
    }
}