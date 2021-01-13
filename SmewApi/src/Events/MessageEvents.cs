using System;
using Smew.Infrastructure;

namespace Smew.Events {
    public record PostMessageEvent (MessageMutation Body) : IReactiveEvent<MessageMutation>;
    public record GetMessageRequestEvent (MessageQuery Body) : IReactiveEvent<MessageQuery>;
    public record GetMessageResponseEvent (MessageEntity Body) : IReactiveEvent<MessageEntity>;
    public record MessageMutation (string Content, string AuthorUid) : MessageEntity (Content, AuthorUid);
    public record MessageQuery (string Content, string AuthorUid) : MessageEntity (Content, AuthorUid);
    public record MessageEntity (string Content, string AuthorUid, DateTime? createdDate = null);
}