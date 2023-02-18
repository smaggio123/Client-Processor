using ClientApp.Models;
using ClientApp.Views;
using ReactiveUI;
using Server;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;

namespace ClientApp.ViewModels
{
    public class FormFillingViewModel : ReactiveObject, IRoutableViewModel
    {
        public List<FormInputField> InputFields { get; } = new List<FormInputField>();
        public ObservableCollection<string> UserInputList { get; set; } = new ObservableCollection<string>();

        private List<string> _TextInputList = new List<string>();

        public List<string> TextInputList
        {
            get => _TextInputList;
            set
            {
                _TextInputList = value;
                this.RaiseAndSetIfChanged(ref _TextInputList, value);
            }
        }

        public Server.Procedure.ProcedureClient procedureClient;
        public FormName name;
        public Server.FormFields formFields;
        List<string> ListOfFieldNames = new();

        public static List<Server.Field> ListOfFields { get; set; } = new();

        public static RoutingState Router { get; set; } = new RoutingState();
        public static ReactiveCommand<Unit, IRoutableViewModel>? NavigateToFormMenu { get; set; }
        public FormFillingViewModel()
        {
            ListOfFields = new();
            //RouterToFormMenu = new();
            Locator.CurrentMutable.Register(() => new FormMenuView(), typeof(IViewFor<FormMenuView>));
            NavigateToFormMenu = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FormMenuViewModel()));

            procedureClient = new Procedure.ProcedureClient(Program.gRPCChannel);
            name = new() { FormName_ = FormMenuViewModel.FormName };
            formFields = procedureClient.getFormFields(name);
            //int index = 0;
            foreach (var field in formFields.Fields)
            {
                if (field.FieldType.Equals("iText.Forms.Fields.PdfTextFormField"))
                {
                    ListOfFields.Add(field);
                }
            }
        }

        public void SubmitFormCommand()
        {
            if(NavigateToFormMenu!=null)NavigateToFormMenu.Execute();
        }

        //This is for the IRoutableViewModel class
        public string? UrlPathSegment => throw new System.NotImplementedException();
        //This is for the IRoutableViewModel class
        public IScreen HostScreen => throw new System.NotImplementedException();
    }
}
