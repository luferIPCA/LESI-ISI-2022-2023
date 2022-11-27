﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApp.WS {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WS.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetData", ReplyAction="http://tempuri.org/IService1/GetDataResponse")]
        string GetData();
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/GetData", ReplyAction="http://tempuri.org/IService1/GetDataResponse")]
        System.IAsyncResult BeginGetData(System.AsyncCallback callback, object asyncState);
        
        string EndGetData(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetHotelCidade", ReplyAction="http://tempuri.org/IService1/GetHotelCidadeResponse")]
        System.Data.DataSet GetHotelCidade(string cidade);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IService1/GetHotelCidade", ReplyAction="http://tempuri.org/IService1/GetHotelCidadeResponse")]
        System.IAsyncResult BeginGetHotelCidade(string cidade, System.AsyncCallback callback, object asyncState);
        
        System.Data.DataSet EndGetHotelCidade(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : ConsoleApp.WS.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetDataCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetDataCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public string Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetHotelCidadeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetHotelCidadeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public System.Data.DataSet Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<ConsoleApp.WS.IService1>, ConsoleApp.WS.IService1 {
        
        private BeginOperationDelegate onBeginGetDataDelegate;
        
        private EndOperationDelegate onEndGetDataDelegate;
        
        private System.Threading.SendOrPostCallback onGetDataCompletedDelegate;
        
        private BeginOperationDelegate onBeginGetHotelCidadeDelegate;
        
        private EndOperationDelegate onEndGetHotelCidadeDelegate;
        
        private System.Threading.SendOrPostCallback onGetHotelCidadeCompletedDelegate;
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<GetDataCompletedEventArgs> GetDataCompleted;
        
        public event System.EventHandler<GetHotelCidadeCompletedEventArgs> GetHotelCidadeCompleted;
        
        public string GetData() {
            return base.Channel.GetData();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetData(System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetData(callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public string EndGetData(System.IAsyncResult result) {
            return base.Channel.EndGetData(result);
        }
        
        private System.IAsyncResult OnBeginGetData(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return this.BeginGetData(callback, asyncState);
        }
        
        private object[] OnEndGetData(System.IAsyncResult result) {
            string retVal = this.EndGetData(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetDataCompleted(object state) {
            if ((this.GetDataCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetDataCompleted(this, new GetDataCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetDataAsync() {
            this.GetDataAsync(null);
        }
        
        public void GetDataAsync(object userState) {
            if ((this.onBeginGetDataDelegate == null)) {
                this.onBeginGetDataDelegate = new BeginOperationDelegate(this.OnBeginGetData);
            }
            if ((this.onEndGetDataDelegate == null)) {
                this.onEndGetDataDelegate = new EndOperationDelegate(this.OnEndGetData);
            }
            if ((this.onGetDataCompletedDelegate == null)) {
                this.onGetDataCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetDataCompleted);
            }
            base.InvokeAsync(this.onBeginGetDataDelegate, null, this.onEndGetDataDelegate, this.onGetDataCompletedDelegate, userState);
        }
        
        public System.Data.DataSet GetHotelCidade(string cidade) {
            return base.Channel.GetHotelCidade(cidade);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginGetHotelCidade(string cidade, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetHotelCidade(cidade, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.Data.DataSet EndGetHotelCidade(System.IAsyncResult result) {
            return base.Channel.EndGetHotelCidade(result);
        }
        
        private System.IAsyncResult OnBeginGetHotelCidade(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string cidade = ((string)(inValues[0]));
            return this.BeginGetHotelCidade(cidade, callback, asyncState);
        }
        
        private object[] OnEndGetHotelCidade(System.IAsyncResult result) {
            System.Data.DataSet retVal = this.EndGetHotelCidade(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetHotelCidadeCompleted(object state) {
            if ((this.GetHotelCidadeCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetHotelCidadeCompleted(this, new GetHotelCidadeCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetHotelCidadeAsync(string cidade) {
            this.GetHotelCidadeAsync(cidade, null);
        }
        
        public void GetHotelCidadeAsync(string cidade, object userState) {
            if ((this.onBeginGetHotelCidadeDelegate == null)) {
                this.onBeginGetHotelCidadeDelegate = new BeginOperationDelegate(this.OnBeginGetHotelCidade);
            }
            if ((this.onEndGetHotelCidadeDelegate == null)) {
                this.onEndGetHotelCidadeDelegate = new EndOperationDelegate(this.OnEndGetHotelCidade);
            }
            if ((this.onGetHotelCidadeCompletedDelegate == null)) {
                this.onGetHotelCidadeCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetHotelCidadeCompleted);
            }
            base.InvokeAsync(this.onBeginGetHotelCidadeDelegate, new object[] {
                        cidade}, this.onEndGetHotelCidadeDelegate, this.onGetHotelCidadeCompletedDelegate, userState);
        }
    }
}