using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SF_Tools.Editor
{
	public class EditorData<T>
    {
        #region Private Members

        private T data = default(T);
        private string name = string.Empty;
        private bool isEditing = false;

        #endregion  

        #region Public Properties

        public T Data
        {
            get { return data; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsEditing
        {
            get { return isEditing; }
            set { isEditing = value; }
        }

        #endregion

        #region Public Interface

        public EditorData(T newData, string name)
        {
            this.data = newData;
            this.name = name;
        }

        #endregion
    }
}
