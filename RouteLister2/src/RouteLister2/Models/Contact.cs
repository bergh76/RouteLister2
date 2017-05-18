using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RouteLister2.Models
{
    public class Contact : IComparable
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}