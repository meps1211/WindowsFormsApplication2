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
    class CallHandler
    {
        Softphone _softphone;

        IPhoneCall _Call;
        MediaConnector _connector;
        PhoneCallAudioSender _mediaSender;
        MP3StreamPlayback _MP3file;

        public CallHandler(Softphone softphone, IPhoneCall Call, PhoneCallAudioSender mediaSender)
        {
            _softphone = softphone;
 
            _Call = Call;
            _mediaSender = new PhoneCallAudioSender();
            _connector = new MediaConnector();
            _mediaSender = mediaSender;
        }

        public event EventHandler Completed;

        public void Start()
        {

            _Call.CallStateChanged += OutgoingCallStateChanged;
            //_mediaSender.AttachToCall(_Call); 
            _Call.DtmfReceived += DtmfReceived;
        }

        void DtmfReceived(object sender, VoIPEventArgs<DtmfInfo> e)
        {
            //_dtmfPressed = true;
            //_dtmfChain += DtmfNamedEventConverter.DtmfNamedEventsToString(e.Item.Signal.Signal);
            int Dtmf = e.Item.Signal.Signal;
          //  MessageBox.Show(Dtmf.ToString());
            if (Dtmf.ToString() == "1")
            {
                _Call.BlindTransfer("039295559");
            }   
        }

        private void OutgoingCallStateChanged(object sender, CallStateChangedArgs e)
        {
            if (e.State == CallState.Answered)
            {
                PlayMsg();
            }
            else if (e.State.IsCallEnded())
            {
                var handler = Completed;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        private void PlayMsg()
        {
            _MP3file = new MP3StreamPlayback("c:/tmp/test.mp3");
            
            //_mediaSender.AttachToCall(_Call);
            _connector.Connect(_MP3file, _mediaSender);
            _MP3file.Start();
            //var textToSpeech = new TextToSpeech();
            //_connector.Connect(textToSpeech, _mediaSender);
            //_mediaSender.AttachToCall(_Call);
            //textToSpeech.AddAndStartText("1111  111111111111");
           
            MessageBox.Show("wait");
            //_MP3file.Dispose();
        }
    }
}