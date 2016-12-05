using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using Ozeki.Media;
using Ozeki.VoIP;
// using Ozeki.VoIP.SDK;

namespace WindowsFormsApplication2
{
    static class Program
    {
        private static Softphone _mySoftphone;   // softphone object.
        private static string _numberToCall;     // indicates the last called number. can be used for redialing.
        private static string _numberToTransfer;
        private static string _exampleSteps;     // guides the user through the steps
        private static Form1 _MainWin;
        private static CallHandler _CallHandler;
    
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Ozeki.Common.LicenseManager.Instance.SetLicense("OZSDK-TAL32CALL-140520-EE768FF1", "TUNDOjMyLE1QTDozMixHNzI5OnRydWUsTVNMQzozMixNRkM6MzIsVVA6MjAxNS4wNS4yMCxQOjIxOTkuMDEuMDF8d2VqU2lEWmFxTGpENmNsZ0s1ZEdOTUwxRkRtMGwxckdObkRIcHk2Z1hQKzAwMjVzSDduSUpZL0s0U1BxOHVIclZleTlZckNFZGp2RzBQK29ZZGtxSFE9PQ==");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _MainWin = new Form1();
            Application.Run(_MainWin);

        }

        public static void InitSF()
        {
            InitSoftphone();
            ReadRegisterInfos();
        }

        public static void StartCall(string ToDail)
        {
           
            _mySoftphone.StartCall(ToDail);
            _mySoftphone.mediaSender().AttachToCall(_mySoftphone.Call());
            _CallHandler = new CallHandler(_mySoftphone, _mySoftphone.Call(), _mySoftphone.mediaSender());
            //MessageBox.Show("wait");
            _CallHandler.Start();
        }

        /// <summary>
        /// Initializes the softphone logic and subscribes to its events to get notifications from it.
        /// (eg. the registration state of the phone line has changed or an incoming call received)
        /// </summary>
        private static void InitSoftphone()
        {
            _mySoftphone = new Softphone();
            _mySoftphone.PhoneLineStateChanged += mySoftphone_PhoneLineStateChanged;
            _mySoftphone.CallStateChanged += mySoftphone_CallStateChanged;

            _numberToTransfer = string.Empty;
            _numberToCall = string.Empty;
            _exampleSteps = string.Empty;
        }

        /// <summary>
        /// This will be called when the registration state of the phone line has changed.
        /// </summary>
        static void mySoftphone_PhoneLineStateChanged(object sender, RegistrationStateChangedArgs e)
        {
            if (e.State == RegState.RegistrationSucceeded)
            {
                _MainWin.WriteMsg("Registration succeeded - Online!");
            }
            else if (e.State == RegState.NotRegistered)
            {
                _MainWin.WriteMsg("Not registered");
            }
            else if (e.State == RegState.Error)
            {
                _MainWin.WriteMsg("Registration Error!");
            }
        }

     
        /// <summary>
        /// This will be called when the state of the call has changed. (eg. ringing, answered, rejected)
        /// With the help of the exampleSteps variable, the steps can be followed one by one.
        /// </summary>
        private static void mySoftphone_CallStateChanged(object sender, CallStateChangedArgs e)
        {

         
            if (e.State == CallState.Answered && _exampleSteps == "Calling")
            {
                _exampleSteps = "Held";
                _mySoftphone.HoldCall();
                _MainWin.WriteMsg("The call is: " + _exampleSteps + " by the user!");    

            }

            if (e.State == CallState.LocalHeld)
            {
                _exampleSteps = "Unheld";
                MessageBox.Show("Press any key to take the call off hold.");
                _MainWin.WriteMsg("The call is: " + _exampleSteps + " by the user!");
                _mySoftphone.HoldCall();
            }

            if (e.State == CallState.InCall && _exampleSteps == "Unheld")
            {
                _exampleSteps = "HangedUp";
                _MainWin.WriteMsg("The call is: " + _exampleSteps + " by the user!");
                _mySoftphone.HangUp();
            }

            if (e.State == CallState.Completed && _exampleSteps == "HangedUp")
            {
                _exampleSteps = "Redialed";
                MessageBox.Show("Press any key to redial.");
                _MainWin.WriteMsg("The call is: " + _exampleSteps + " by the user!");
                _mySoftphone.StartCall(_numberToCall);
            }

            //if (e.State == CallState.Answered && _exampleSteps == "Redialed")
            //{
            //    _exampleSteps = "Transfering";
            //    _mySoftphone.TransferTo(_numberToTransfer);
            //    _MainWin.WriteMsg("The call is: " + _exampleSteps + " by the user!");
            //}
        }

   
        /// <summary>
        /// Reads the SIP account information from the standard user input (some of these have default values).
        /// If the softphone cannot be registered, the user will be asked about the correct informations again.
        /// </summary>
        private static void ReadRegisterInfos()
        {
            var registrationRequired = true;
            var displayName = "miki";
            var userName = "4555";
            var authenticationId = "miki";
            var registerPassword = "12345";       
            var domainHost = "10.17.0.201";
            var domainPort = 5060;
     
            // When every information has been given, we are trying to register to the PBX with the softphone's Register() method.
            _mySoftphone.Register(registrationRequired, displayName, userName, authenticationId, registerPassword, domainHost, domainPort);
        }

    }
}
