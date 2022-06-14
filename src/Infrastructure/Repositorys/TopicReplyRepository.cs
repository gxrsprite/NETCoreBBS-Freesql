using NetCoreBBS.Entities;
using NetCoreBBS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;
using FreeSql;

namespace NetCoreBBS.Infrastructure.Repositorys
{
    public class TopicReplyRepository : Repository<TopicReply>, ITopicReplyRepository
    {
        private readonly DataContext _dbContext;
        public TopicReplyRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override ISelect<TopicReply> List(Expression<Func<TopicReply, bool>> predicate)
        {
            return _dbContext.TopicReplys.Include(r=>r.ReplyUser).Where(predicate);
        }
    }
}
