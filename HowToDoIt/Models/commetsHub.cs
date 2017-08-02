using HowToDoIt.Models.Classes_for_Db;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HowToDoIt.Models
{
    public class commetsHub : Hub
    {
        public void OpenInstruction(int instructionId)
        {
            Groups.Add(Context.ConnectionId, instructionId.ToString());
        }


        public Task CreateComment(string instructionId, Comment data)
        {
            return Clients.Group(instructionId).createComment(data);
        }



    }
}