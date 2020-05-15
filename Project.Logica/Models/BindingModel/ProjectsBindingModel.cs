using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ProjectCore.Logica.Models.BindingModel
{
    public class ProjectsCreateBindingModel
    {
        [Required(ErrorMessage ="The field title is required")]
        [Display(Name ="Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The field details is required")]
        [Display(Name = "Details")]
        public string Details { get; set; }

        [Required(ErrorMessage = "The field Expected Completion Date is required")]
        [Display(Name = "Expected Completion Date")]
        public DateTime? ExpectedCompletionDate { get; set; }  

    }


    public class ProjectsEditBindingModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Details { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }

    }

}
