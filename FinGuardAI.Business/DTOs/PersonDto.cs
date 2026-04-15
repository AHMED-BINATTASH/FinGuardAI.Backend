using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinGuardAI.DataAccess.DTOs
{
    public class PersonDto
    {

        public int Id { get; set; }
        public string NationalId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        //public PersonDto(int PersonID, string NationalID, string FirstName, string LastName, string Phone, string Email)
        //{
        //    this.Id = PersonID;
        //    this.NationalId = NationalID;
        //    this.FirstName = FirstName;
        //    this.LastName = LastName;

        //    this.Phone = Phone;
        //    this.Email = Email;
        //}

    }
}
