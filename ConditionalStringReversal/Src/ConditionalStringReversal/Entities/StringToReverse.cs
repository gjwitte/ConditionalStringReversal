using System.ComponentModel.DataAnnotations.Schema;

namespace ConditionalStringReversal.Entities
{
    public class StringToReverse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DataValue { get; set; }
    }
}
