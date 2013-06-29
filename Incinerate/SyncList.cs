using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel; 

namespace Incinerate
{
    public class SyncList<T> : BindingList<T>
    {
        private ISynchronizeInvoke m_SyncObject;
        private Action<ListChangedEventArgs> m_FireEventAction;
        
        public SyncList()
            : this(null)
        {
        }
        
        public SyncList(ISynchronizeInvoke syncObject)
        {
            m_SyncObject = syncObject;
            m_FireEventAction = FireEvent;
        }
        
        protected override void OnListChanged(ListChangedEventArgs args)
        {
            if (m_SyncObject == null)
            {
                FireEvent(args);
            }
            else
            {
                m_SyncObject.Invoke(m_FireEventAction, new object[] { args });
            }
        }
        
        private void FireEvent(ListChangedEventArgs args)
        {
            base.OnListChanged(args);
        }
    } 
}
