using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TanfeethIntegration.Models
{
    
        [Table("RequestResponseLog")]
        public class RequestResponseLog
        {
            [Key]
            public int LogId { get; set; }

            public string Request { get; set; }
            public string Response { get; set; }
            public int ResponseStatusCode { get; set; }
            public DateTime Timestamp { get; set; }
        }
    
}
