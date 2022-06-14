using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBBS.Entities
{
    public class TopicNode:IEntity
    {
        [FreeSql.DataAnnotations.Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string NodeName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
