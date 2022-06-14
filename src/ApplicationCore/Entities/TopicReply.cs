using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreBBS.Entities
{
    public class TopicReply:IEntity
    {
        [FreeSql.DataAnnotations.Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string ReplyUserId { get; set; }
        public User ReplyUser { get; set; }
        public string ReplyEmail { get; set; }
        public string ReplyContent { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
