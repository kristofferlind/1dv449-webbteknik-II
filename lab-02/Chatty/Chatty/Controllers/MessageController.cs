using Chatty.App_Infrastructure;
using Chatty.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Chatty.Controllers
{
    public class MessageController : ApiController
    {
        IMessageRepository messageRepository;

        public MessageController()
        {
            messageRepository = new MessageRepository();
            messageRepository.OnSave += messageRepository_OnSave;
        }

        // GET api/message
        public IEnumerable<ChatMessage> Get()
        {
            return messageRepository.All();
        }


        [System.Web.Mvc.AsyncTimeout(60), Route("api/message/poll"), HttpGet]
        public async Task<IEnumerable<ChatMessage>> Poll()
        {
            await _updated.Task;
            return messageRepository.All();
        }

        static TaskCompletionSource<ChatMessage> _updated = new TaskCompletionSource<ChatMessage>();

        void messageRepository_OnSave(object sender, ChatMessageEventArgs e)
        {
            _updated.SetResult(e.ChatMessage);
            _updated = new TaskCompletionSource<ChatMessage>();
            GlobalHost.ConnectionManager.GetHubContext<ChatHub>().Clients.All.receiveMessage(e.ChatMessage);
        }

        // GET api/message/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/message
        public void Post(ChatMessage chatMessage)
        {
            messageRepository.Add(chatMessage);
        }

        // PUT api/message/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/message/5
        public void Delete(int id)
        {
        }
    }
}