using BusinessModels.View;
using DataProcessing.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Security;

namespace DataPresentationUI
{
    public partial class EmployeesViewer : Form
    {
        public EmployeesViewer(IService services)
        {
            InitializeComponent();

            _services = services;

            Controls.Add(selectButton);
        }

        private IService _services;

        private void selectButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var filePath = openFileDialog1.FileName;

                    var employees = _services.GetLongestEmployeesParticipation(filePath);

                    PopulateDataGridView(employees);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void PopulateDataGridView(IEnumerable<LongestDaysWorkedEmployeesPairView> employeesPair)
        {
            if (!employeesPair.Any())
            {
                return;
            }
            PropertyInfo[] properties = employeesPair.First().GetType().GetProperties().ToArray();
            foreach(var prop in properties)
            {
                var attrib = GetAttribute<DisplayNameAttribute>(prop, true);

                dataGridView1.Columns.Add(prop.Name, attrib.DisplayName);
            }

            foreach (var pair in employeesPair)
            {
                dataGridView1.Rows.Add(pair.EmployeeId1, pair.EmployeeId2, pair.ProjectId, pair.DaysWorked);
            }
        }

        private T GetAttribute<T>(PropertyInfo propInfo, bool isRequired)
            where T : Attribute
        {
            var attribute = propInfo.GetCustomAttributes(typeof(T)).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException($"The {typeof(T).Name} attribute must be defined on member {propInfo.Name}");
            }

            return (T)attribute;
        }
    }
}