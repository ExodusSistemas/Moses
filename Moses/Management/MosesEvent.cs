using System;
using System.Collections.Generic;
using System.Text;

namespace Moses.Management
{
    /// <summary>
    /// Classe responsável por guardar as informações do evento
    /// </summary>
    public class MosesEvent
    {
        public MosesEventArgs Args;

        public MosesEvent(string eventMessage, MosesEventArgs e )
        {
            e.EventMessage = eventMessage;
            Args = e;
        } 

        public void Raise()
        {
            //Aqui ele tem que acessar o MosesEventProvider
            //o Provider deve Ter configurado o canal de comunicação padrão para feedback ao manager
            //a saída padrão deve ser para um socket TCP
            //MosesEvent mosesEvent = new MosesEvent("Divisão por zero.", sender, 0);
            //mosesEvent.Raise();
            //envia um relatório de erros para a Exodus
            if (MosesConfiguration.Default.EventProvider != null)
            {
                MosesEventProvider.Default.BeginProcessEvent(this);
            }
        }
    }

    [Serializable]
    public class MosesEventArgs : EventArgs
    {
        public MosesEventArgs() 
        {
            EventTime = DateTime.Now;
        }

        public MosesEventArgs(object value, MosesEventType e)
        {
            EventTime = DateTime.Now;
            Value = value;
            EventType = e;
        }

        public string EventMessage;
        public object Value;
        public DateTime EventTime;
        public MosesEventType EventType;
        
    }

    public enum MosesEventType
    {
        SqlTrace,
        ConnectionTest,
        ExceptionNotification
    }
}
