using AIT.DataBases;
using AIT.DataBases.DBModel;
using AIT.Helpers;
using AIT.Models;
using AIT.Windows;
using AITLib.Constants;
using AITLib.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AIT.Pages.PartlyPages
{
    public partial class AccountFieldFrame : Page
    {
        private Dictionary<string, object> controlDic;

        public AccountFieldFrame()
        {
            InitializeComponent();
            Init();
        }

        public void Refresh()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            controlDic = new Dictionary<string, object>
            {
                [AitStrings.FNAME_C] = aitFName,
                [AitStrings.LNAME_C] = aitLName,
                [AitStrings.EMAIL_C] = aitEmail,
                [AitStrings.LOGIN_C] = aitLogin,
                [AitStrings.PASSWORD_C] = aitPassword,
                [AitStrings.REPEAT_C] = aitRepeat,
                [AitStrings.BDATE_C] = aitBDate,
                [AitStrings.PESEL_C] = aitPesel,
            };
        }

        public bool ControlsValidation()
        {
            var error = false;
            if (controlDic[AitStrings.EMAIL_C] is TextBox email)
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match((string)email.Text);
                if (match.Success)
                {
                    email.BorderBrush = Background;
                    email.BorderThickness = new Thickness(0);
                }
                else
                {
                    error = true;
                    email.BorderBrush = Brushes.Red;
                    email.BorderThickness = new Thickness(2);
                }

                if (AitDBContextInstance.Instance.AitPersons.Any())
                {
                    var findEmail = AitDBContextInstance.Instance.AitPersons.Where(p => p.Email.Equals(email.Text));
                    if (findEmail == null || AitDebugHelper.RunningInDebugMode())
                    {
                        email.BorderBrush = Background;
                        email.BorderThickness = new Thickness(0);
                    }
                    else if (findEmail.Count() > 1 || findEmail.Any(q => !q.ID.Equals(AitSessionManager.GetSession.Person.ID)))
                    {
                        error = true;
                        email.BorderBrush = Brushes.Red;
                        email.BorderThickness = new Thickness(2);

                        var msg = AitStrings.EMAIL_FOUND;
                        var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                        var dialog = new DialogWindow(dialogModel);
                        dialog.Show();
                    }

                }
            }

            if(controlDic[AitStrings.LOGIN_C] is TextBox login)
            {
                if(!string.IsNullOrWhiteSpace(login.Text))
                {
                    login.BorderBrush = Background;
                    login.BorderThickness = new Thickness(0);
                }
                else
                {
                    error = true;
                    login.BorderBrush = Brushes.Red;
                    login.BorderThickness = new Thickness(2);

                    var msg = AitStrings.REQUIRED_FIELD;
                    var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                    var dialog = new DialogWindow(dialogModel);
                    dialog.Show();
                }
            }

            if (controlDic[AitStrings.PASSWORD_C] is PasswordBox pass &&
               controlDic[AitStrings.REPEAT_C] is PasswordBox repeat)
            {
                if (!pass.Password.Equals(repeat.Password) 
                    && !pass.Password.Equals(AitStrings.SIMPLE_PASSWORD))
                {
                    error = true;
                    pass.BorderBrush = Brushes.Red;
                    pass.BorderThickness = new Thickness(2);
                    repeat.BorderBrush = Brushes.Red;
                    repeat.BorderThickness = new Thickness(2);

                    var msg = AitStrings.PASS_NOT_EQ;
                    var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                    var dialog = new DialogWindow(dialogModel);
                    dialog.Show();
                }
                else
                {
                    pass.BorderBrush = Background;
                    pass.BorderThickness = new Thickness(0);
                    repeat.BorderBrush = Background;
                    repeat.BorderThickness = new Thickness(0);
                }
            }

            if (controlDic[AitStrings.BDATE_C] is DatePicker date)
            {
                if ((!date.SelectedDate.HasValue || date.SelectedDate.Value >= DateTime.Now.Date.AddYears(-18))
                     && !AitDebugHelper.RunningInDebugMode())
                {
                    error = true;
                    date.BorderBrush = Brushes.Red;
                    date.BorderThickness = new Thickness(2);

                    var msg = date.SelectedDate.HasValue ? AitStrings.CHILD_DATE : AitStrings.DATE_REQ;
                    var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                    var dialog = new DialogWindow(dialogModel);
                    dialog.Show();
                }
                else
                {
                    date.BorderBrush = Background;
                    date.BorderThickness = new Thickness(0);
                }
            }

            if (controlDic[AitStrings.PESEL_C] is TextBox pesel)
            {
                if ((pesel.Text.Length != 11 || ulong.TryParse(pesel.Text, out _))
                    && !string.IsNullOrEmpty(pesel.Text) 
                    && !AitDebugHelper.RunningInDebugMode())
                {
                    error = true;
                    pesel.BorderBrush = Brushes.Red;
                    pesel.BorderThickness = new Thickness(2);

                    var msg = AitStrings.INVALID_PESEL_NUMBER;
                    var dialogModel = new AitDialogModel(DialogType.ERROR, msg);
                    var dialog = new DialogWindow(dialogModel);
                    dialog.Show();
                }
                else
                {
                    pesel.BorderBrush = Background;
                    pesel.BorderThickness = new Thickness(0);
                }
            }
            return !error;
        }

        public async Task<bool> PersonRejestracion()
        {
            try
            {
                var person = new AitPerson
                {
                    Login = aitLogin.Text,
                    Password = AitGenerators.GenerateMD5Hash(aitPassword.Password),
                    Email = aitEmail.Text
                };

                var personDetail = new AitPersonsDetail
                {
                    PersonID = person.ID,
                    FName = aitFName.Text,
                    LName = aitLName.Text,
                    Pesel = aitPesel.Text,
                    BDate = (DateTime)aitBDate.SelectedDate,
                    Person = person
                };
                var quickNotes = new List<AitQuickNote>();

                person.PersonalDetails = personDetail;
                person.QuickNotes = quickNotes;

                AitDBContextInstance.Instance.AitPersons.Add(person);
                await AitDBContextInstance.Instance.SaveChangesAsync();

                var msg = string.Format(AitStrings.CREATE_ACCOUNT_SUCCESS, aitLogin.Text);
                var success = new DialogWindow(new AitDialogModel(DialogType.INFORMATION, msg));
                success.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(string.Format(AitStrings.CREATE_ACCOUNT_CRASH, aitLogin.Text));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                error.Show();
                return false;
            }
            return true;
        }

        public void PersonAccountInit()
        {
            try
            {
                var person = AitSessionManager.GetSession.Person;
                if (person != null && person.PersonalDetails != null)
                {
                    aitFName.Text = person.PersonalDetails.FName;
                    aitLName.Text = person.PersonalDetails.LName;
                    aitEmail.Text = person.Email;
                    aitLogin.Text = person.Login;
                    aitPassword.Password = AitStrings.SIMPLE_PASSWORD;
                    aitBDate.SelectedDate = person.PersonalDetails.BDate;
                    aitPesel.Text = person.PersonalDetails.Pesel;
                }
                else
                    throw new Exception(AitStrings.INIT_CRASH);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(AitStrings.INIT_CRASH);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                error.Show();
            }
        }

        public async Task SavePersonAccountChanges()
        {
            try
            {
                var person = AitSessionManager.GetSession.Person;
                person.Login = aitLogin.Text;
                person.Email = aitEmail.Text;
                person.Password = aitPassword.Password.Equals(aitRepeat.Password) 
                    && !AitStrings.SIMPLE_PASSWORD.Equals(aitPassword.Password)
                    && !AitStrings.SIMPLE_PASSWORD.Equals(aitRepeat.Password) ? 
                    AitGenerators.GenerateMD5Hash(aitPassword.Password) : person.Password;
                
                person.PersonalDetails.FName = aitFName.Text;
                person.PersonalDetails.LName = aitLName.Text;
                person.PersonalDetails.BDate = aitBDate.SelectedDate.Value;
                person.PersonalDetails.Pesel = aitPesel.Text;

                await AitDBContextInstance.Instance.SaveChangesAsync();

                var msg = string.Format(AitStrings.UPDATE_ACCOUNT_SUCCESS, aitLogin.Text);
                var success = new DialogWindow(new AitDialogModel(DialogType.INFORMATION, msg));
                success.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                AitExceptionsLogger.Instance.LogToFile(new AitLog { Title = AitLogTitle.EXCEPTION, Message = ex.ToString() });

                var stringBuilder = new StringBuilder();
                stringBuilder.Append(string.Format(AitStrings.UPDATE_ACCOUNT_CRASH, aitLogin.Text));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append(AitStrings.CRASH_INFORMATION);
                var error = new DialogWindow(new AitDialogModel(DialogType.ERROR, stringBuilder.ToString()));
                error.Show();
            }
        }
    }
}
