using System;
using System.Collections.Generic;
using System.Text;

namespace AgileTrace.Entity
{
    public enum NoticeMsgOp
    {
        Eq = 0,
        Start = 1,
        End = 2,
        In = 3
    }

    public class NoticeRule : IEntity
    {
        public string Id { get; set; }

        public string AppId { get; set; }

        public string Level { get; set; }

        public string Topic { get; set; }

        public NoticeMsgOp MsgOp { get; set; }

        public string MsgPatten { get; set; }

        public string MailTo { get; set; }

        public bool Enabled { get; set; }
    }
}
