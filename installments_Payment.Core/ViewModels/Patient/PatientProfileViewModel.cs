using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace installments_Payment.Core.ViewModels.Patient
{
    public class PatientProfileViewModel
    {
        public string FullName { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool DisableRequestButton { get; set; }

        public string SecondaryPhoneNumber { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public string NationalCardImagePath { get; set; }
    }

}
