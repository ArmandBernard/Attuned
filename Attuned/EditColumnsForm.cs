using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Attuned
{
    public partial class EditColumnsForm : Form
    {
        /// <summary>
        /// All selected items
        /// </summary>
        public List<string> Selected 
        {
            get { return ListCLB.CheckedItems.Cast<string>().ToList(); } 
        }

        public EditColumnsForm(List<string> allItems, List<string> toSelect)
        {
            InitializeComponent();

            // add all items
            allItems.ForEach(c => ListCLB.Items.Add(c));

            // manually select those requested
            for (int i = 0; i < ListCLB.Items.Count; i++)
            {
                // if in saved list
                if (toSelect.Contains(ListCLB.Items[i]))
                {
                    // check it
                    ListCLB.SetItemChecked(i, true);
                }
            }
        }

        private void EditColumnsForm_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
