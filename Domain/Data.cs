using System;
using System.Collections.Generic;
using System.Text;

namespace ServicoProducerConsumer.Domain
{
    /// <summary>
    /// Base de atributos para novas mensagens
    /// Id:         Identificador da requisição
    /// Value:      Conteúdo da mensagem
    /// TimeStamp:  Data/hora de geração da mensagem
    /// Guid:       Identificador único do micro-serviço
    /// </summary>
    public sealed class Data
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public Guid MicroServiceGuid { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
