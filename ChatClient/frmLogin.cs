using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace ChatClient
{
    public partial class frmLogin : Form
    {
        private bool canClose;
        private Server.CommandClient.CMDClient client;
        public Server.CommandClient.CMDClient Client
        {
            get { return client; }
        }
        public frmLogin()
        {
            InitializeComponent();
            txtIp.Text = "127.0.0.1";
            txtPorta.Text = "12345";
            this.canClose = false;
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }

        private void client_ConnectingFailed(object sender , EventArgs e)
        {
            //TODO MSG BOX
            //frmPopup popup = new frmPopup(PopupSkins.SmallInfoSkin);
            //popup.ShowPopup("Error" , "Server Is Not Accessible !" , 200 , 2000 , 2000);
            MessageBox.Show("Servidor não acessível");
            this.SetEnablity(true);
        }

        private void client_ConnectingSuccessed(object sender , EventArgs e)
        {
           this.client.SendCommand(new Server.CommandClient.Command(Server.CommandClient.CommandType.IsNameExists,this.client.IP,this.client.NetworkName));
        }

        void CommandReceived(object sender , Server.CommandClient.CommandEventArgs e)
        {
            if ( e.Command.CommandType == Server.CommandClient.CommandType.IsNameExists )
            {
                if ( e.Command.MetaData.ToLower() == "true" )
                {
                    //TODO MSG BOX
                    MessageBox.Show("Nome em uso");
                    //frmPopup popup = new frmPopup(PopupSkins.SmallInfoSkin);
                    //popup.ShowPopup("Error" , "The Username is already exists !" , 300 , 2000 , 2000);
                    this.client.Disconnect();
                    this.SetEnablity(true);
                }
                else
                {
                    this.canClose = true;
                    this.Close();
                }
            }
        }

        private void LoginToServer()
        {
            if ( this.txtUsetName.Text.Trim() == "" )
            {
                //TODO MSG BOX
                MessageBox.Show("Nome vazio");
                //frmPopup popup = new frmPopup(PopupSkins.SmallInfoSkin);
                //popup.ShowPopup("Error" , "Username is empty !" , 1000 , 2000 , 2000);
                this.SetEnablity(true);
            }
            else
            {
                this.client.NetworkName = this.txtUsetName.Text.Trim();
                this.client.ConnectToServer();
            }
        }
        private void btnEnter_Click(object sender , EventArgs e)
        {
            this.client = new Server.CommandClient.CMDClient(IPAddress.Parse(txtIp.Text), Convert.ToInt32(txtPorta.Text), "None");
            this.client.CommandReceived += new Server.CommandClient.CommandReceivedEventHandler(CommandReceived);
            this.client.ConnectingSuccessed += new Server.CommandClient.ConnectingSuccessedEventHandler(client_ConnectingSuccessed);
            this.client.ConnectingFailed += new Server.CommandClient.ConnectingFailedEventHandler(client_ConnectingFailed);

            this.SetEnablity(false);
            this.LoginToServer();
        }
        private void SetEnablity(bool enable)
        {
            this.btnEnter.Enabled = enable;
            this.txtUsetName.Enabled = enable;
            this.btnExit.Enabled = enable;
        }

        private void btnExit_Click(object sender , EventArgs e)
        {
            this.canClose = true;
        }

        private void frmLogin_FormClosing(object sender , FormClosingEventArgs e)
        {
            if ( !this.canClose )
                e.Cancel = true;
            else
                this.client.CommandReceived -= new Server.CommandClient.CommandReceivedEventHandler(CommandReceived);
        }
    }
}