using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskApi.Models
{
    public class TaskModel
    {
        public Guid Id { get; private set; }
        public string Descricao { get; set; }

        public TaskModel(string descricao)
        {
            Id = Guid.NewGuid();
            Descricao = descricao;
        }
    }
}